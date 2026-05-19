using SumitKumar.Domain.Entities;

namespace SumitKumar.Application.Abstractions;

public interface IBlogRepository
{
    IQueryable<BlogPost> Query();
    Task<BlogPost?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<BlogPost?> GetBySlugAsync(string slug, CancellationToken ct);
    Task AddAsync(BlogPost post, CancellationToken ct);
    void Update(BlogPost post);
    void Remove(BlogPost post);
    Task<int> SaveChangesAsync(CancellationToken ct);

    Task<BlogCategory> EnsureCategoryAsync(string name, CancellationToken ct);
    Task<List<BlogTag>> EnsureTagsAsync(IEnumerable<string> names, CancellationToken ct);
    Task<IReadOnlyList<BlogCategory>> GetCategoriesAsync(CancellationToken ct);
}

public interface IContactRepository
{
    Task AddAsync(ContactMessage message, CancellationToken ct);
    Task<int> SaveChangesAsync(CancellationToken ct);
}
