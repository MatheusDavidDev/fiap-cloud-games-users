namespace FCG.Users.Api.Erros;

public class ApiException
{
    public ApiException(string statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
        //Detail = detail;
    }

    public string StatusCode { get; set; }
    public string Message { get; set; }
    //public string Detail { get; set; }
}
