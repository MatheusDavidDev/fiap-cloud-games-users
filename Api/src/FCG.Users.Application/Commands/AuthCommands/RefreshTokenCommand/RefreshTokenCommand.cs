using FCG.Users.Application.Responses;
using MediatR;

namespace FCG.Users.Application.Commands.AuthCommands.RefreshTokenCommand;

public class RefreshTokenCommand : IRequest<TokenResponse>
{
    public RefreshTokenCommand(string refreshToken)
    {
        RefreshToken = refreshToken;
    }

    public string RefreshToken { get; set; }
}
