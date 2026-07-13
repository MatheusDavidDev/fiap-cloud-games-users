using FCG.Users.Core.Models;

namespace FCG.Users.Domain.Entities;

public class Usuario : BaseEntity
{
    public Usuario(string nome, string email, string senha, TipoUsuario tipo)
    {
        Nome = nome;
        Email = email;
        Senha = senha;
        Tipo = tipo;
    }

    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string Senha { get; private set; }
    public TipoUsuario Tipo { get; private set; }

    public ICollection<RefreshToken> RefreshTokens { get; private set; }
    = new List<RefreshToken>();
}
