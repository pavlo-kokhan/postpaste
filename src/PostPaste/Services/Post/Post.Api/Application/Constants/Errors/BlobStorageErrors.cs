using Shared.Result.Results;

namespace Post.Api.Application.Constants.Errors;

public static class BlobStorageErrors
{
    public static readonly Error FailedToUpload = Error.Create("FAILED_TO_UPLOAD_BLOB");
    public static readonly Error FailedToDownload = Error.Create("FAILED_TO_DOWNLOAD_BLOB");
    public static readonly Error FailedToDelete = Error.Create("FAILED_TO_DELETE_BLOB");
    public static readonly Error NotFound = Error.Create("BLOB_NOT_FOUND");
}