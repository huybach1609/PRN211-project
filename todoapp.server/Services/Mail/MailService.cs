using System.Net.Mail;
using System.Net;
using todoapp.server.Constants;
using todoapp.server.Exceptions;


namespace todoapp.server.Services.Mail
{
    public class MailService
    {
        private readonly string _smtpServer;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;
        private readonly bool _enableSsl;

        public MailService(IConfiguration configuration)
        {
            _smtpServer = configuration[ConfigurationConstants.SmtServerMailSettings]
               ?? throw new  EmptyConfigurationValueException();
            _port = Convert.ToInt32(configuration[ConfigurationConstants.PortMailSettings]
               ?? throw new  EmptyConfigurationValueException());
                
            _username = configuration[ConfigurationConstants.UsernameMailSettings] 
               ?? throw new  EmptyConfigurationValueException();
                
            _password = configuration[ConfigurationConstants.PasswordMailSettings]
               ?? throw new  EmptyConfigurationValueException();
                
            _enableSsl = true;
        }
        public async Task SendMailAsync(string content, string to, string header)
        {
            try
            {
                using (var client = new SmtpClient(_smtpServer, _port))
                {
                    client.EnableSsl = _enableSsl;
                    client.Credentials = new NetworkCredential(_username, _password);

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_username),
                        Subject = header,
                        Body = content,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(to);

                    await client.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send email: {ex.Message}", ex);
            }
        }

        public void SendMail(string content, string to, string header)
        {
            SendMailAsync(content, to, header).GetAwaiter().GetResult();
        }

    }
}
