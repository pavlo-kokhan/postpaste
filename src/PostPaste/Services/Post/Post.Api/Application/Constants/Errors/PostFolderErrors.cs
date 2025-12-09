using Shared.Result.Results;

namespace Post.Api.Application.Constants.Errors;

public static class PostFolderErrors
{
    public static readonly Error AlreadyExists = Error.Create("POST_FOLDER_ALREADY_EXISTS");
    
    public static readonly Error NotFound = Error.Create("POST_FOLDER_NOT_FOUND");
}