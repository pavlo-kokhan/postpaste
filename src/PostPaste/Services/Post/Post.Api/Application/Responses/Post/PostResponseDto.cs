using Post.Api.Application.Responses.Common;
using Post.Api.Application.Responses.PostFolder;
using Post.Api.Application.Responses.User;
using Post.Domain.Entities.Post;

namespace Post.Api.Application.Responses.Post;

public record PostResponseDto(
    int Id,
    string Name,
    PostCategoryValueObject Category,
    IReadOnlyCollection<string> Tags,
    bool IsProtected,
    DateTime? ExpirationDate,
    string Content,
    ShortUserResponseDto User,
    ShortPostFolderResponseDto? Folder) : TimeRelatedResponseDto;