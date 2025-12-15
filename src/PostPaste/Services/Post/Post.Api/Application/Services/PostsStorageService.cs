using System.Runtime.CompilerServices;
using System.Text;
using Azure.Core;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using Post.Api.Application.Constants.Errors;
using Post.Api.Application.Options;
using Post.Api.Application.Responses;
using Post.Api.Application.Services.Abstract;
using Shared.Result.Results;
using Shared.Result.Results.Generic;

namespace Post.Api.Application.Services;

public class PostsStorageService : IPostsStorageService
{
    private readonly BlobContainerClient _blobContainerClient;

    public PostsStorageService(
        BlobServiceClient blobServiceClient, 
        IOptions<AzureStorageOptions> storageOptions)
    {
        var storageOptions1 = storageOptions.Value;
        _blobContainerClient = blobServiceClient.GetBlobContainerClient(storageOptions1.PostsContainerName);
    }
    
    public async IAsyncEnumerable<BlobItemInfo> ListAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var blobItem in _blobContainerClient.GetBlobsAsync(cancellationToken: cancellationToken))
        {
            yield return new BlobItemInfo(blobItem.Name, blobItem.Properties.CreatedOn ?? DateTime.UtcNow);
        }
    }

    public async Task<Result> UploadAsync(string key, string content, CancellationToken cancellationToken = default)
    {
        var blob = _blobContainerClient.GetBlobClient(key);
        await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        
        var response = await blob.UploadAsync(
            stream,
            new BlobHttpHeaders
            {
                ContentType = ContentType.TextPlain.ToString()
            },
            cancellationToken: cancellationToken);
        
        return response.GetRawResponse().IsError 
            ? Result.ValidationFailure(BlobStorageErrors.FailedToUpload) 
            : Result.Success();
    }

    public async Task<Result<string>> DownloadAsync(string key, CancellationToken cancellationToken = default)
    {
        var blob = _blobContainerClient.GetBlobClient(key);

        if (!await blob.ExistsAsync(cancellationToken))
            return Result<string>.ValidationFailure(BlobStorageErrors.NotFound);

        var response = await blob.DownloadAsync(cancellationToken);

        if (response.GetRawResponse().IsError)
            return Result<string>.ValidationFailure(BlobStorageErrors.FailedToDownload);

        await using var contentStream = response.Value.Content;
        using var reader = new StreamReader(contentStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);

        return await reader.ReadToEndAsync(cancellationToken);
    }

    public async Task<Result> DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        var blob = _blobContainerClient.GetBlobClient(key);
        
        if (!await blob.ExistsAsync(cancellationToken)) 
            return Result<Stream>.ValidationFailure(BlobStorageErrors.NotFound);
        
        var response = await blob.DeleteIfExistsAsync(cancellationToken: cancellationToken);

        return response.GetRawResponse().IsError
            ? Result.ValidationFailure(BlobStorageErrors.FailedToDelete)
            : Result.Success();
    }
}