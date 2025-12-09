using System.Text;
using Azure.Core;
using MediatR;
using Post.Api.Application.BusinessRules;
using Post.Api.Application.BusinessRules.Abstract;
using Post.Api.Application.Constants.Errors;
using Post.Api.Application.Services.Abstract;
using Post.Domain;
using Post.Domain.Entities.Post;
using Shared.Result.Results;

namespace Post.Api.Application.Commands.Post.Create;

public record CreatePostCommand(
    string Name,
    PostCategoryValueObject Category,
    IReadOnlyCollection<string> Tags,
    string Content,
    string? Password,
    DateTime? ExpirationDate,
    int? FolderId) : IRequest<Result>
{
    public class Handler : IRequestHandler<CreatePostCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserAccessor _userAccessor;
        private readonly IPostsStorageService _storageService;
        private readonly IPasswordProtector _passwordProtector;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IBusinessRuleValidator<ConfirmedUserRule> _confirmedUserRuleValidator;
        private readonly IBusinessRuleValidator<PostFolderExistsRule> _postFolderExistsRuleValidator;

        public Handler(
            IUnitOfWork unitOfWork, 
            IUserAccessor userAccessor,
            IPostsStorageService storageService,
            IPasswordProtector passwordProtector,
            IHostEnvironment hostEnvironment,
            IBusinessRuleValidator<ConfirmedUserRule> confirmedUserRuleValidator, 
            IBusinessRuleValidator<PostFolderExistsRule> postFolderExistsRuleValidator)
        {
            _unitOfWork = unitOfWork;
            _userAccessor = userAccessor;
            _storageService = storageService;
            _passwordProtector = passwordProtector;
            _hostEnvironment = hostEnvironment;
            _confirmedUserRuleValidator = confirmedUserRuleValidator;
            _postFolderExistsRuleValidator = postFolderExistsRuleValidator;
        }

        public async Task<Result> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var userId = _userAccessor.UserId;

            if (!userId.HasValue)
                return Result.ValidationFailure(IdentityErrors.UserNotFound);
            
            if (await _confirmedUserRuleValidator.IsBrokenAsync(new ConfirmedUserRule(userId.Value), cancellationToken))
                return Result.ValidationFailure(_confirmedUserRuleValidator.Error);
            
            if (request.FolderId.HasValue && 
                await _postFolderExistsRuleValidator.IsBrokenAsync(new PostFolderExistsRule(request.FolderId.Value), cancellationToken))
                return Result.ValidationFailure(_postFolderExistsRuleValidator.Error);

            var postContentBytes = Encoding.UTF8.GetBytes(request.Content);
            await using var postContentStream = new MemoryStream(postContentBytes);

            var key = $"{_hostEnvironment.EnvironmentName}/user_id-{userId.Value}/{request.Name}::{Guid.NewGuid()}";
            
            var uploadResult = await _storageService.UploadAsync(
                key,
                postContentStream,
                ContentType.TextPlain,
                cancellationToken);

            if (uploadResult.IsFailure)
                return Result.ValidationFailure(uploadResult.Errors);
            
            var passwordHashResult = request.Password is null 
                ? null 
                : _passwordProtector.Create(request.Password);

            if (passwordHashResult is not null && passwordHashResult.IsFailure)
                return Result.ValidationFailure(passwordHashResult.Errors);

            var createPostResult = PostEntity.Create(
                request.Name,
                request.Category,
                request.Tags,
                passwordHashResult?.Data.Hash,
                passwordHashResult?.Data.Salt,
                request.ExpirationDate,
                "Temp",
                key,
                userId.Value,
                request.FolderId);

            if (createPostResult.IsFailure)
                return createPostResult;

            await _unitOfWork.PostRepository.CreateAsync(createPostResult.Data, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}