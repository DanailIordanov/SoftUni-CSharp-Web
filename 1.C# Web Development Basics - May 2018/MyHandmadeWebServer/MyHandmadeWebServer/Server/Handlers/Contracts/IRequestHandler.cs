namespace MyHandmadeWebServer.Server.Handlers.Contracts
{
    using MyHandmadeWebServer.Server.Http.Contracts;

    public interface IRequestHandler
    {
        IHttpResponse Handle(IHttpContext context);
    }
}
