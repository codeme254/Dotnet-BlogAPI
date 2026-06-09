using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace BlogAPI.Services.Implementations;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpSenderEmail;
    private readonly string _smtpPassword;

    public EmailService(IConfiguration config)
    {
        _config = config;
        _smtpServer = _config.GetValue("EmailSettings:SmtpServer", "");
        _smtpPort = _config.GetValue("EmailSettings:Port", 0);
        _smtpUsername = _config.GetValue("EmailSettings:Username", "");
        _smtpSenderEmail = _config.GetValue("EmailSettings:SenderEmail", "");
        _smtpPassword = _config.GetValue("EmailSettings:Password", "");
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_smtpUsername, _smtpSenderEmail));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_smtpSenderEmail, _smtpPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}

