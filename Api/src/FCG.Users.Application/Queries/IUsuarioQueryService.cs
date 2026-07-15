using FCG.Users.Application.Queries.DTOs;

namespace FCG.Users.Application.Queries;

public interface IUsuarioQueryService
{
    Task<UsuarioDto> ObterUsuarioPorIdAsync(Guid id, CancellationToken cancellationTokenq);
    Task<IEnumerable<UsuarioDto>> ObterTodosUsuariosAsync(CancellationToken cancellationToken);
}
