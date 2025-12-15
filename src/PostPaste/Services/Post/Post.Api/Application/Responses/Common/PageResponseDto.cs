namespace Post.Api.Application.Responses.Common;

public record PageResponseDto<T>(IReadOnlyCollection<T> Items, int TotalCount)
{
    public static readonly PageResponseDto<T> Empty = new([], 0);
}