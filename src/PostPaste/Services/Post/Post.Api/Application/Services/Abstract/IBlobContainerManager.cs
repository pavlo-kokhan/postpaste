namespace Post.Api.Application.Services.Abstract;

public interface IBlobContainerManager
{
    public Task CheckConnectionAsync();
    
    public Task EnsureContainersExistsAsync();
}