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
            string sessionIdToSend = null;

            if (!context.Request.Cookies.ContainsKey(SessionStore.SessionCookieKey))
            {
                var sessionId = Guid.NewGuid().ToString();

                context.Request.Session = SessionStore.Get(sessionId);

                sessionIdToSend = sessionId;
            }

            var response = this.handlingFunc(context.Request);

            response.AddHeader(HttpHeader.ContentType, "text/html");

            if (sessionIdToSend != null)
            {
                response.AddHeader(HttpHeader.SetCookie, $"{SessionStore.SessionCookieKey}={sessionIdToSend}; HttpOnly; path=/");
            }

            foreach (var cookie in response.Cookies)
            {
                response.AddHeader(HttpHeader.SetCookie, cookie.ToString());
            }

            return response;
        }
    }
}