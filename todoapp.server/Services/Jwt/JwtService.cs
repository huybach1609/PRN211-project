using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using todoapp.server.Services.Jwt;

namespace todoapp.server.Services.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly string _secretKey;

        public JwtService(IConfiguration configuration)
        {
            _secretKey = configuration["JwtSettings:SecretKey"];
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

        // Keep existing ValidateToken method for backward compatibility
        public string? ValidateToken(string token)
        {
            var result = ValidateTokenWithUserId(token);
            return result.username;
        }

        // Add new method that returns both username and userId
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
        //public string? ValidateToken(string token)
        //{
        //    if (string.IsNullOrEmpty(token))
        //        return null;

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.UTF8.GetBytes(_secretKey);

        //    try
        //    {
        //        var validationParameters = new TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(key),
        //            ValidateIssuer = false,
        //            //ValidIssuer = "http://localhost:5085",
        //            ValidateAudience = false,
        //            //ValidAudience = "http://localhost:5085",
        //            ValidateLifetime = true,
        //            ClockSkew = TimeSpan.Zero // Ensure token expiry time is strict
        //        };

        //        var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

        //        if (validatedToken is JwtSecurityToken jwtToken &&
        //            jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        //        {
        //            return principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        //        }
        //    }
        //    catch
        //    {
        //        // Token validation failed
        //        return null;
        //    }
        //    return null;
        //}
    }
}
