using SumitKumar.Domain.Common;

namespace SumitKumar.Domain.Entities;

public class BlogTag : Entity
{
    public string Name { get; set; } = string.Empty;
    public List<BlogPost> Posts { get; set; } = new();
}
