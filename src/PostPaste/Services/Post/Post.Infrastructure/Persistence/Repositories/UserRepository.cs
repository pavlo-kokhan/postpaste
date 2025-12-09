using Microsoft.EntityFrameworkCore;
using Post.Domain.Entities.User;

namespace Post.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public UserRepository(ApplicationDbContext dbContext) 
        => _dbContext = dbContext;
    
    public async Task<UserEntity?> GetByIdAsync(int id, CancellationToken cancellationToken) 
        => await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        => await _dbContext.Users
            .FirstOrDefaultAsync(x => 
                x.NormalizedEmail != null && 
                x.NormalizedEmail.Equals(
                    email, 
                    StringComparison.CurrentCultureIgnoreCase), 
                cancellationToken);
}