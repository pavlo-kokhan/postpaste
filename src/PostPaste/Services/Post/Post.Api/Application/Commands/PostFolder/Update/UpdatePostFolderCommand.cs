using MediatR;
using Post.Api.Application.Constants.Errors;
using Post.Domain;
using Shared.Result.Results;

namespace Post.Api.Application.Commands.PostFolder.Update;

public record UpdatePostFolderCommand(int Id, string Name) : IRequest<Result>
{
    public class Handler : IRequestHandler<UpdatePostFolderCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork) 
            => _unitOfWork = unitOfWork;

        public async Task<Result> Handle(UpdatePostFolderCommand request, CancellationToken cancellationToken)
        {
            var postFolder = await _unitOfWork.PostFolderRepository.GetByIdAsync(request.Id, cancellationToken);

            if (postFolder is null)
                return Result.ValidationFailure(PostFolderErrors.NotFound);

            var updatePostFolderResult = postFolder.Update(request.Name);

            if (updatePostFolderResult.IsFailure)
                return updatePostFolderResult;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}