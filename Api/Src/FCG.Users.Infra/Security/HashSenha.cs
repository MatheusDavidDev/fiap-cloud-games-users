using FCG.Users.Application.Interfaces.Security;

namespace FCG.Users.Infra.Security;

public class HashSenha : IHashSenha
{
    public string GerarHash(string senha)
    {
        return BCrypt.Net.BCrypt.HashPassword(senha);
    }

    public bool VerificarHash(string senha, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(senha, hash);
    }
}
