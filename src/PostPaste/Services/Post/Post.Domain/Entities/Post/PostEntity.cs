using FluentValidation;
using Post.Domain.Entities.Abstract;
using Shared.Result.Results.Generic;

namespace Post.Domain.Entities.Post;

// todo: implement and configure entity
public class PostEntity : PersistenceEntity
{
    private static readonly IValidator<PostEntity> Validator = new PostEntityValidator(
        new PostCategoryValueObjectValidator());

    private PostEntity(
        string name,
        PostCategoryValueObject category,
        IReadOnlyCollection<string> tags,
        string? passwordHash,
        string? passwordSalt,
        DateTime? expirationDate,
        string shortCode,
        string blobKey,
        long ownerId,
        long? folderId)
    {
        Name = name;
        Category = category;
        Tags = tags;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        ExpirationDate = expirationDate;
        ShortCode = shortCode;
        BlobKey = blobKey;
        OwnerId = ownerId;
        FolderId = folderId;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public PostEntity()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    { }
    
    public string Name { get; private set; }
    
    public PostCategoryValueObject Category { get; private set; }
    
    public IReadOnlyCollection<string> Tags { get; private set; }
    
    public string? PasswordHash { get; private set; }
    
    public string? PasswordSalt { get; private set; }
    
    public bool IsProtected => PasswordHash is not null;
    
    public DateTime? ExpirationDate { get; private set; }
    
    public string ShortCode { get; private set; }
    
    public string BlobKey { get; private set; }
    
    public long OwnerId { get; private set; }
    
    public long? FolderId { get; private set; }

    public static Result<PostEntity> Create(
        string name,
        PostCategoryValueObject category,
        IReadOnlyCollection<string> tags,
        string? passwordHash,
        string? passwordSalt,
        DateTime? expirationDate,
        string shortCode,
        string blobKey,
        long ownerId,
        long? folderId) 
        => Validator.ToResult(
                new PostEntity(
                    name,
                    category,
                    tags,
                    passwordHash,
                    passwordSalt,
                    expirationDate,
                    shortCode,
                    blobKey,
                    ownerId,
                    folderId));

    public Result<PostEntity> Update(
        string name,
        PostCategoryValueObject category,
        IReadOnlyCollection<string> tags,
        string? passwordHash,
        string? passwordSalt,
        DateTime? expirationDate,
        string shortCode,
        string blobKey,
        long ownerId,
        long? folderId)
    {
        Name = name;
        Category = category;
        Tags = tags;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        ExpirationDate = expirationDate;
        ShortCode = shortCode;
        BlobKey = blobKey;
        OwnerId = ownerId;
        FolderId = folderId;

        return Validator.ToResult(this);
    }
}