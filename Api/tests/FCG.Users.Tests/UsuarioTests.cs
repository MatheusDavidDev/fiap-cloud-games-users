using FCG.Users.Domain.Entities;
using Xunit;

namespace FCG.Users.Tests;

public class UsuarioTests
{
    [Fact]
    public void CriarUsuario_DeveArmazenarDadosCorretamente()
    {
        // Arrange
        var nome = "Matheus";
        var email = "teste@email.com";
        var senha = "Senha12345@";

        // Act
        var usuario = new Usuario(nome, email, senha, default);

        // Assert
        Assert.Equal(nome, usuario.Nome);
        Assert.Equal(email, usuario.Email);
        Assert.Equal(senha, usuario.Senha);
        Assert.Empty(usuario.RefreshTokens);
    }
}
