namespace FCG.Users.Api.Erros;

public class ApiException
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Detail { get; set; }
}