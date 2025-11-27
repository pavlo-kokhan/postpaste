using Azure.Core;
using Shared.Result.Results;
using Shared.Result.Results.Generic;

namespace Post.Api.Application.Services.Abstract;

public interface IBlobStorageService
{
    Task<Result> UploadAsync(string key, Stream content, ContentType contentType, CancellationToken cancellationToken = default);
    
    Task<Result<Stream>> DownloadAsync(string key, CancellationToken cancellationToken = default);
    
    Task<Result> DeleteAsync(string key, CancellationToken cancellationToken = default);
}