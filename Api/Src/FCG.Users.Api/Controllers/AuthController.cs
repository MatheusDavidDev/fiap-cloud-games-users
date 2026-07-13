using FCG.Users.Api.Controllers.Models;
using FCG.Users.Application.Commands.AuthCommands.LoginCommand;
using FCG.Users.Application.Commands.AuthCommands.RefreshTokenCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Users.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var login = await _mediator.Send(new LoginCommand(model.Email, model.Senha));

        return Ok(login);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(string refreshToken)
    {
        var result = await _mediator.Send(new RefreshTokenCommand(refreshToken));
        return Ok(result);
    }
}
