using System.Net.Http.Json;
using SumitKumar.Application.DTOs;

namespace SumitKumar.Shared.Services;

/// <summary>Typed HTTP client used by Blazor components (server, WASM, MAUI) to talk to portfolio APIs.</summary>
public sealed class PortfolioApiClient(HttpClient http)
{
    public async Task<IReadOnlyList<BlogPostDto>> GetPublishedPostsAsync(CancellationToken ct = default)
        => await http.GetFromJsonAsync<List<BlogPostDto>>("api/blog", ct) ?? new();

    public async Task<BlogPostDto?> GetPostAsync(string slug, CancellationToken ct = default)
        => await http.GetFromJsonAsync<BlogPostDto>($"api/blog/{slug}", ct);

    public async Task SubmitContactAsync(ContactMessageDto message, CancellationToken ct = default)
    {
        var resp = await http.PostAsJsonAsync("api/contact", message, ct);
        resp.EnsureSuccessStatusCode();
    }
}
