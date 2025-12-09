using Microsoft.Extensions.DependencyInjection;
using Post.Domain;
using Post.Domain.Entities.Post;
using Post.Domain.Entities.PostFolder;
using Post.Domain.Entities.User;

namespace Post.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IServiceProvider _serviceProvider;

    private IUserRepository? _userRepository;
    private IPostRepository? _postRepository;
    private IPostFolderRepository? _postFolderRepository;

    public UnitOfWork(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        _dbContext = dbContext;
        _serviceProvider = serviceProvider;
    }

    public IUserRepository UserRepository
        => _userRepository ??= _serviceProvider.GetRequiredService<IUserRepository>();

    public IPostRepository PostRepository
        => _postRepository ??= _serviceProvider.GetRequiredService<IPostRepository>();
    
    public IPostFolderRepository PostFolderRepository
        => _postFolderRepository ??= _serviceProvider.GetRequiredService<IPostFolderRepository>();
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) 
        => _dbContext.SaveChangesAsync(cancellationToken);
}