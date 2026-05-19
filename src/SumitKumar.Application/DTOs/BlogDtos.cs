namespace SumitKumar.Application.DTOs;

public record BlogPostDto(
    Guid Id,
    string Title,
    string Slug,
    string Summary,
    string ContentMarkdown,
    string? CoverImageUrl,
    string AuthorName,
    string? CategoryName,
    IReadOnlyList<string> Tags,
    bool IsPublished,
    DateTimeOffset? PublishedUtc,
    DateTimeOffset CreatedUtc
);

public record BlogPostUpsertDto(
    Guid? Id,
    string Title,
    string Slug,
    string Summary,
    string ContentMarkdown,
    string? CoverImageUrl,
    string AuthorName,
    string? CategoryName,
    IReadOnlyList<string> Tags,
    bool IsPublished
);

public record BlogCategoryDto(Guid Id, string Name, string Slug, int PostCount);

public record ContactMessageDto(
    string FromName,
    string FromEmail,
    string Subject,
    string Body
);
