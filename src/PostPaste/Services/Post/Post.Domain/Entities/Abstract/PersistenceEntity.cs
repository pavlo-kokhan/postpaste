namespace Post.Domain.Entities.Abstract;

public class PersistenceEntity : Entity, IPersistenceEntity, ITimeRelatedEntity
{
    public bool IsDeleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime DeletedAt { get; private set; }

    public void SoftDelete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }
}