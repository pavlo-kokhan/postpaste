namespace Post.Domain.Entities.Abstract;

public interface ICrudRepository<TEntity>
{
    public Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    public Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
}