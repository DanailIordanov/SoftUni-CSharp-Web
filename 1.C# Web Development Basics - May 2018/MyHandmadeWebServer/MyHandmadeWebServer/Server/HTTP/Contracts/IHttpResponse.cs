namespace MyHandmadeWebServer.Server.Http.Contracts
{
    using MyHandmadeWebServer.Server.Enums;

    public interface IHttpResponse
    {
        HttpStatusCode StatusCode { get; }

        IHttpHeaderCollection Headers { get; }

        void AddHeader(string key, string value);
    }
}
