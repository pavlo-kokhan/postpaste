using MediatR;
using Microsoft.EntityFrameworkCore;
using Post.Api.Application.Extensions;
using Post.Api.Application.Queries.Post.Extensions;
using Post.Api.Application.Responses.Common;
using Post.Api.Application.Responses.Post;
using Post.Infrastructure.Persistence;
using Shared.Result.Results.Generic;

namespace Post.Api.Application.Queries.Post.ByUser;

public record UserPostsQuery(
    int UserId, 
    int? FolderId, 
    int Page, 
    int PageSize, 
    string? Search, 
    string? OrderBy, 
    bool IsAscending)
    : IRequest<Result<PageResponseDto<ShortPostResponseDto>>>
{
    public class Handler : IRequestHandler<UserPostsQuery, Result<PageResponseDto<ShortPostResponseDto>>>
    {
        private readonly ApplicationDbContext _dbContext;
        
        public Handler(ApplicationDbContext dbContext) 
            => _dbContext = dbContext;

        public async Task<Result<PageResponseDto<ShortPostResponseDto>>> Handle(UserPostsQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Posts
                .AsNoTracking()
                .ApplyUserIdFilter(request.UserId)
                .ApplyFolderFilter(request.FolderId)
                .IncludeReferences(null, _dbContext.PostFolders)
                .ApplySearch(request.Search)
                .ApplySort(request.OrderBy, request.IsAscending);

            var totalCount = await query.CountAsync(cancellationToken);

            if (totalCount == 0)
                return PageResponseDto<ShortPostResponseDto>.Empty;

            var items = await query
                .ToPageQuery(request.Page, request.PageSize)
                .Select(p => new ShortPostResponseDto(
                    p.Post.Id, 
                    p.Post.Name, 
                    p.Post.Category, 
                    p.Post.Tags)
                {
                    CreatedAt = p.Post.CreatedAt,
                    UpdatedAt = p.Post.UpdatedAt
                })
                .ToListAsync(cancellationToken);

            return new PageResponseDto<ShortPostResponseDto>(items, totalCount);
        }
    }
}