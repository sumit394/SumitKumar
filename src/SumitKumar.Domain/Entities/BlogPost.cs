using SumitKumar.Domain.Common;

namespace SumitKumar.Domain.Entities;

public class BlogPost : Entity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string ContentMarkdown { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public DateTimeOffset? PublishedUtc { get; set; }

    public Guid? CategoryId { get; set; }
    public BlogCategory? Category { get; set; }

    public List<BlogTag> Tags { get; set; } = new();
}
