using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using todoapp.server.Dtos.UserDto;
using todoapp.server.Enums;
using todoapp.server.Services.Jwt;
using todoapp.server.Services.Mail;
using todoapp.server.Utils;
using todoapp.server.Models;
using Newtonsoft.Json;

namespace todoapp.server.Services.iml
{
    public class AuthService
    {

        private readonly Prn231ProjectContext _context;
        private readonly IJwtService _jwtService;
        private readonly MailService _mailService;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LinkGenerator _linkGenerator;

        public AuthService(Prn231ProjectContext context, IJwtService jwtService, MailService mailService, IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
        {
            _context = context;
            _jwtService = jwtService;
            _mailService = mailService;
            _httpContextAccessor = httpContextAccessor;
            _linkGenerator = linkGenerator;
        }

        public async Task<UserLoginResponse> UserLogin(UserLoginRequest request, HttpContext httpContext)
        {
            // hash password to check 
            string passRequest = StringUtils.ComputeSha256Hash(request.Password);
            var user = await _context.Accounts.FirstOrDefaultAsync(x => x.UserName == request.Username || x.Email.ToLower() == request.Username.ToLower());
            if (user == null)
            {
                return new UserLoginResponse()
                {
                    Success = false,
                    Message = "Invalid username!",
                };
            }
            if (!passRequest.Equals(user.Password))
            {
                return new UserLoginResponse()
                {
                    Success = false,
                    Message = "Wrong password!",
                };
            }
            else
            {
                // add session 
                UserSessionManager.SetUserInfo(httpContext, user);

                string token = "";

                long oneday = 60 * 24;
                token = _jwtService.GenerateToken(user.UserName, user.Id, oneday);

                user.Password = "";
                // return success
                return new UserLoginResponse()
                {
                    Success = true,
                    Message = "Login successful!",
                    Key = token,
                    Account = user
                };
            }


        }
        public async Task<UserSignUpResponse> UserSignUp(UserSignUpRequest request)
        {

            var validationContext = new ValidationContext(request);
            var validateResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(request, validationContext, validateResults, true);
            if (!isValid)
            {
                // create list of error objects
                var errorList = validateResults.Select(x => new { error = x.ErrorMessage }).ToList();
                Console.WriteLine(errorList);
                // serialize json
                string errorJson = JsonConvert.SerializeObject(errorList);
                Console.WriteLine(errorJson);


                return new UserSignUpResponse()
                {
                    Success = false,
                    Message = errorJson
                };

            }

            // Check for existing username or email in a single query
            var existingUser = await _context.Accounts
                .Where(x => x.UserName == request.Username || x.Email.ToLower() == request.Email.ToLower())
                .Select(x => new { x.UserName, x.Email })
                .FirstOrDefaultAsync();

            if (existingUser != null)
            {
                string message = existingUser.UserName == request.Username
                    ? "Username already exists!" : "Email already exists!";

                return new UserSignUpResponse
                {
                    Success = false,
                    Message = message
                };
            }

            // Create new user
            var newUser = new Account
            {
                UserName = request.Username,
                Email = request.Email,
                Password = StringUtils.ComputeSha256Hash(request.Password),
                //CreateAt = DateTime.UtcNow,
                //Status = 1,
                Roll = (int)Roles.User
            };

            await _context.Accounts.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return new UserSignUpResponse()
            {
                Success = true,
                Message = "User registered successfully"
            };
        }


        public async Task<bool> ValidateToken(string token, HttpContext httpContext)
        {
            string usename = _jwtService.ValidateToken(token);
            if (usename != null)
            {
                var user = await _context.Accounts.FirstOrDefaultAsync(x => x.UserName == usename);
                if (user != null)
                {
                    UserSessionManager.SetUserInfo(httpContext, user);
                    return true;
                }
            }
            return false;
        }

        public UserLoginResponse ForgotPass(string key)
        {
            // Check for existing username or email in a single query
            var existingUser = _context.Accounts
            .Where(x => x.UserName == key || x.Email.ToLower() == key.ToLower())
            .Select(x => new { x.UserName,x.Id, x.Email })
                .FirstOrDefault();

            if (existingUser != null)
            {
                // generate token
                string token = _jwtService.GenerateToken(existingUser.UserName,existingUser.Id, 5);

                string linkToken = GeneratePasswordResetUrl(token);

                // send mail
                string message = "If an account exists with this " + existingUser.UserName == key ? "username" : "email" + ", you will receive password reset instructions.";

                string content = "<div style='text-align: center; font-family: Consolas; background-color:#1b263b; color:#e0e1dd; padding: 20px;border-radius: 10px'>\n" +
                        "    <h1 style='margin: 10px'>Hey is you forget Pass!</h1>\n" +
                        "    <nav style='margin: 20px;font-size: 12px; color:orangered'>*if you not require to change password, you can ignore this\n" +
                        "        email!\n" +
                        "    </nav>\n" +
                        "\n" +
                        "    <h4>Click link below to reset you password</h4>\n" +
                        "    <a style='padding:10px 20px;background-color:#e0e1dd; color: #0d1b2a; text-decoration: none; border-radius: 5px'\n" +
                        "       href='" + linkToken + "'>here</a>\n" +
                        "</div>\n";
                var header = "Reset your password | BookStore";

                _mailService.SendMail(content, existingUser.Email, header);

                return new UserLoginResponse
                {
                    Success = true,
                    Message = message
                };
            }
            else
            {
                return new UserLoginResponse()
                {
                    Message = "Not found any email or username",
                    Success = false
                };
            }
        }
        private string GeneratePasswordResetUrl(string token)
        {
            var request = _httpContextAccessor.HttpContext!.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            var resetLink = _linkGenerator.GetPathByAction(
                action: "ResetPassword",
                controller: "Authentication",
                values: new { token });

            return $"{baseUrl}{resetLink}";
        }

        public string CheckToken(string token)
        {
            return _jwtService.ValidateToken(token);
        }

        public bool ResetPassWord(string password, string repassword, string token)
        {
            // check token , time again
            var username = _jwtService.ValidateToken(token);
            if (string.IsNullOrEmpty(username)) { return false; }

            // find user
            var existUser = _context.Accounts.FirstOrDefault(x => x.UserName.Equals(username));
            if (existUser == null || password != repassword) return false;

            // change password, save change
            existUser.Password = StringUtils.ComputeSha256Hash(password);
            _context.SaveChanges();

            return true;
        }
    }
}
