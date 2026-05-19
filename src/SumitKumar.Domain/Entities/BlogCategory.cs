using SumitKumar.Domain.Common;

namespace SumitKumar.Domain.Entities;

public class BlogCategory : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public List<BlogPost> Posts { get; set; } = new();
}
