using FCG.Users.Api.Controllers.Models;
using FCG.Users.Application.Commands.UsuarioCommands.CadastrarUsuario;
using FCG.Users.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FCG.Users.Api.Controllers;

[Authorize]
[ApiController]
[Route("usuarios")]
public class UsuarioController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUsuarioQueryService _queryService;

    public UsuarioController(IMediator mediator, IUsuarioQueryService queryService)
    {
        _mediator = mediator;
        _queryService = queryService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> BuscarPerfilAtual()
    {
        var idUsuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(idUsuarioClaim, out Guid id))
            return Unauthorized("Token inválido.");

        var result = await _queryService.ObterUsuarioPorIdAsync(id, CancellationToken.None);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> UsuarioPorId(Guid id)
    {

        var result = await _queryService.ObterUsuarioPorIdAsync(id, CancellationToken.None);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> ObterUsuarios()
    {
        var result = await _queryService.ObterTodosUsuariosAsync(CancellationToken.None);
        return Ok(result);
    }
}
