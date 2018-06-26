namespace MyHandmadeWebServer.Server.Http.Response
{
    using Enums;

    public class NotFoundResponse : HttpResponse
    {
        public NotFoundResponse()
            : base()
        {
            this.StatusCode = HttpStatusCode.NotFound;
        }
    }
}