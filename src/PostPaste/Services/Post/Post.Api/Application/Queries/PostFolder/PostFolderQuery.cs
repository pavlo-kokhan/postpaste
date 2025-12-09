using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Post.Api.Application.Constants.Errors;
using Post.Api.Application.Responses.Post;
using Post.Api.Application.Responses.PostFolder;
using Post.Api.Application.Responses.User;
using Post.Infrastructure.Persistence;
using Shared.Result.Results.Generic;

namespace Post.Api.Application.Queries.PostFolder;

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
                .LeftJoin(
                    _dbContext.Users,
                    f => f.OwnerId,
                    u => u.Id,
                    (f, u) => new { Folder = f, User = u })
                .Select(f => new PostFolderResponseDto(
                    f.Folder.Id,
                    f.Folder.Name,
                    new ShortUserResponseDto(f.User.Id, f.User.Email))
                {
                    CreatedAt = f.Folder.CreatedAt,
                    UpdatedAt = f.Folder.UpdatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);


            if (postFolder is null)
                return Result<PostFolderResponseDto>.ValidationFailure(PostFolderErrors.NotFound);

            return postFolder;
        }
    }
}