using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using Post.Api.Application.Options;
using Post.Api.Application.Services.Abstract;

namespace Post.Api.Application.Services;

public class BlobContainerManager : IBlobContainerManager
{
    private readonly ILogger<BlobContainerManager> _logger;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly AzureStorageOptions _storageOptions;

    public BlobContainerManager(
        ILogger<BlobContainerManager> logger,
        BlobServiceClient blobServiceClient, 
        IOptions<AzureStorageOptions> storageOptions)
    {
        _logger = logger;
        _blobServiceClient = blobServiceClient;
        _storageOptions = storageOptions.Value;
    }
    
    public async Task CheckConnectionAsync()
    {
        try
        {
            await _blobServiceClient.GetPropertiesAsync();
            _logger.LogInformation("Connection to Azure Blob Storage is successful.");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Failed to connect to Azure Blob Storage.");
            throw;
        }
    }

    public async Task EnsureContainersExistsAsync()
    {
        await EnsurePostsContainerExistsAsync();
    }
    
    private async Task EnsurePostsContainerExistsAsync()
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_storageOptions.PostsContainerName);
        
        if (await containerClient.ExistsAsync())
        {
            _logger.LogInformation("Posts container already exists.");
            
            return;
        }
        
        await containerClient.CreateAsync();
        _logger.LogInformation("Posts container created.");
    }
}