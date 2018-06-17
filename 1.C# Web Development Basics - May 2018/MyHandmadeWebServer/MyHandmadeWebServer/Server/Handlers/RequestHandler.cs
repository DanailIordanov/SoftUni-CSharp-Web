namespace MyHandmadeWebServer.Server.Handlers
{
    using MyHandmadeWebServer.Server.Handlers.Contracts;
    using MyHandmadeWebServer.Server.Http.Contracts;

    using System;

    public abstract class RequestHandler : IRequestHandler
    {
        private readonly Func<IHttpRequest, IHttpResponse> handlingFunc;

        public RequestHandler(Func<IHttpRequest, IHttpResponse> handlingFunc)
        {
            this.handlingFunc = handlingFunc;
        }

        public IHttpResponse Handle(IHttpContext context)
        {
            var response = this.handlingFunc(context.Request);

            response.AddHeader("Content-Type", "text/html");

            return response;
        }
    }
}
