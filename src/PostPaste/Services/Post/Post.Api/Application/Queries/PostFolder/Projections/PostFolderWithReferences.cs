using Post.Domain.Entities.PostFolder;
using Post.Domain.Entities.User;

namespace Post.Api.Application.Queries.PostFolder.Projections;

public class PostFolderWithReferences
{
    public required PostFolderEntity PostFolder;

    public UserEntity? User;
}