using FluentValidation;

namespace FCG.Users.Application.Commands.AuthCommands.RefreshTokenCommand;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("O Refresh Token é obrigatório.");
    }
}

