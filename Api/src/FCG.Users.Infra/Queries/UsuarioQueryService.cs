using FCG.Users.Application.Queries;
using FCG.Users.Application.Queries.DTOs;
using FCG.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FCG.Users.Infra.Queries;

public class UsuarioQueryService : IUsuarioQueryService
{
    private readonly FcgUsersDbContext _context;

    public UsuarioQueryService(FcgUsersDbContext context)
    {
        _context = context;
    }

    public async Task<UsuarioDto> ObterUsuarioPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (usuario == null)
            return null;


        return new UsuarioDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            TipoUsuario = usuario.Tipo.ToString()
        };
    }

    public async Task<IEnumerable<UsuarioDto>> ObterTodosUsuariosAsync(CancellationToken cancellationToken)
    {
        var usuarios = await _context.Usuarios.ToListAsync(cancellationToken);

        return usuarios.OrderByDescending(x => x.CreatedAt)
            .Select(x => new UsuarioDto
            {
                Id = x.Id,
                Nome = x.Nome,
                Email = x.Email,
                TipoUsuario = x.Tipo.ToString()
            });
    }
}