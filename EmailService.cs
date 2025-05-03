using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ApiWithAuth;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task Send(EmailMessageModel model)
    {
        var emailSettings = _configuration.GetSection("EmailSettings");
        var client = new SmtpClient();
        client.Host = emailSettings["Host"];
        client.Port = emailSettings.GetValue<int>("Port");;
        client.Credentials = new NetworkCredential(emailSettings["Username"], emailSettings["Password"]);
        var message = new MailMessage();
        message.To.Add(new MailAddress(model.ToAddress));
        message.From = new MailAddress(emailSettings["FromEmail"], emailSettings["FromName"]);
        message.Subject = model.Subject;
        message.Body = model.Body;
        message.IsBodyHtml = true;
        await client.SendMailAsync(message);

        message.Dispose();
        client.Dispose();
    }
}

public interface IEmailService
{
    Task Send(EmailMessageModel model);
}

public class EmailMessageModel
{
    // userdan alınca eklenebilir
    // public string ToName { get; set; } 
    public required string ToAddress { get; set; }
    public required string Subject { get; set; }
    public required string Body { get; set; }
}

public class EmailSender : IEmailSender
{
    private readonly IEmailService _emailService;
    
    public EmailSender(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        await _emailService.Send(new EmailMessageModel
        {
            ToAddress = email,
            Subject = subject,
            Body = htmlMessage
        });
    }
}