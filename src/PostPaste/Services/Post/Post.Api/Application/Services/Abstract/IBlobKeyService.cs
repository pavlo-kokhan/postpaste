namespace Post.Api.Application.Services.Abstract;

public interface IBlobKeyService
{
    public string GeneratePostKey(int userId);
}