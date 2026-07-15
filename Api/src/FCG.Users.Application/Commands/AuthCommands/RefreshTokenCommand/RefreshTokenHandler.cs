using FCG.Users.Application.Interfaces.Security;
using FCG.Users.Application.Responses;
using FCG.Users.Core.UnitOfWork;
using FCG.Users.Domain.Entities;
using MediatR;

namespace FCG.Users.Application.Commands.AuthCommands.RefreshTokenCommand;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, TokenResponse>
{
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenHandler(ITokenService tokenService, IUnitOfWork unitOfWork)
    {
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<TokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var tokenRepository = _unitOfWork.GetRepository<RefreshToken>();
        var usuarioRepository = _unitOfWork.GetRepository<Usuario>();

        var tokenUsuario = await tokenRepository.GetByAsync(x => x.Token == request.RefreshToken, cancellationToken: cancellationToken);
        // Busca o token atual no banco

        // Valida utilizando as regras e métodos da SUA classe
        if (tokenUsuario == null || tokenUsuario.IsExpired() || tokenUsuario.IsRevoked())
        {
            
            throw new UnauthorizedAccessException("Token inválido, expirado ou já revogado.");
        }

        // Busca o usuário associado
        var usuario = await usuarioRepository.GetByAsync(x => x.Id == tokenUsuario.IdUsuario, cancellationToken: cancellationToken);
        if (usuario == null)
        {
            throw new UnauthorizedAccessException("Usuário não encontrado.");
        }

        // Executa o método de revogação que você criou na imagem
        tokenUsuario.Revoke();
        tokenRepository.Update(tokenUsuario);

        // Gera as novas credenciais
        var newAccessToken = _tokenService.GerarToken(usuario);
        var newRefreshTokenString = _tokenService.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddDays(7);

        // Cria o novo objeto usando o seu construtor
        var newRefreshToken = new RefreshToken(newRefreshTokenString, expiresAt, usuario.Id);

        // Salva o novo token gerado
        await tokenRepository.AddAsync(newRefreshToken, cancellationToken);

        await _unitOfWork.SaveChanges();

        return new TokenResponse(newAccessToken, newRefreshTokenString);
    }
}
