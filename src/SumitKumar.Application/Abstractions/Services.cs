using SumitKumar.Application.DTOs;

namespace SumitKumar.Application.Abstractions;

public interface IBlogService
{
    Task<IReadOnlyList<BlogPostDto>> GetPublishedAsync(int skip = 0, int take = 20, CancellationToken ct = default);
    Task<IReadOnlyList<BlogPostDto>> GetAllAsync(CancellationToken ct = default);
    Task<BlogPostDto?> GetBySlugAsync(string slug, CancellationToken ct = default);
    Task<BlogPostDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BlogPostDto> UpsertAsync(BlogPostUpsertDto input, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<BlogCategoryDto>> GetCategoriesAsync(CancellationToken ct = default);
}

public interface IContactService
{
    Task SubmitAsync(ContactMessageDto message, CancellationToken ct = default);
}

public interface IEmailSender
{
    Task SendAsync(string to, string subject, string htmlBody, string? replyTo = null, CancellationToken ct = default);
}
