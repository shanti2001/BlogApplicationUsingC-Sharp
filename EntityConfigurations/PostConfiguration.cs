using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApplication.Models.EntityConfigurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("posts");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("Id").ValueGeneratedOnAdd();

            builder.Property(p => p.Title).IsRequired().HasMaxLength(255);
            builder.Property(p => p.Excerpt).HasMaxLength(500);
            builder.Property(p => p.Content).HasColumnType("nvarchar(4000)").IsRequired();

            builder.Property(p => p.PublishedAt).IsRequired();
            builder.Property(p => p.IsPublished).IsRequired();

            // Define relationship with author
            builder.HasOne(p => p.Author)
                   .WithMany(u => u.Posts)
                   .HasForeignKey(p => p.AuthorId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Define relationship with tags
            builder.HasMany(p => p.Tags)
                   .WithMany(t => t.Posts)
                   .UsingEntity(j => j.ToTable("PostTag"));
        }
    }
}
