using Microsoft.EntityFrameworkCore;
using Post.Domain.Entities.Abstract;

namespace Post.Infrastructure.Persistence.Repositories;

public class CrudRepository<TEntity> : ICrudRepository<TEntity> where TEntity : PersistenceEntity
{
    protected CrudRepository(ApplicationDbContext context) 
        => Context = context;

    protected ApplicationDbContext Context { get; }
    
    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default) 
        => await Context
            .Set<TEntity>()
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

    public async Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Context.AddAsync(entity, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
    }
}