namespace Post.Api.Application.Options;

public class EmailOptions
{
    public required string SenderEmail { get; set; }
    
    public required string Sender { get; set; }
    
    public required string Host { get; set; }
    
    public required int Port { get; set; }
}