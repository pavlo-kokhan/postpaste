using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Post.Api.Application.Commands.User;
using Post.Api.Extensions;

namespace Post.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) 
        => _mediator = mediator;

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync(RegisterUserCommand request, CancellationToken cancellationToken)
        => (await _mediator.Send(request, cancellationToken)).ToActionResultWrapper();
    
    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmUserEmailAsync(int id, string token, CancellationToken cancellationToken)
        => (await _mediator.Send(new ConfirmUserEmailCommand(id, token), cancellationToken)).ToActionResultWrapper();

    [HttpPost("login")]
    public async Task<IActionResult> LoginUserAsync(LoginUserCommand request, CancellationToken cancellationToken)
        => (await _mediator.Send(request, cancellationToken)).ToActionResultWrapper();
}