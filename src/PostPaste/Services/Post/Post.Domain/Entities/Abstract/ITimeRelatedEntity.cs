namespace Post.Domain.Entities.Abstract;

public interface ITimeRelatedEntity
{
    DateTime CreatedAt { get; }
    
    DateTime UpdatedAt { get; }
    
    DateTime DeletedAt { get; }
}