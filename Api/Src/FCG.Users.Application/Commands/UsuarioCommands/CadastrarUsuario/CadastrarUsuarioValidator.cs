using FluentValidation;

namespace FCG.Users.Application.Commands.UsuarioCommands.CadastrarUsuario;

public class CadastrarUsuarioValidator : AbstractValidator<CadastrarUsuarioCommand>
{
    public CadastrarUsuarioValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithErrorCode("O nome é obrigatório.")
            .MinimumLength(3);

        RuleFor(x => x.Email)
            .NotEmpty().WithErrorCode("O email é obrigatório.")
            .EmailAddress().WithErrorCode("O email deve ser válido.");

        RuleFor(x => x.Senha)
            .NotEmpty().WithErrorCode("A senha é obrigatória.")
            .MinimumLength(8).WithErrorCode("A senha deve ter no mínimo 8 caracteres.")
            .Matches("[0-9]").WithErrorCode("A senha deve conter número")
            .Matches("[A-Z]").WithMessage("Senha deve conter letra maiúscula")
            .Matches("[^a-zA-Z0-9]").WithMessage("Senha deve conter caractere especial");
    }
}
