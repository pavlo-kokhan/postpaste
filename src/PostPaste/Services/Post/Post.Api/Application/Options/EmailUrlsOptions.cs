namespace Post.Api.Application.Options;

public class EmailUrlsOptions
{
    public required string EmailConfirmationBaseUrl { get; set; }
    
    public required string LoginBaseUrl { get; set; }
}