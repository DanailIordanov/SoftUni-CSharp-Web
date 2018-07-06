namespace MyHandmadeWebServer.Server.Http.Response
{
    using Common;
    using Enums;

    public class RedirectResponse : HttpResponse
    {
        public RedirectResponse(string redirectUrl)
            : base()
        {
            CoreValidator.ThrowIfNullOrEmpty(redirectUrl, nameof(redirectUrl));

            this.StatusCode = HttpStatusCode.Found;
            this.AddHeader(HttpHeader.Location, redirectUrl);
        }
    }
}