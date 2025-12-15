using MediatR;
using Post.Api.Application.BusinessRules;
using Post.Api.Application.BusinessRules.Abstract;
using Post.Api.Application.Constants.Errors;
using Post.Api.Application.Services.Abstract;
using Post.Domain;
using Post.Domain.Entities.Post;
using Shared.Result.Results;

namespace Post.Api.Application.Commands.Post.Update;

public record UpdatePostCommand(
    int Id,
    string Name,
    PostCategoryValueObject Category,
    IReadOnlyCollection<string> Tags,
    string Content,
    string? Password,
    DateTime? ExpirationDate,
    int? FolderId) : IRequest<Result>
{
    public class Handler : IRequestHandler<UpdatePostCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserAccessor _userAccessor;
        private readonly IPostsStorageService _storageService;
        private readonly IPasswordProtector _passwordProtector;
        private readonly IBlobKeyService _blobKeyService;

        private readonly IBusinessRuleValidator<PostFolderExistsRule> _postFolderExistsRuleValidator;
        private readonly IBusinessRuleValidator<PostOwnedByUserRule> _postOwnedByUserRuleValidator;

        public Handler(
            IUnitOfWork unitOfWork,
            IUserAccessor userAccessor,
            IPostsStorageService storageService,
            IPasswordProtector passwordProtector,
            IBlobKeyService blobKeyService,
            IBusinessRuleValidator<PostFolderExistsRule> postFolderExistsRuleValidator, 
            IBusinessRuleValidator<PostOwnedByUserRule> postOwnedByUserRuleValidator)
        {
            _unitOfWork = unitOfWork;
            _userAccessor = userAccessor;
            _storageService = storageService;
            _passwordProtector = passwordProtector;
            _blobKeyService = blobKeyService;
            _postFolderExistsRuleValidator = postFolderExistsRuleValidator;
            _postOwnedByUserRuleValidator = postOwnedByUserRuleValidator;
        }

        public async Task<Result> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var userId = _userAccessor.UserId;

            if (!userId.HasValue)
                return Result.ValidationFailure(IdentityErrors.UserNotFound);
            
            var post = await _unitOfWork.PostRepository.GetByIdAsync(request.Id, cancellationToken);
            
            if (post is null)
                return Result.ValidationFailure(PostErrors.NotFound);

            if (request.FolderId.HasValue &&
                await _postFolderExistsRuleValidator.IsBrokenAsync(new PostFolderExistsRule(request.FolderId.Value), cancellationToken))
                return Result.ValidationFailure(_postFolderExistsRuleValidator.Error);

            if (await _postOwnedByUserRuleValidator.IsBrokenAsync(new PostOwnedByUserRule(request.Id, userId.Value), cancellationToken))
                return Result.ValidationFailure(_postOwnedByUserRuleValidator.Error);

            var passwordHashResult = request.Password is null
                ? null
                : _passwordProtector.Create(request.Password);

            if (passwordHashResult is not null && passwordHashResult.IsFailure)
                return Result.ValidationFailure(passwordHashResult.Errors);
            
            var deleteResult = await _storageService.DeleteAsync(post.BlobKey, cancellationToken);

            if (deleteResult.IsFailure)
                return Result.ValidationFailure(deleteResult.Errors);

            var newBlobKey = _blobKeyService.GeneratePostKey(userId.Value);
            
            // todo: make it transactional
            // delete old blob
            var uploadResult = await _storageService.UploadAsync(
                newBlobKey,
                request.Content,
                cancellationToken);

            if (uploadResult.IsFailure)
                return Result.ValidationFailure(uploadResult.Errors);

            var updateResult = post.Update(
                request.Name,
                request.Category,
                request.Tags,
                passwordHashResult?.Data.Hash,
                passwordHashResult?.Data.Salt,
                request.ExpirationDate,
                newBlobKey,
                userId.Value,
                request.FolderId);

            if (updateResult.IsFailure)
                return updateResult;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}