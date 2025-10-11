namespace todoapp.server.Services.Jwt
{
    public interface IJwtService
    {
        // time = minutes
        public string GenerateToken(string username, int userId, long time);
        string ValidateToken(string token);
        public (string username, int? userId) ValidateTokenWithUserId(string token);

    }
}
