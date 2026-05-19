using SumitKumar.Application.DTOs;

namespace SumitKumar.UnitTests;

public class BlogPostDtoTests
{
    [Fact]
    public void BlogPostDto_Constructs_With_Required_Fields()
    {
        var dto = new BlogPostDto(
            Id: Guid.NewGuid(),
            Title: "Hello",
            Slug: "hello",
            Summary: "Summary",
            ContentMarkdown: "Body",
            CoverImageUrl: null,
            AuthorName: "Sumit",
            CategoryName: null,
            Tags: new[] { "dotnet" },
            IsPublished: true,
            PublishedUtc: DateTimeOffset.UtcNow,
            CreatedUtc: DateTimeOffset.UtcNow);

        Assert.Equal("Hello", dto.Title);
        Assert.Equal("hello", dto.Slug);
        Assert.True(dto.IsPublished);
    }
}
