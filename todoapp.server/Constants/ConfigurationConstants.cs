namespace todoapp.server.Constants
{
    public static class ConfigurationConstants
    {
        public const string DefaultConnectionString = "ConnectionStrings:Default";
        public const string RemoteConnectionString = "ConnectionStrings:Remote";

        public const string SecretKeyJwtSettings = "JwtSettings:SecretKey";
        public const string SecretKeyIssuer = "JwtSettings:Issuer";
        public const string SecretKeyAudience = "JwtSettings:Audience";

        public const string SmtServerMailSettings = "MailSettings:smtpServer";
        public const string PortMailSettings= "MailSettings:port";
        public const string UsernameMailSettings = "MailSettings:username";
        public const string PasswordMailSettings = "MailSettings:password";

        public const string AllowedCorsOrigins = "AllowedCorsOrigins";
    }
}
