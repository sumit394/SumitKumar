using Microsoft.EntityFrameworkCore;
using SumitKumar.Application.Abstractions;
using SumitKumar.Domain.Entities;

namespace SumitKumar.Infrastructure.Persistence;

public sealed class BlogRepository(BlogDbContext db) : IBlogRepository
{
    public IQueryable<BlogPost> Query() => db.BlogPosts
        .Include(p => p.Category)
        .Include(p => p.Tags)
        .AsQueryable();

    public Task<BlogPost?> GetByIdAsync(Guid id, CancellationToken ct) =>
        db.BlogPosts.Include(p => p.Category).Include(p => p.Tags).FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<BlogPost?> GetBySlugAsync(string slug, CancellationToken ct) =>
        db.BlogPosts.Include(p => p.Category).Include(p => p.Tags).FirstOrDefaultAsync(p => p.Slug == slug, ct);

    public async Task AddAsync(BlogPost post, CancellationToken ct) => await db.BlogPosts.AddAsync(post, ct);
    public void Update(BlogPost post) => db.BlogPosts.Update(post);
    public void Remove(BlogPost post) => db.BlogPosts.Remove(post);
    public Task<int> SaveChangesAsync(CancellationToken ct) => db.SaveChangesAsync(ct);

    public async Task<BlogCategory> EnsureCategoryAsync(string name, CancellationToken ct)
    {
        var slug = Slugify(name);
        var existing = await db.BlogCategories.FirstOrDefaultAsync(c => c.Slug == slug, ct);
        if (existing is not null) return existing;
        var cat = new BlogCategory { Name = name, Slug = slug };
        db.BlogCategories.Add(cat);
        return cat;
    }

    public async Task<List<BlogTag>> EnsureTagsAsync(IEnumerable<string> names, CancellationToken ct)
    {
        var list = new List<BlogTag>();
        foreach (var raw in names.Select(n => n.Trim()).Where(n => n.Length > 0).Distinct(StringComparer.OrdinalIgnoreCase))
        {
            var existing = await db.BlogTags.FirstOrDefaultAsync(t => t.Name == raw, ct);
            if (existing is null)
            {
                existing = new BlogTag { Name = raw };
                db.BlogTags.Add(existing);
            }
            list.Add(existing);
        }
        return list;
    }

    public async Task<IReadOnlyList<BlogCategory>> GetCategoriesAsync(CancellationToken ct) =>
        await db.BlogCategories.Include(c => c.Posts).OrderBy(c => c.Name).ToListAsync(ct);

    private static string Slugify(string value)
    {
        var s = value.Trim().ToLowerInvariant();
        var sb = new System.Text.StringBuilder(s.Length);
        foreach (var ch in s)
        {
            if (char.IsLetterOrDigit(ch)) sb.Append(ch);
            else if (ch is ' ' or '-' or '_' && sb.Length > 0 && sb[^1] != '-') sb.Append('-');
        }
        return sb.ToString().Trim('-');
    }
}

public sealed class ContactRepository(BlogDbContext db) : IContactRepository
{
    public async Task AddAsync(SumitKumar.Domain.Entities.ContactMessage message, CancellationToken ct) =>
        await db.ContactMessages.AddAsync(message, ct);

    public Task<int> SaveChangesAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
}
