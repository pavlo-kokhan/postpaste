using MediatR;
using Microsoft.AspNetCore.Mvc;
using Post.Api.Application.Commands.Post.Create;
using Post.Api.Application.Commands.Post.Delete;
using Post.Api.Application.Commands.Post.Update;
using Post.Api.Application.Queries.Post;
using Post.Api.Application.Queries.Post.ById;
using Post.Api.Application.Queries.Post.ByUser;
using Post.Api.Application.Queries.Post.Own;
using Post.Api.Extensions;
using Post.Api.Filters;
using Post.Domain.Constants;

namespace Post.Api.Controllers;

[ApiController]
[Route("posts")]
[AppAuthorize(Roles.User | Roles.Admin)]
public class PostController : ControllerBase
{
    private readonly IMediator _mediator;

    public PostController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, string? password, CancellationToken cancellationToken)
        => (await _mediator.Send(new PostQuery(id, password), cancellationToken)).ToActionResultWrapper();
    
    [HttpGet("own")]
    public async Task<IActionResult> GetOwnAsync(int? folderId, int page, int pageSize, string? search, CancellationToken cancellationToken)
        => (await _mediator.Send(new OwnPostsQuery(folderId, page, pageSize, search), cancellationToken)).ToActionResultWrapper();
    
    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetOwnAsync(int userId,int? folderId, int page, int pageSize, string? search, CancellationToken cancellationToken)
        => (await _mediator.Send(new UserPostsQuery(userId, folderId, page, pageSize, search), cancellationToken)).ToActionResultWrapper();

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreatePostCommand request, CancellationToken cancellationToken)
        => (await _mediator.Send(request, cancellationToken)).ToActionResultWrapper();
    
    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdatePostCommand request, CancellationToken cancellationToken)
        => (await _mediator.Send(request, cancellationToken)).ToActionResultWrapper();
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DelteAsync(int id, CancellationToken cancellationToken)
        => (await _mediator.Send(new DeletePostCommand(id), cancellationToken)).ToActionResultWrapper();
}