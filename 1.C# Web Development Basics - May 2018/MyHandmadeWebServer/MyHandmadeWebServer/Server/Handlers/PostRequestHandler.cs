namespace MyHandmadeWebServer.Server.Handlers
{
    using Http.Contracts;

    using System;

    public class PostRequestHandler : RequestHandler
    {
        public PostRequestHandler(Func<IHttpRequest, IHttpResponse> handlingFunc)
            : base(handlingFunc)
        {
        }
    }
}