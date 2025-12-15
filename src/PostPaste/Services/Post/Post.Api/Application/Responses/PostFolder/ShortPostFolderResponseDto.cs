using Post.Api.Application.Responses.Common;

namespace Post.Api.Application.Responses.PostFolder;

public record ShortPostFolderResponseDto(
    int Id, 
    string Name) : TimeRelatedResponseDto;