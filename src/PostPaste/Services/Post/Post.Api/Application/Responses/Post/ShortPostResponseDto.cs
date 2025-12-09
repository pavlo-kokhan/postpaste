using Post.Api.Application.Responses.Common;
using Post.Domain.Entities.Post;

namespace Post.Api.Application.Responses.Post;

public record ShortPostResponseDto(
    int Id,
    string Name,
    PostCategoryValueObject Category,
    IReadOnlyCollection<string> Tags) : TimeRelatedResponseDto;