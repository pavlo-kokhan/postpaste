namespace Post.Api.Application.Responses.Common;

public record TimeRelatedResponseDto
{
    public DateTime? CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}