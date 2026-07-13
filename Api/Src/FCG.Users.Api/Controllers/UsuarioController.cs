using FCG.Users.Api.Controllers.Models;
using FCG.Users.Application.Commands.UsuarioCommands.CadastrarUsuario;
using FCG.Users.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Users.Api.Controllers;

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

    [HttpPost]
    public async Task<IActionResult> Criar(CriarUsuarioModel model)
    {
        await _mediator.Send(new CadastrarUsuarioCommand(model.Nome, model.Email, model.TipoUsuario, model.Senha));
        return Ok();
    }

    //[Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> UsuarioPorId(Guid id)
    {
        var result = await _queryService.ObterUsuarioPorIdAsync(id, CancellationToken.None);
        return Ok();
    }

    //[Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> ObterUsuarios()
    {
        var result = await _queryService.ObterTodosUsuariosAsync(CancellationToken.None);
        return Ok();
    }
}
