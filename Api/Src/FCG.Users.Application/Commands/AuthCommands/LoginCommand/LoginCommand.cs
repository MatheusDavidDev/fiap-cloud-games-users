using FCG.Users.Application.Responses;
using MediatR;

namespace FCG.Users.Application.Commands.AuthCommands.LoginCommand;

public class LoginCommand : IRequest<TokenResponse>
{
    public LoginCommand(string email, string senha)
    {
        Email = email;
        Senha = senha;
    }

    public string Email { get; set; }
    public string Senha { get; set; }
}
