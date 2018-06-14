namespace MyHandmadeWebServer.Server.HTTP.Contracts
{
    using MyHandmadeWebServer.Server.Enums;

    public interface IHttpResponse
    {
        HttpStatusCode StatusCode { get; }

        IHttpHeaderCollection Headers { get; }
    }
}
