namespace MyHandmadeWebServer.Server.HTTP
{
    using MyHandmadeWebServer.Server.HTTP.Contracts;

    class HttpContext : IHttpContext
    {
        private readonly IHttpRequest request;

        public HttpContext(string requestString)
        {
            this.request = new HttpRequest(requestString);
        }

        public IHttpRequest Request => this.request;
    }
}
