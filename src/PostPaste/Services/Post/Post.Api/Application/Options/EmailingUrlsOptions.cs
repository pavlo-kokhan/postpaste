namespace Post.Api.Application.Options;

public class EmailingUrlsOptions
{
    public required string EmailConfirmationBaseUrl { get; set; }
    
    public required string LoginBaseUrl { get; set; }
}