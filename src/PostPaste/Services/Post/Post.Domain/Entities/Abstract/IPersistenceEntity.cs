namespace Post.Domain.Entities.Abstract;

public interface IPersistenceEntity
{
    bool IsDeleted { get; }
}