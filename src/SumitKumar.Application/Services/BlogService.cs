using SumitKumar.Application.Abstractions;
using SumitKumar.Application.DTOs;
using SumitKumar.Domain.Entities;

namespace SumitKumar.Application.Services;

public sealed class BlogService(IBlogRepository repo) : IBlogService
{
    public async Task<IReadOnlyList<BlogPostDto>> GetPublishedAsync(int skip = 0, int take = 20, CancellationToken ct = default)
    {
        var posts = QueryWithIncludes()
            .Where(p => p.IsPublished)
            .OrderByDescending(p => p.PublishedUtc ?? p.CreatedUtc)
            .Skip(skip)
            .Take(take)
            .ToList();
        return await Task.FromResult(posts.Select(Map).ToList());
    }

    public async Task<IReadOnlyList<BlogPostDto>> GetAllAsync(CancellationToken ct = default)
    {
        var posts = QueryWithIncludes()
            .OrderByDescending(p => p.CreatedUtc)
            .ToList();
        return await Task.FromResult(posts.Select(Map).ToList());
    }

    public async Task<BlogPostDto?> GetBySlugAsync(string slug, CancellationToken ct = default)
    {
        var post = await repo.GetBySlugAsync(slug, ct);
        return post is null ? null : Map(post);
    }

    public async Task<BlogPostDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var post = await repo.GetByIdAsync(id, ct);
        return post is null ? null : Map(post);
    }

    public async Task<BlogPostDto> UpsertAsync(BlogPostUpsertDto input, CancellationToken ct = default)
    {
        BlogPost? entity = null;
        if (input.Id.HasValue)
        {
            entity = await repo.GetByIdAsync(input.Id.Value, ct);
        }

        var category = string.IsNullOrWhiteSpace(input.CategoryName)
            ? null
            : await repo.EnsureCategoryAsync(input.CategoryName, ct);

        var tags = await repo.EnsureTagsAsync(input.Tags, ct);

        if (entity is null)
        {
            entity = new BlogPost
            {
                Title = input.Title,
                Slug = input.Slug,
                Summary = input.Summary,
                ContentMarkdown = input.ContentMarkdown,
                CoverImageUrl = input.CoverImageUrl,
                AuthorName = input.AuthorName,
                IsPublished = input.IsPublished,
                PublishedUtc = input.IsPublished ? DateTimeOffset.UtcNow : null,
                Category = category,
                Tags = tags
            };
            await repo.AddAsync(entity, ct);
        }
        else
        {
            entity.Title = input.Title;
            entity.Slug = input.Slug;
            entity.Summary = input.Summary;
            entity.ContentMarkdown = input.ContentMarkdown;
            entity.CoverImageUrl = input.CoverImageUrl;
            entity.AuthorName = input.AuthorName;
            entity.Category = category;
            entity.Tags = tags;
            if (input.IsPublished && !entity.IsPublished)
            {
                entity.PublishedUtc = DateTimeOffset.UtcNow;
            }
            entity.IsPublished = input.IsPublished;
            entity.UpdatedUtc = DateTimeOffset.UtcNow;
            repo.Update(entity);
        }

        await repo.SaveChangesAsync(ct);
        return Map(entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var post = await repo.GetByIdAsync(id, ct);
        if (post is null) return;
        repo.Remove(post);
        await repo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<BlogCategoryDto>> GetCategoriesAsync(CancellationToken ct = default)
    {
        var cats = await repo.GetCategoriesAsync(ct);
        return cats.Select(c => new BlogCategoryDto(c.Id, c.Name, c.Slug, c.Posts.Count)).ToList();
    }

    private IQueryable<BlogPost> QueryWithIncludes() => repo.Query();

    private static BlogPostDto Map(BlogPost p) => new(
        p.Id,
        p.Title,
        p.Slug,
        p.Summary,
        p.ContentMarkdown,
        p.CoverImageUrl,
        p.AuthorName,
        p.Category?.Name,
        p.Tags.Select(t => t.Name).ToList(),
        p.IsPublished,
        p.PublishedUtc,
        p.CreatedUtc
    );
}
