using MediatR;
using Microsoft.EntityFrameworkCore;
using Post.Api.Application.Constants.Errors;
using Post.Api.Application.Extensions;
using Post.Api.Application.Queries.PostFolder.Extensions;
using Post.Api.Application.Responses.Common;
using Post.Api.Application.Responses.PostFolder;
using Post.Api.Application.Services.Abstract;
using Post.Infrastructure.Persistence;
using Shared.Result.Results.Generic;

namespace Post.Api.Application.Queries.PostFolder.Own;

public record OwnPostFoldersQuery(int Page, int PageSize, string? Search)
    : IRequest<Result<PageResponseDto<ShortPostFolderResponseDto>>>
{
    public class Handler : IRequestHandler<OwnPostFoldersQuery, Result<PageResponseDto<ShortPostFolderResponseDto>>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserAccessor _userAccessor;
        
        public Handler(ApplicationDbContext dbContext, IUserAccessor userAccessor)
        {
            _dbContext = dbContext;
            _userAccessor = userAccessor;
        }

        public async Task<Result<PageResponseDto<ShortPostFolderResponseDto>>> Handle(OwnPostFoldersQuery request, CancellationToken cancellationToken)
        {
            var userId = _userAccessor.UserId;

            if (!userId.HasValue)
                return Result<PageResponseDto<ShortPostFolderResponseDto>>.ValidationFailure(
                    IdentityErrors.UserNotFound);
            
            var query = _dbContext.PostFolders
                .AsNoTracking()
                .ApplyUserFilter(userId.Value)
                .ApplySearch(request.Search)
                .OrderByDescending(f => f.CreatedAt);

            var totalCount = await query.CountAsync(cancellationToken);

            if (totalCount == 0)
                return PageResponseDto<ShortPostFolderResponseDto>.Empty;

            var items = await query
                .ToPageQuery(request.Page, request.PageSize)
                .Select(f => new ShortPostFolderResponseDto(f.Id, f.Name)
                {
                    CreatedAt = f.CreatedAt,
                    UpdatedAt = f.UpdatedAt
                })
                .ToArrayAsync(cancellationToken);

            return new PageResponseDto<ShortPostFolderResponseDto>(items, totalCount);
        }
    }
}