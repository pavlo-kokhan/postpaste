using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Post.Domain.Entities.Post;
using Post.Domain.Entities.PostFolder;
using Post.Domain.Entities.User;

namespace Post.Infrastructure.EntityConfigurations;

public class PostEntityConfiguration : IEntityTypeConfiguration<PostEntity>
{
    public void Configure(EntityTypeBuilder<PostEntity> builder)
    {
        builder
            .ToTable("Posts")
            .HasKey(p => p.Id);

        builder.HasIndex(p => p.IsDeleted);
        
        builder
            .Property(p => p.Name)
            .HasMaxLength(256);

        builder
            .OwnsOne<PostCategoryValueObject>(
                p => p.Category,
                navigationBuilder =>
                {
                    navigationBuilder.ToTable("PostCategories");
                    navigationBuilder.Property<int>("Id");
                    navigationBuilder.HasKey("Id");
                    navigationBuilder.WithOwner().HasForeignKey("PostId");
                    
                    navigationBuilder
                        .Property(p => p.Name)
                        .HasMaxLength(256);
                });

        builder
            .Property(p => p.Tags)
            .HasColumnType("jsonb")
            .HasDefaultValue(Array.Empty<string>());

        builder
            .HasOne<PostFolderEntity>()
            .WithMany()
            .HasForeignKey(p => p.FolderId);
        
        builder
            .HasOne<UserEntity>()
            .WithMany()
            .HasForeignKey(p => p.OwnerId);
    }
}