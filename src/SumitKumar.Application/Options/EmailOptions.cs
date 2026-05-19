using System.ComponentModel.DataAnnotations;

namespace SumitKumar.Application.Options;

public sealed class EmailOptions
{
    public const string SectionName = "Email";

    [Required] public string Host { get; set; } = "localhost";
    [Range(1, 65535)] public int Port { get; set; } = 25;
    public bool EnableSsl { get; set; }
    public string? User { get; set; }
    public string? Password { get; set; }

    [Required, EmailAddress] public string FromAddress { get; set; } = "no-reply@example.com";
    [Required] public string FromName { get; set; } = "Portfolio";
    [Required, EmailAddress] public string ContactInboxAddress { get; set; } = "owner@example.com";
}
