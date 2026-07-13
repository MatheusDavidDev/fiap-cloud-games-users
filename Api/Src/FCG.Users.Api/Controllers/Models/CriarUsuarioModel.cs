namespace FCG.Users.Api.Controllers.Models;

public record CriarUsuarioModel(string Nome, string Email, string Senha, int TipoUsuario);

