namespace WebServer.Http.Response
{
    using Enums;

    public class BadRequestResponse : HttpResponse
    {
        public BadRequestResponse()
            : base()
        {
            this.StatusCode = HttpStatusCode.BadRequest;
        }
    }
}
