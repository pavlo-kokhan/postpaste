using Post.Domain.Entities.Post;
using Post.Domain.Entities.PostFolder;
using Post.Domain.Entities.User;

namespace Post.Api.Application.Queries.Post.Projections;

public class PostWithReferences
{
    public required PostEntity Post { get; init; }

    public UserEntity? User { get; init; }
    
    public PostFolderEntity? Folder { get; init; }
}