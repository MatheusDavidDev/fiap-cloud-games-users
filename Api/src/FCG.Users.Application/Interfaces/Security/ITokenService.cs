using FCG.Users.Domain.Entities;

namespace FCG.Users.Application.Interfaces.Security;

public interface ITokenService
{
    string GerarToken(Usuario usuario);

    public string GenerateRefreshToken();
}
