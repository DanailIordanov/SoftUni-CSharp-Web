namespace MyHandmadeWebServer.Server.HTTP.Contracts
{
    public interface IHttpContext
    {
        IHttpRequest Request { get; }
    }
}
