using Post.Api.Application.Responses;
using Shared.Result.Results;
using Shared.Result.Results.Generic;

namespace Post.Api.Application.Services.Abstract;

public interface IPostsStorageService
{
    IAsyncEnumerable<BlobItemInfo> ListAllAsync(CancellationToken cancellationToken = default);
    
    Task<Result> UploadAsync(string key, string content, CancellationToken cancellationToken = default);
    
    Task<Result<string>> DownloadAsync(string key, CancellationToken cancellationToken = default);
    
    Task<Result> DeleteAsync(string key, CancellationToken cancellationToken = default);
}