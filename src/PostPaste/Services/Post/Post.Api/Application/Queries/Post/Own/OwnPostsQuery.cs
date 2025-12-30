using MediatR;
using Microsoft.EntityFrameworkCore;
using Post.Api.Application.Constants.Errors;
using Post.Api.Application.Extensions;
using Post.Api.Application.Queries.Post.Extensions;
using Post.Api.Application.Responses.Common;
using Post.Api.Application.Responses.Post;
using Post.Api.Application.Services.Abstract;
using Post.Infrastructure.Persistence;
using Shared.Result.Results.Generic;

namespace Post.Api.Application.Queries.Post.Own;

public record OwnPostsQuery(
    int? FolderId, 
    int Page, 
    int PageSize, 
    string? Search, 
    string? OrderBy, 
    bool IsAscending)
    : IRequest<Result<PageResponseDto<ShortPostResponseDto>>>
{
    public class Handler : IRequestHandler<OwnPostsQuery, Result<PageResponseDto<ShortPostResponseDto>>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserAccessor _userAccessor;
        
        public Handler(ApplicationDbContext dbContext, IUserAccessor userAccessor)
        {
            _dbContext = dbContext;
            _userAccessor = userAccessor;
        }

        public async Task<Result<PageResponseDto<ShortPostResponseDto>>> Handle(OwnPostsQuery request, CancellationToken cancellationToken)
        {
            var userId = _userAccessor.UserId;
            
            if (!userId.HasValue)
                return Result<PageResponseDto<ShortPostResponseDto>>.ValidationFailure(IdentityErrors.UserNotFound);
            
            var query = _dbContext.Posts
                .AsNoTracking()
                .ApplyUserIdFilter(userId.Value)
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
                .ToArrayAsync(cancellationToken);
            
            return new PageResponseDto<ShortPostResponseDto>(items, totalCount);
        }
    }
}