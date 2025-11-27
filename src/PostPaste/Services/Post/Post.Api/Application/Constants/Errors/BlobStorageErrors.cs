using Shared.Result.Results;

namespace Post.Api.Application.Constants.Errors;

public static class BlobStorageErrors
{
    public static readonly Error FailedToUploadBlob = Error.Create("FAILED_TO_UPLOAD_BLOB");
    public static readonly Error FailedToDownloadBlob = Error.Create("FAILED_TO_DOWNLOAD_BLOB");
    public static readonly Error FailedToDeleteBlob = Error.Create("FAILED_TO_DELETE_BLOB");
    public static readonly Error BlobNotFound = Error.Create("BLOB_NOT_FOUND");
}