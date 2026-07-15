using FCG.Users.Application.Interfaces.Security;
using FCG.Users.Application.Responses;
using FCG.Users.Core.UnitOfWork;
using FCG.Users.Domain.Entities;
using MediatR;

namespace FCG.Users.Application.Commands.AuthCommands.LoginCommand;

public class LoginHandler : IRequestHandler<LoginCommand, TokenResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IHashSenha _hashSenha;

    public LoginHandler(IUnitOfWork unitOfWork, ITokenService tokenService, IHashSenha hashSenha)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _hashSenha = hashSenha;
    }

    public async Task<TokenResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var usuarioRepository = _unitOfWork.GetRepository<Usuario>();
        var tokenRepository = _unitOfWork.GetRepository<RefreshToken>();

        var usuario = await usuarioRepository.GetByAsync(predicate: u => u.Email == request.Email, cancellationToken: cancellationToken);

        if (usuario == null)
            throw new Exception("Usuário inválido");

        var senhaValida = _hashSenha.VerificarHash(request.Senha, usuario.Senha);

        if (!senhaValida)
            throw new Exception("Senha inválida");

        var token = _tokenService.GerarToken(usuario);

        var refreshTokenString = _tokenService.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddDays(7);

        var refreshTokenEntity = new RefreshToken(refreshTokenString, expiresAt, usuario.Id);

        // Salva o Refresh Token no banco de dados
        await tokenRepository.AddAsync(refreshTokenEntity, cancellationToken);

        await _unitOfWork.SaveChanges();

        return new TokenResponse(token, refreshTokenString);

    }
}
