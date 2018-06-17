namespace MyHandmadeWebServer.Server.Http.Response
{
    using MyHandmadeWebServer.Server.Enums;

    public class RedirectResponse : HttpResponse
    {
        public RedirectResponse(string redirectUrl)
            : base()
        {
            this.StatusCode = HttpStatusCode.Found;
            this.AddHeader("Location", redirectUrl);
        }
    }
}
