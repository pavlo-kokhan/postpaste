using LinqKit;
using Microsoft.EntityFrameworkCore;
using Post.Api.Application.Queries.PostFolder.Projections;
using Post.Domain.Entities.PostFolder;
using Post.Domain.Entities.User;

namespace Post.Api.Application.Queries.PostFolder.Extensions;

public static class PostFolderQueryableExtensions
{
    public static IQueryable<PostFolderEntity> ApplyUserFilter(this IQueryable<PostFolderEntity> query, int userId)
        => query.Where(p => p.OwnerId == userId);

    public static IQueryable<PostFolderWithReferences> IncludeReferences(
        this IQueryable<PostFolderEntity> source,
        IQueryable<UserEntity>? users)
    {
        var query = source.Select(p => new PostFolderWithReferences { PostFolder = p });

        if (users is not null)
        {
            query = query.LeftJoin(users, p => p.PostFolder.OwnerId, u => u.Id,
                (p, u) => new PostFolderWithReferences
                {
                    PostFolder = p.PostFolder,
                    User = u
                });
        }

        return query;
    }
    
    public static IQueryable<PostFolderEntity> ApplySearch(this IQueryable<PostFolderEntity> query, string? search)
    {
        if (string.IsNullOrEmpty(search))
            return query;
        
        var pattern = $"%{search}%";
        
        return query
            .Where(p => EF.Functions.ILike(p.Name, pattern));
    }
}