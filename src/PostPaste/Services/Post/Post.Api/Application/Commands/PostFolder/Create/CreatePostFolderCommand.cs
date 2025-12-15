using MediatR;
using Post.Api.Application.BusinessRules;
using Post.Api.Application.BusinessRules.Abstract;
using Post.Api.Application.Constants.Errors;
using Post.Api.Application.Services.Abstract;
using Post.Domain;
using Post.Domain.Entities.PostFolder;
using Shared.Result.Results;

namespace Post.Api.Application.Commands.PostFolder.Create;

public record CreatePostFolderCommand(string Name) : IRequest<Result>
{
    public class Hadler : IRequestHandler<CreatePostFolderCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserAccessor _userAccessor;
        private readonly IBusinessRuleValidator<UniqueFolderNameRule> _uniqueFolderNameValidator;

        public Hadler(
            IUnitOfWork unitOfWork, 
            IUserAccessor userAccessor,
            IBusinessRuleValidator<UniqueFolderNameRule> uniqueFolderNameValidator)
        {
            _unitOfWork = unitOfWork;
            _userAccessor = userAccessor;
            _uniqueFolderNameValidator = uniqueFolderNameValidator;
        }

        public async Task<Result> Handle(CreatePostFolderCommand request, CancellationToken cancellationToken)
        {
            var userId = _userAccessor.UserId;

            if (!userId.HasValue)
                return Result.ValidationFailure(IdentityErrors.UserNotFound);
            
            if (await _uniqueFolderNameValidator.IsBrokenAsync(new UniqueFolderNameRule(request.Name, userId.Value), cancellationToken))
                return Result.ValidationFailure(_uniqueFolderNameValidator.Error);
            
            var createPostFolderResult = PostFolderEntity.Create(request.Name, userId.Value);

            if (createPostFolderResult.IsFailure)
                return createPostFolderResult;

            await _unitOfWork.PostFolderRepository.CreateAsync(createPostFolderResult.Data, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}