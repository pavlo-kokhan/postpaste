using MediatR;
using Microsoft.EntityFrameworkCore;
using Post.Api.Application.Constants.Errors;
using Post.Api.Application.Extensions;
using Post.Api.Application.Queries.PostFolder.Extensions;
using Post.Api.Application.Responses.Common;
using Post.Api.Application.Responses.PostFolder;
using Post.Domain.Entities.User;
using Post.Infrastructure.Persistence;
using Shared.Result.Results.Generic;

namespace Post.Api.Application.Queries.PostFolder.ByUser;

public record UserPostFoldersQuery(int UserId, int Page, int PageSize, string? Search)
    : IRequest<Result<PageResponseDto<ShortPostFolderResponseDto>>>
{
    public class Handler : IRequestHandler<UserPostFoldersQuery, Result<PageResponseDto<ShortPostFolderResponseDto>>>
    {
        private readonly ApplicationDbContext _dbContext;
        
        public Handler(ApplicationDbContext dbContext, IUserRepository userRepository) 
            => _dbContext = dbContext;

        public async Task<Result<PageResponseDto<ShortPostFolderResponseDto>>> Handle(UserPostFoldersQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.PostFolders
                .AsNoTracking()
                .ApplyUserFilter(request.UserId)
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