using Azure.Core;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Post.Api.Application.Constants.Errors;
using Post.Api.Application.Services.Abstract;
using Shared.Result.Results;
using Shared.Result.Results.Generic;

namespace Post.Api.Application.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _blobContainerClient;
    
    public BlobStorageService(BlobContainerClient blobContainerClient)
    {
        _blobContainerClient = blobContainerClient;
    }
    
    public async Task<Result> UploadAsync(string key, Stream content, ContentType contentType, CancellationToken cancellationToken = default)
    {
        var blob = _blobContainerClient.GetBlobClient(key);
        
        if (!await blob.ExistsAsync(cancellationToken)) 
            return Result<Stream>.ValidationFailure(BlobStorageErrors.BlobNotFound);

        var response = await blob.UploadAsync(
            content,
            new BlobHttpHeaders
            {
                ContentType = contentType.ToString()
            },
            cancellationToken: cancellationToken);
        
        return response.GetRawResponse().IsError 
            ? Result.ValidationFailure(BlobStorageErrors.FailedToUploadBlob) 
            : Result.Success();
    }

    public async Task<Result<Stream>> DownloadAsync(string key, CancellationToken cancellationToken = default)
    {
        var blob = _blobContainerClient.GetBlobClient(key);
        
        if (!await blob.ExistsAsync(cancellationToken)) 
            return Result<Stream>.ValidationFailure(BlobStorageErrors.BlobNotFound);

        var response = await blob.DownloadAsync(cancellationToken);
        
        return response.GetRawResponse().IsError
            ? Result<Stream>.ValidationFailure(BlobStorageErrors.FailedToDownloadBlob)
            : response.Value.Content;
    }

    public async Task<Result> DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        var blob = _blobContainerClient.GetBlobClient(key);
        
        if (!await blob.ExistsAsync(cancellationToken)) 
            return Result<Stream>.ValidationFailure(BlobStorageErrors.BlobNotFound);
        
        var response = await blob.DeleteIfExistsAsync(cancellationToken: cancellationToken);

        return response.GetRawResponse().IsError
            ? Result.ValidationFailure(BlobStorageErrors.FailedToDeleteBlob)
            : Result.Success();
    }
}