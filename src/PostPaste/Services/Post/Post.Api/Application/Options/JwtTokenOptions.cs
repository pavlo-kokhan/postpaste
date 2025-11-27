namespace Post.Api.Application.Options;

public class JwtTokenOptions
{
    public required byte[] Key { get; set; }
    
    public required int ExpiresIn { get; set; }
}