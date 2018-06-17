namespace MyHandmadeWebServer.Server.Http
{
    using MyHandmadeWebServer.Server.Http.Contracts;

    public class HttpContext : IHttpContext
    {
        private readonly IHttpRequest request;

        public HttpContext(IHttpRequest request)
        {
            this.request = request;
        }

        public IHttpRequest Request => this.request;
    }
}
