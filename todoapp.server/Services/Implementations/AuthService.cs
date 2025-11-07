using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using todoapp.server.Dtos.UserDtos;
using todoapp.server.Enums;
using todoapp.server.Models;
using todoapp.server.Services.Interfaces;
using todoapp.server.Services.Jwt;
using todoapp.server.Services.Mail;
using todoapp.server.Utils;

namespace todoapp.server.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly Prn231ProjectContext _context;
        private readonly MailService _mailService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Token lifetimes (minutes)
        private const int AccessTokenMinutes = 60 * 24; // 1 day
        private const int ResetTokenMinutes = 5;       // 5 minutes

        public AuthService(
            Prn231ProjectContext context,
            IJwtService jwtService,
            MailService mailService,
            IHttpContextAccessor httpContextAccessor,
            LinkGenerator linkGenerator)
        {
            _context = context;
            _jwtService = jwtService;
            _mailService = mailService;
            _httpContextAccessor = httpContextAccessor;
            _linkGenerator = linkGenerator;
        }

        public async Task<UserLoginResponse> LoginAsync(UserLoginRequest request, CancellationToken ct)
        {
            var passRequest = StringUtils.ComputeSha256Hash(request.Password);

            // AsNoTracking to avoid mutating tracked entity later
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.UserName == request.Username
                      || x.Email.ToLower() == request.Username.ToLower(),
                    ct);

            if (user is null)
            {
                return new UserLoginResponse { Result = false, Message = "Invalid username!" };
            }

            if (!string.Equals(passRequest, user.Password, StringComparison.Ordinal))
            {
                return new UserLoginResponse { Result = false, Message = "Wrong password!" };
            }

            // Set session
            var httpContext = _httpContextAccessor.HttpContext!;
            UserSessionManager.SetUserInfo(httpContext, user);

            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Email, user.Email ?? ""),
                new (ClaimTypes.Name, user.UserName ?? ""),
                new (ClaimTypes.Role, user.Role.ToString() ?? ""),
            };
            
            var token = await _jwtService.GenerateAccessToken(claims, DateTime.UtcNow.AddHours(1), ct);

            var safeUser = new User
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Password = string.Empty,
                Role = user.Role
            };


            return new UserLoginResponse
            {
                Result = true,
                Message = "Login successful!",
                AccessToken = token,
            };
        }

        public async Task<UserSignUpResponse> SignUpAsync(UserSignUpRequest request, CancellationToken ct)
        {
            var validationContext = new ValidationContext(request);
            var validateResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(request, validationContext, validateResults, true);

            if (!isValid)
            {
                var errorList = validateResults.Select(x => new { error = x.ErrorMessage }).ToList();
                var errorJson = JsonConvert.SerializeObject(errorList);
                return new UserSignUpResponse { Success = false, Message = errorJson };
            }

            // One roundtrip to check conflicts
            var exists = await _context.Users
                .AnyAsync(x => x.UserName == request.Username
                            || x.Email.ToLower() == request.Email.ToLower(), ct);

            if (exists)
            {
                // Resolve which field collides for clearer message (two small extra queries; still fine)
                var usernameTaken = await _context.Users.AnyAsync(x => x.UserName == request.Username, ct);
                var message = usernameTaken ? "Username already exists!" : "Email already exists!";

                return new UserSignUpResponse { Success = false, Message = message };
            }

            var newUser = new User
            {
                UserName = request.Username,
                Email = request.Email,
                Password = StringUtils.ComputeSha256Hash(request.Password),
                Role = (int)Roles.User
            };

            await _context.Users.AddAsync(newUser, ct);
            await _context.SaveChangesAsync(ct);

            return new UserSignUpResponse
            {
                Success = true,
                Message = "User registered successfully"
            };
        }

        public async Task<bool> ValidateTokenAsync(string token, CancellationToken ct)
        {
            var username = _jwtService.ValidateToken(token);
            if (string.IsNullOrEmpty(username)) return false;

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserName == username, ct);

            if (user is null) return false;

            var httpContext = _httpContextAccessor.HttpContext!;
            UserSessionManager.SetUserInfo(httpContext, user);
            return true;
        }

        public async Task<UserLoginResponse> ForgotPasswordAsync(string key, CancellationToken ct)
        {
            var existingUser = await _context.Users
                .AsNoTracking()
                .Where(x => x.UserName == key || x.Email.ToLower() == key.ToLower())
                .Select(x => new { x.UserName, x.Id, x.Email })
                .FirstOrDefaultAsync(ct);

            if (existingUser is null)
            {
                return new UserLoginResponse { Result= false, Message = "Not found any email or username" };
            }

            var token = _jwtService.GenerateToken(existingUser.UserName, existingUser.Id, ResetTokenMinutes);
            var linkToken = GeneratePasswordResetUrl(token);

            // Fix operator precedence bug from original message
            var byUserName = string.Equals(existingUser.UserName, key, StringComparison.Ordinal);
            var message = $"If an account exists with this {(byUserName ? "username" : "email")}, you will receive password reset instructions.";

            var content =
                "<div style='text-align: center; font-family: Consolas; background-color:#1b263b; color:#e0e1dd; padding: 20px;border-radius: 10px'>" +
                "  <h1 style='margin: 10px'>Hey, you requested a password reset!</h1>" +
                "  <nav style='margin: 20px;font-size: 12px; color:orangered'>*If you didn't request this, you can ignore this email.</nav>" +
                "  <h4>Click the link below to reset your password</h4>" +
                $"  <a style='padding:10px 20px;background-color:#e0e1dd; color: #0d1b2a; text-decoration: none; border-radius: 5px' href='{linkToken}'>Reset password</a>" +
                "</div>";

            var header = "Reset your password | TodoApp";
            _mailService.SendMail(content, existingUser.Email, header);

            return new UserLoginResponse { Result= true, Message = message };
        }

        public async Task<bool> ResetPasswordAsync(string password, string repassword, string token, CancellationToken ct)
        {
            var username = _jwtService.ValidateToken(token);
            if (string.IsNullOrEmpty(username)) return false;
            if (!string.Equals(password, repassword, StringComparison.Ordinal)) return false;

            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username, ct);
            if (user is null) return false;

            user.Password = StringUtils.ComputeSha256Hash(password);
            await _context.SaveChangesAsync(ct);
            return true;
        }

        private string GeneratePasswordResetUrl(string token)
        {
            var http = _httpContextAccessor.HttpContext;
            if (http is null) return string.Empty;

            var request = http.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var path = _linkGenerator.GetPathByAction(
                action: "ResetPassword",
                controller: "Authentication",
                values: new { token }) ?? "/";

            return $"{baseUrl}{path}";
        }
         }
}
