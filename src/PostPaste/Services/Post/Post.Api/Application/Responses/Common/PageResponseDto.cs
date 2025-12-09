namespace Post.Api.Application.Responses.Common;

public record PageResponseDto<T>(T Data, int Page, int PageSize, int TotalCount);