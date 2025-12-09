using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Post.Domain.Entities.Post;
using Post.Domain.Entities.PostFolder;
using Post.Domain.Entities.User;

namespace Post.Infrastructure.EntityConfigurations;

public class PostFolderEntityConfiguration : IEntityTypeConfiguration<PostFolderEntity>
{
    public void Configure(EntityTypeBuilder<PostFolderEntity> builder)
    {
        builder
            .ToTable("PostFolders")
            .HasKey(p => p.Id);
        
        builder.HasIndex(p => p.IsDeleted);
        
        builder
            .Property(p => p.Name)
            .HasMaxLength(256);

        builder
            .HasOne<UserEntity>()
            .WithMany()
            .HasForeignKey(p => p.OwnerId);
    }
}