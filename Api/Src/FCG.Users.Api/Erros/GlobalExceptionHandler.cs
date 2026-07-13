using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace FCG.Users.Api.Erros;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        context.Response.ContentType = "application/json";

        context.Response.StatusCode = exception switch
        {
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized, // 401
            _ => (int)HttpStatusCode.InternalServerError                    // 500
        };

        // Usa a sua classe padrão ApiException que você já tinha criado
        var response = new ApiException(context.Response.StatusCode.ToString(), exception.Message);

        // Devolve o JSON limpo para o usuário da API
        await context.Response.WriteAsJsonAsync(response, cancellationToken);

        return true; // Informa ao .NET que o erro já foi tratado
    }
}

