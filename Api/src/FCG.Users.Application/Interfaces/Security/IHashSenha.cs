namespace FCG.Users.Application.Interfaces.Security;

public interface IHashSenha 
{
    public string GerarHash(string senha);

    public bool VerificarHash(string senha, string hash);

}
