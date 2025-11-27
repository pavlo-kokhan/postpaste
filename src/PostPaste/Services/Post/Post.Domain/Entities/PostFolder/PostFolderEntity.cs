using FluentValidation;
using Post.Domain.Entities.Abstract;
using Shared.Result.Results.Generic;

namespace Post.Domain.Entities.PostFolder;

// todo: implement and configure entity
public class PostFolderEntity : PersistenceEntity
{
    private static readonly IValidator<PostFolderEntity> Validator = new PostFolderEntityValidator();
    
    private PostFolderEntity(string name, long ownerId)
    {
        Name = name;
        OwnerId = ownerId;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private PostFolderEntity()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    { }
    
    public string Name { get; private set; }
    
    public long OwnerId { get; private set; }
    
    public static Result<PostFolderEntity> Create(string name, long ownerId) 
        => Validator.ToResult(new PostFolderEntity(name, ownerId));
    
    public Result<PostFolderEntity> Update(string name, long ownerId)
    {
        Name = name;
        OwnerId = ownerId;

        return Validator.ToResult(this);
    }
}