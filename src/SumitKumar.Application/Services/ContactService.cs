using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SumitKumar.Application.Abstractions;
using SumitKumar.Application.DTOs;
using SumitKumar.Application.Options;
using SumitKumar.Domain.Entities;

namespace SumitKumar.Application.Services;

public sealed class ContactService(
    IContactRepository repo,
    IEmailSender email,
    IOptions<EmailOptions> options,
    ILogger<ContactService> logger) : IContactService
{
    public async Task SubmitAsync(ContactMessageDto message, CancellationToken ct = default)
    {
        var entity = new ContactMessage
        {
            FromName = message.FromName,
            FromEmail = message.FromEmail,
            Subject = message.Subject,
            Body = message.Body
        };

        await repo.AddAsync(entity, ct);
        await repo.SaveChangesAsync(ct);

        try
        {
            var html = $"<p><strong>From:</strong> {message.FromName} &lt;{message.FromEmail}&gt;</p>" +
                       $"<p><strong>Subject:</strong> {message.Subject}</p>" +
                       $"<hr/><div>{System.Net.WebUtility.HtmlEncode(message.Body).Replace("\n", "<br/>")}</div>";

            await email.SendAsync(
                to: options.Value.ContactInboxAddress,
                subject: $"[Portfolio Contact] {message.Subject}",
                htmlBody: html,
                replyTo: message.FromEmail,
                ct: ct);

            entity.DeliveredToInbox = true;
            await repo.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to deliver contact message from {Email}", message.FromEmail);
        }
    }
}
