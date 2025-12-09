namespace Post.Domain.Entities.User;

public interface IUserRepository
{
    Task<UserEntity?> GetByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}