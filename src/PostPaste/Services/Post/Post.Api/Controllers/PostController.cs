using MediatR;
using Microsoft.AspNetCore.Mvc;
using Post.Api.Application.Commands.Post.Create;
using Post.Api.Application.Queries.Post;
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

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreatePostCommand request, CancellationToken cancellationToken)
        => (await _mediator.Send(request, cancellationToken)).ToActionResultWrapper();
}