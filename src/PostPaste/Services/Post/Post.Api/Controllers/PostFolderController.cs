using MediatR;
using Microsoft.AspNetCore.Mvc;
using Post.Api.Application.Commands.PostFolder.Create;
using Post.Api.Application.Commands.PostFolder.Delete;
using Post.Api.Application.Commands.PostFolder.Update;
using Post.Api.Application.Queries.PostFolder;
using Post.Api.Extensions;
using Post.Api.Filters;
using Post.Domain.Constants;

namespace Post.Api.Controllers;

[ApiController]
[Route("post-folders")]
[AppAuthorize(Roles.User | Roles.Admin)]
public class PostFolderController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public PostFolderController(IMediator mediator) 
        => _mediator = mediator;

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
        => (await _mediator.Send(new PostFolderQuery(id), cancellationToken)).ToActionResultWrapper();
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreatePostFolderCommand request, CancellationToken cancellationToken)
        => (await _mediator.Send(request, cancellationToken)).ToActionResultWrapper();
    
    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdatePostFolderCommand request, CancellationToken cancellationToken)
        => (await _mediator.Send(request, cancellationToken)).ToActionResultWrapper();
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        => (await _mediator.Send(new DeletePostFolderCommand(id), cancellationToken)).ToActionResultWrapper();
}