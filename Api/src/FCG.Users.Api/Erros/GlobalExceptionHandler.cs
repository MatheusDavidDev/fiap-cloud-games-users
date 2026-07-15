using Microsoft.AspNetCore.Diagnostics;

namespace FCG.Users.Api.Erros;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        context.Response.ContentType = "application/json";

        ApiException response;

        switch (exception)
        {
            case FluentValidation.ValidationException validationException:

                context.Response.StatusCode = 400;

                response = new ApiException
                {
                    StatusCode = 400,
                    Message = "Erro de validação.",
                    Detail = string.Join(
                        ", ",
                        validationException.Errors
                            .Select(x => x.ErrorMessage))
                };

                break;

            case UnauthorizedAccessException:

                context.Response.StatusCode = 401;

                response = new ApiException
                {
                    StatusCode = 401,
                    Message = "Não autorizado."
                };

                break;

            default:

                context.Response.StatusCode = 500;

                response = new ApiException
                {
                    StatusCode = 500,
                    Message = exception.Message,
                    Detail = exception.InnerException?.Message
                };

                break;
        }

        await context.Response.WriteAsJsonAsync(
            response,
            cancellationToken);

        return true;
    }
}
