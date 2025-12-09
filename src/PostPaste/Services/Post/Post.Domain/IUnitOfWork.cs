using Post.Domain.Entities.Post;
using Post.Domain.Entities.PostFolder;
using Post.Domain.Entities.User;

namespace Post.Domain;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    
    IPostRepository PostRepository { get; }
    
    IPostFolderRepository PostFolderRepository { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}