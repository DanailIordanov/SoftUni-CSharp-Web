namespace MyHandmadeWebServer.Server.Handlers
{
    using MyHandmadeWebServer.Server.Http.Contracts;

    using System;

    public class GetRequestHandler : RequestHandler
    {
        public GetRequestHandler(Func<IHttpRequest, IHttpResponse> handlingFunc)
            : base(handlingFunc)
        {
        }
    }
}
