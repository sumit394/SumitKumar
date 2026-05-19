using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SumitKumar.Application.Abstractions;
using SumitKumar.Application.Options;

namespace SumitKumar.Infrastructure.Email;

public sealed class SmtpEmailSender(IOptions<EmailOptions> options, ILogger<SmtpEmailSender> logger) : IEmailSender
{
    public async Task SendAsync(string to, string subject, string htmlBody, string? replyTo = null, CancellationToken ct = default)
    {
        var o = options.Value;
        using var msg = new MailMessage
        {
            From = new MailAddress(o.FromAddress, o.FromName),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };
        msg.To.Add(to);
        if (!string.IsNullOrWhiteSpace(replyTo)) msg.ReplyToList.Add(replyTo);

        using var client = new SmtpClient(o.Host, o.Port) { EnableSsl = o.EnableSsl };
        if (!string.IsNullOrEmpty(o.User))
        {
            client.Credentials = new NetworkCredential(o.User, o.Password);
        }

        logger.LogInformation("Sending email to {To} via {Host}:{Port}", to, o.Host, o.Port);
        await client.SendMailAsync(msg, ct);
    }
}

/// <summary>Logs the message instead of sending. Used in Development.</summary>
public sealed class NullEmailSender(ILogger<NullEmailSender> logger) : IEmailSender
{
    public Task SendAsync(string to, string subject, string htmlBody, string? replyTo = null, CancellationToken ct = default)
    {
        logger.LogInformation("[DEV-EMAIL] To={To} Subject={Subject} ReplyTo={ReplyTo}\n{Body}", to, subject, replyTo, htmlBody);
        return Task.CompletedTask;
    }
}
