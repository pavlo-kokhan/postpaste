using LinqKit;
using Microsoft.EntityFrameworkCore;
using Post.Api.Application.Queries.Post.Projections;
using Post.Domain.Entities.Post;
using Post.Domain.Entities.PostFolder;
using Post.Domain.Entities.User;

namespace Post.Api.Application.Queries.Post.Extensions;

public static class PostQueryableExtensions
{
    public static IQueryable<PostEntity> ApplyUserIdFilter(this IQueryable<PostEntity> query, int userId) 
        => query.Where(p => p.OwnerId == userId);

    public static IQueryable<PostEntity> ApplyFolderFilter(this IQueryable<PostEntity> query, int? folderId)
    {
        if (!folderId.HasValue)
            return query;
        
        return query.Where(p => p.FolderId == folderId.Value);
    }

    public static IQueryable<PostWithReferences> IncludeReferences(
        this IQueryable<PostEntity> source,
        IQueryable<UserEntity>? users,
        IQueryable<PostFolderEntity>? folders)
    {
        var query = source.Select(p => new PostWithReferences { Post = p });

        if (users is not null)
        {
            query = query.LeftJoin(users, p => p.Post.OwnerId, u => u.Id,
                (p, u) => new PostWithReferences
                {
                    Post = p.Post,
                    User = u,
                    Folder = p.Folder
                });
        }

        if (folders is not null)
        {
            query = query.LeftJoin(folders, p => p.Post.FolderId, f => f.Id,
                (p, f) => new PostWithReferences
                {
                    Post = p.Post,
                    User = p.User,
                    Folder = f
                });
        }

        return query;
    }
    
    public static IQueryable<PostWithReferences> ApplySearch(
        this IQueryable<PostWithReferences> query,
        string? search)
    {
        if (string.IsNullOrEmpty(search))
            return query;
        
        var pattern = $"%{search}%";
        
        return query
            .Where(p => 
                EF.Functions.ILike(p.Post.Name, pattern) ||
                EF.Functions.ILike(p.Post.Category.Name, pattern) ||
                p.Post.Tags.Any(t => EF.Functions.ILike(t, pattern)) ||
                (p.Folder != null && EF.Functions.ILike(p.Folder.Name, pattern)));
    }
}