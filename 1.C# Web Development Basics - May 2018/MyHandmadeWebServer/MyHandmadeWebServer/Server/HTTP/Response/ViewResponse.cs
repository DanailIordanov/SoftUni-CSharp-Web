namespace MyHandmadeWebServer.Server.Http.Response
{
    using MyHandmadeWebServer.Server.Contracts;
    using MyHandmadeWebServer.Server.Enums;
    using MyHandmadeWebServer.Server.Exceptions;

    public class ViewResponse : HttpResponse
    {
        private readonly IView view;

        public ViewResponse(HttpStatusCode statusCode, IView view)
            : base()
        {
            this.ValidateStatusCode(statusCode);

            this.StatusCode = statusCode;
            this.view = view;
            this.AddHeader("ContentType", "text/html");
        }

        private void ValidateStatusCode(HttpStatusCode statusCode)
        {
            if ((int)statusCode >= 300 && (int)statusCode < 400)
            {
                throw new InvalidResponseException("View responses need a status code below 300 or above 400 (inclusive).");
            }
        }
    }
}
