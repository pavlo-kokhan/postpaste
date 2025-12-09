using Post.Api.Application.Responses.Common;
using Post.Api.Application.Responses.Post;
using Post.Api.Application.Responses.User;

namespace Post.Api.Application.Responses.PostFolder;

public record PostFolderResponseDto(
    int Id,
    string Name,
    ShortUserResponseDto User) : TimeRelatedResponseDto;