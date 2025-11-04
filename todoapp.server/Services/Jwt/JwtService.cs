using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using todoapp.server.Constants;
using todoapp.server.Exceptions;
using todoapp.server.Models;

namespace todoapp.server.Services.Jwt
{

    public class JwtService : IJwtService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtService(IConfiguration configuration)
        {
            _secretKey = configuration[ConfigurationConstants.SecretKeyJwtSettings] ??
                throw new EmptyConfigurationValueException();

            _issuer = configuration[ConfigurationConstants.SecretKeyIssuer] ??
                throw new EmptyConfigurationValueException();

            _audience = configuration[ConfigurationConstants.SecretKeyAudience] ??
                throw new EmptyConfigurationValueException();
        }

        public string GenerateToken(string username, long time)
        {
            var handler = new JwtSecurityTokenHandler();

            var privateKey = Encoding.UTF8.GetBytes(_secretKey);

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(privateKey),
                SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(1),
                Subject = GenerateClaims(username)
            };

            var token = handler.CreateEncodedJwt(tokenDescriptor);
            return token;
        }

        private static ClaimsIdentity GenerateClaims(string username)
        {
            var ci = new ClaimsIdentity();

            ci.AddClaim(new Claim("username", username));

            return ci;
        }
        public string ValidateToken(string token)
        {
            var result = ValidateTokenWithUserId(token);
            return result.username;
        }

         public async Task<string> GenerateAccessToken(List<Claim> claims, DateTime expiresAt)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public string GenerateToken(string username, int userId, long time)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())  // Add userId as NameIdentifier claim
        }),
                Expires = DateTime.UtcNow.AddMinutes(time),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateEncodedJwt(tokenDescriptor);
            return token;
        }

        public (string username, int? userId) ValidateTokenWithUserId(string token)
        {
            if (string.IsNullOrEmpty(token))
                return (null, null);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);
            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                if (validatedToken is JwtSecurityToken jwtToken &&
                    jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    var username = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                    var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                    int? userId = userIdClaim != null ? int.Parse(userIdClaim) : null;
                    return (username, userId);
                }
            }
            catch
            {
                // Token validation failed
                return (null, null);
            }
            return (null, null);

        }

        Task<string> IJwtService.GenerateAccessToken(List<Claim> claims, DateTime expiresAt)
        {
            return GenerateAccessToken(claims, expiresAt);
        }
    }
}
