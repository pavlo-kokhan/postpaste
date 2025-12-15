using MediatR;
using Post.Api.Application.BusinessRules;
using Post.Api.Application.BusinessRules.Abstract;
using Post.Api.Application.Constants.Errors;
using Post.Api.Application.Services.Abstract;
using Post.Domain;
using Shared.Result.Results;

namespace Post.Api.Application.Commands.Post.Delete;

public record DeletePostCommand(int PostId) : IRequest<Result>
{
    public class Handler : IRequestHandler<DeletePostCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserAccessor _userAccessor;

        private readonly IBusinessRuleValidator<PostOwnedByUserRule> _postOwnedByUserRuleValidator;

        public Handler(
            IUnitOfWork unitOfWork,
            IUserAccessor userAccessor,
            IBusinessRuleValidator<PostOwnedByUserRule> postOwnedByUserRuleValidator)
        {
            _unitOfWork = unitOfWork;
            _userAccessor = userAccessor;
            _postOwnedByUserRuleValidator = postOwnedByUserRuleValidator;
        }

        public async Task<Result> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var userId = _userAccessor.UserId;

            if (!userId.HasValue)
                return Result.ValidationFailure(IdentityErrors.UserNotFound);
            
            var post = await _unitOfWork.PostRepository.GetByIdAsync(request.PostId, cancellationToken);
            
            if (post is null)
                return Result.ValidationFailure(PostErrors.NotFound);

            if (await _postOwnedByUserRuleValidator.IsBrokenAsync(new PostOwnedByUserRule(request.PostId, userId.Value), cancellationToken))
                return Result.ValidationFailure(_postOwnedByUserRuleValidator.Error);

            // todo: delete blob
            post.SoftDelete();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return Result.Success();
        }
    }
}
