namespace FCG.Users.Application.Queries.DTOs;

public class UsuarioDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string TipoUsuario { get; set; }
}
