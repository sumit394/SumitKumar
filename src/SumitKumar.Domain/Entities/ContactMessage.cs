using SumitKumar.Domain.Common;

namespace SumitKumar.Domain.Entities;

public class ContactMessage : Entity
{
    public string FromName { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public bool DeliveredToInbox { get; set; }
}
