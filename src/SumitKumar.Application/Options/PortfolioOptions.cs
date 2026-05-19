using System.ComponentModel.DataAnnotations;

namespace SumitKumar.Application.Options;

/// <summary>
/// Strongly-typed configuration for the public portfolio site.
/// Bound from the "Portfolio" section of appsettings.json.
/// </summary>
public sealed class PortfolioOptions
{
    public const string SectionName = "Portfolio";

    [Required] public ProfileSection Profile { get; set; } = new();
    public List<SocialLink> Social { get; set; } = new();
    public AboutSection About { get; set; } = new();
    public List<Stat> Stats { get; set; } = new();
    public List<Skill> Skills { get; set; } = new();
    public ResumeSection Resume { get; set; } = new();
    public List<ServiceItem> Services { get; set; } = new();
    public List<PortfolioItem> Portfolio { get; set; } = new();
    public List<Testimonial> Testimonials { get; set; } = new();
    public ContactSection Contact { get; set; } = new();
}

public sealed class ProfileSection
{
    [Required] public string FullName { get; set; } = "Sumit Kumar";
    public string Headline { get; set; } = "Software Engineer & Technology Leader";
    public List<string> Roles { get; set; } = new() { "Software Engineer", "Technology Leader", "Solution Architect" };
    public string AvatarUrl { get; set; } = "img/profile.jpg";
    public string HeroBackgroundUrl { get; set; } = "img/hero-bg.jpg";
}

public sealed class SocialLink
{
    public string Network { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}

public sealed class AboutSection
{
    public string Tagline { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<KeyValueItem> Facts { get; set; } = new();
}

public sealed class KeyValueItem
{
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public sealed class Stat
{
    public string Label { get; set; } = string.Empty;
    public int Value { get; set; }
    public string Icon { get; set; } = string.Empty;
}

public sealed class Skill
{
    public string Name { get; set; } = string.Empty;
    public int Percent { get; set; }
}

public sealed class ResumeSection
{
    public string Summary { get; set; } = string.Empty;
    public List<TimelineEntry> Education { get; set; } = new();
    public List<TimelineEntry> Experience { get; set; } = new();
}

public sealed class TimelineEntry
{
    public string Title { get; set; } = string.Empty;
    public string Period { get; set; } = string.Empty;
    public string Organization { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public List<string> Highlights { get; set; } = new();
}

public sealed class ServiceItem
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
}

public sealed class PortfolioItem
{
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}

public sealed class Testimonial
{
    public string Author { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Quote { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
}

public sealed class ContactSection
{
    [EmailAddress] public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}
