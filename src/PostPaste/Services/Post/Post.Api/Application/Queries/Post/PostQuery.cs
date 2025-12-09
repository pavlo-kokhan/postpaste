using LinqKit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Post.Api.Application.Constants.Errors;
using Post.Api.Application.Responses.Post;
using Post.Api.Application.Responses.PostFolder;
using Post.Api.Application.Responses.User;
using Post.Api.Application.Services.Abstract;
using Post.Infrastructure.Persistence;
using Shared.Result.Results.Generic;

namespace Post.Api.Application.Queries.Post;

public record PostQuery(int Id, string? Password) : IRequest<Result<PostResponseDto>>
{
    public class Handler : IRequestHandler<PostQuery, Result<PostResponseDto>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IPostsStorageService _storageService;
        private readonly IPasswordProtector _passwordProtector;

        public Handler(
            ApplicationDbContext dbContext, 
            IPostsStorageService storageService, 
            IPasswordProtector passwordProtector)
        {
            _dbContext = dbContext;
            _storageService = storageService;
            _passwordProtector = passwordProtector;
        }

        public async Task<Result<PostResponseDto>> Handle(PostQuery request, CancellationToken cancellationToken)
        {
            var aggregatedPost = await _dbContext.Posts
                .AsNoTracking()
                .Where(p => p.Id == request.Id)
                .LeftJoin(
                    _dbContext.Users,
                    p => p.OwnerId,
                    u => u.Id,
                    (p, u) => new { Post = p, User = u })
                .LeftJoin(
                    _dbContext.PostFolders,
                    p => p.Post.FolderId,
                    f => f.Id,
                    (p, f) => new { p.Post, p.User, Folder = f })
                .FirstOrDefaultAsync(cancellationToken);
            
            if (aggregatedPost is null)
                return Result<PostResponseDto>.ValidationFailure(PostErrors.NotFound);
            
            if (aggregatedPost.Post.IsProtected && string.IsNullOrEmpty(request.Password))
                return Result<PostResponseDto>.ValidationFailure(PostErrors.PasswordRequired);

            if (aggregatedPost.Post.IsProtected && !string.IsNullOrEmpty(request.Password))
            {
                var isValidPassword = _passwordProtector.Verify(
                    request.Password,
                    aggregatedPost.Post.PasswordHash,
                    aggregatedPost.Post.PasswordSalt);
                
                if (!isValidPassword)
                    return Result<PostResponseDto>.ValidationFailure(PostErrors.InvalidPassword);
            }
            
            var contentResult = await _storageService.DownloadAsync(aggregatedPost.Post.BlobKey, cancellationToken);
            
            if (contentResult.IsFailure)
                return Result<PostResponseDto>.ValidationFailure(contentResult.Errors);
            
            return new PostResponseDto(
                aggregatedPost.Post.Id,
                aggregatedPost.Post.Name,
                aggregatedPost.Post.Category,
                aggregatedPost.Post.Tags,
                aggregatedPost.Post.IsProtected,
                aggregatedPost.Post.ExpirationDate,
                contentResult.Data,
                new ShortUserResponseDto(aggregatedPost.User.Id, aggregatedPost.User.Email),
                aggregatedPost.Folder is null 
                    ? null 
                    : new ShortPostFolderResponseDto(aggregatedPost.Folder.Id, aggregatedPost.Folder.Name))
            {
                CreatedAt = aggregatedPost.Post.CreatedAt,
                UpdatedAt = aggregatedPost.Post.UpdatedAt
            };
        }
    }
}