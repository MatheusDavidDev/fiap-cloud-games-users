using FCG.Users.Domain.Entities;
using MediatR;

namespace FCG.Users.Application.Commands.UsuarioCommands.CadastrarUsuario;

public class CadastrarUsuarioCommand : IRequest
{
    public CadastrarUsuarioCommand(string nome, string email, int tipoUsuario, string senha)
    {
        Nome = nome;
        Email = email;
        Senha = senha;
        TipoUsuario = (TipoUsuario)tipoUsuario;
    }

    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public TipoUsuario TipoUsuario { get; set; }
}
