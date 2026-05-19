using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SumitKumar.Domain.Entities;

namespace SumitKumar.Infrastructure.Persistence;

public class BlogDbContext(DbContextOptions<BlogDbContext> options) : DbContext(options)
{
    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
    public DbSet<BlogCategory> BlogCategories => Set<BlogCategory>();
    public DbSet<BlogTag> BlogTags => Set<BlogTag>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // SQLite can't ORDER BY DateTimeOffset directly — store as long ticks.
        configurationBuilder.Properties<DateTimeOffset>()
            .HaveConversion<DateTimeOffsetToBinaryConverter>();
    }

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        b.Entity<BlogPost>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired().HasMaxLength(300);
            e.Property(x => x.Slug).IsRequired().HasMaxLength(300);
            e.HasIndex(x => x.Slug).IsUnique();
            e.Property(x => x.Summary).HasMaxLength(1000);
            e.Property(x => x.AuthorName).HasMaxLength(200);
            e.HasOne(x => x.Category).WithMany(c => c.Posts).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.SetNull);
            e.HasMany(x => x.Tags).WithMany(t => t.Posts);
        });

        b.Entity<BlogCategory>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(120);
            e.Property(x => x.Slug).IsRequired().HasMaxLength(120);
            e.HasIndex(x => x.Slug).IsUnique();
        });

        b.Entity<BlogTag>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(60);
            e.HasIndex(x => x.Name).IsUnique();
        });

        b.Entity<ContactMessage>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.FromName).IsRequired().HasMaxLength(200);
            e.Property(x => x.FromEmail).IsRequired().HasMaxLength(320);
            e.Property(x => x.Subject).IsRequired().HasMaxLength(300);
            e.Property(x => x.Body).IsRequired();
        });
    }
}
