using Post.Api.Application.Services.Abstract;

namespace Post.Api.Application.Services;

public class BlobKeyService : IBlobKeyService
{
    private readonly IHostEnvironment _hostEnvironment;

    public BlobKeyService(IHostEnvironment hostEnvironment) 
        => _hostEnvironment = hostEnvironment;

    public string GeneratePostKey(int userId) 
        => $"{_hostEnvironment.EnvironmentName}/user[{userId}]/{Guid.NewGuid():N}";
}