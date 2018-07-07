namespace MyHandmadeWebServer.Server.Http.Contracts
{
    using Enums;

    public interface IHttpResponse
    {
        HttpStatusCode StatusCode { get; }

        IHttpHeaderCollection Headers { get; }

        IHttpCookieCollection Cookies { get; }

        void AddHeader(string key, string value);

        void AddCookie(string key, string value);
    }
}