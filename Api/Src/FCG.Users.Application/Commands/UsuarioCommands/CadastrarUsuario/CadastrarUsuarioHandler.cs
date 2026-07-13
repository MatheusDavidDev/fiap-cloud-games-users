using FCG.Users.Application.Interfaces.Security;
using FCG.Users.Core.UnitOfWork;
using FCG.Users.Domain.Entities;
using MassTransit;
using MediatR;

namespace FCG.Users.Application.Commands.UsuarioCommands.CadastrarUsuario;

public class CadastrarUsuarioHandler : IRequestHandler<CadastrarUsuarioCommand>
{
    private readonly IHashSenha _hashSenha;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;

    public CadastrarUsuarioHandler(IHashSenha hashSenha, IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
    {
        _hashSenha = hashSenha;
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(CadastrarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuarioRepository = _unitOfWork.GetRepository<Usuario>();

        var usuarioExistente = await usuarioRepository.GetByAsync(
            predicate: u => u.Email == request.Email,
            cancellationToken: cancellationToken);

        if (usuarioExistente != null)
        {
            throw new Exception("Email já cadastrado.");
        }

        var usuario = new Usuario(
            request.Nome,
            request.Email,
            _hashSenha.GerarHash(request.Senha),
            request.TipoUsuario);

        await usuarioRepository.AddAsync(usuario, cancellationToken);

        await _unitOfWork.SaveChanges();

        //await _publishEndpoint.Publish(
        //    new UsuarioCreatedEvent(usuario.Id,usuario.Nome,usuario.Email,DateTime.UtcNow), cancellationToken);
    }
}
