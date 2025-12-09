namespace Post.Domain.Entities.Abstract;

public class PersistenceEntity : Entity, IPersistenceEntity, ITimeRelatedEntity
{
    public bool IsDeleted { get; private set; }
    
    public DateTime CreatedAt { get; protected set; }
    
    public DateTime UpdatedAt { get; protected set; }
    
    public DateTime DeletedAt { get; protected set; }

    public void SoftDelete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }
}