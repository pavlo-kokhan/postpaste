using FluentValidation;
using Post.Domain.Entities.Abstract;
using Post.Domain.ValidationExtensions;
using Shared.Result.Results.Generic;

namespace Post.Domain.Entities.PostFolder;

public class PostFolderEntity : PersistenceEntity
{
    private static readonly IValidator<PostFolderEntity> Validator = new PostFolderEntityValidator();
    
    private PostFolderEntity(string name, int ownerId)
    {
        Name = name;
        OwnerId = ownerId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private PostFolderEntity()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    { }
    
    public string Name { get; private set; }
    
    public int OwnerId { get; private set; }
    
    public static Result<PostFolderEntity> Create(string name, int ownerId) 
        => Validator.ToResult(new PostFolderEntity(name, ownerId));
    
    public Result<PostFolderEntity> Update(string name)
    {
        Name = name;
        UpdatedAt = DateTime.UtcNow;

        return Validator.ToResult(this);
    }
}