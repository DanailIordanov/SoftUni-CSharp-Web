namespace MyHandmadeWebServer.Server.Handlers
{
    using Common;
    using Contracts;
    using Http.Contracts;
    using MyHandmadeWebServer.Server.Http;
    using System;

    public class RequestHandler : IRequestHandler
    {
        private readonly Func<IHttpRequest, IHttpResponse> handlingFunc;

        public RequestHandler(Func<IHttpRequest, IHttpResponse> handlingFunc)
        {
            CoreValidator.ThrowIfNull(handlingFunc, nameof(handlingFunc));

            this.handlingFunc = handlingFunc;
        }

        public IHttpResponse Handle(IHttpContext context)
        {
            var response = this.handlingFunc(context.Request);

            response.AddHeader(HttpHeader.ContentType, "text/html");

            return response;
        }
    }
}