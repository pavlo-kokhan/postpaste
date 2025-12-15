using MediatR;
using Microsoft.EntityFrameworkCore;
using Post.Api.Application.Constants.Errors;
using Post.Api.Application.Queries.PostFolder.Extensions;
using Post.Api.Application.Responses.PostFolder;
using Post.Api.Application.Responses.User;
using Post.Infrastructure.Persistence;
using Shared.Result.Results.Generic;

namespace Post.Api.Application.Queries.PostFolder.ById;

public record PostFolderQuery(int Id) : IRequest<Result<PostFolderResponseDto>>
{
    public class Handler : IRequestHandler<PostFolderQuery, Result<PostFolderResponseDto>>
    {
        private readonly ApplicationDbContext _dbContext;
        
        public Handler(ApplicationDbContext dbContext) 
            => _dbContext = dbContext;

        public async Task<Result<PostFolderResponseDto>> Handle(PostFolderQuery request, CancellationToken cancellationToken)
        {
            var postFolder = await _dbContext.PostFolders
                .AsNoTracking()
                .Where(f => f.Id == request.Id)
                .IncludeReferences(_dbContext.Users)
                .Select(f => new PostFolderResponseDto(
                    f.PostFolder.Id,
                    f.PostFolder.Name,
                    f.User == null ? null : new ShortUserResponseDto(f.User.Id, f.User.Email))
                {
                    CreatedAt = f.PostFolder.CreatedAt,
                    UpdatedAt = f.PostFolder.UpdatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);


            if (postFolder is null)
                return Result<PostFolderResponseDto>.ValidationFailure(PostFolderErrors.NotFound);

            return postFolder;
        }
    }
}