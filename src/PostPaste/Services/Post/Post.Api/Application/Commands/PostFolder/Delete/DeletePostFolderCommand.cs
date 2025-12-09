using MediatR;
using Post.Api.Application.Constants.Errors;
using Post.Domain;
using Shared.Result.Results;

namespace Post.Api.Application.Commands.PostFolder.Delete;

public record DeletePostFolderCommand(int Id) : IRequest<Result>
{
    public class Handler : IRequestHandler<DeletePostFolderCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork) 
            => _unitOfWork = unitOfWork;

        public async Task<Result> Handle(DeletePostFolderCommand request, CancellationToken cancellationToken)
        {
            var postFolder = await _unitOfWork.PostFolderRepository.GetByIdAsync(request.Id, cancellationToken);

            if (postFolder is null)
                return Result.ValidationFailure(PostFolderErrors.NotFound);
            
            postFolder.SoftDelete();
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return Result.Success();
        }
    }
}