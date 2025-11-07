using System.Security.Claims;
using todoapp.server.Models;

namespace todoapp.server.Services.Jwt
{
    public interface IJwtService
    {

        /// <summary>
        /// Generates an access token with the given claims and expiration time. 
        /// </summary>
        /// <param name="claims">list of claims </param>
        /// <param name="expiresAt">type date time </param>
        /// <returns></returns>
        Task<string> GenerateAccessToken(List<Claim> claims, DateTime eẠkipiresAt, CancellationToken ct);

        public string GenerateToken(string username, int userId, long time);
        string ValidateToken(string token);
        public (string username, int? userId) ValidateTokenWithUserId(string token);

    }
}
