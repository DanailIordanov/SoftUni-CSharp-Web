namespace MyHandmadeWebServer.Server.Http.Response
{
    using Common;
    using Enums;
    using Exceptions;
    using Server.Contracts;

    public class ViewResponse : HttpResponse
    {
        private readonly IView view;

        public ViewResponse(HttpStatusCode statusCode, IView view)
            : base()
        {
            this.ValidateStatusCode(statusCode);
            CoreValidator.ThrowIfNull(view, nameof(view));

            this.StatusCode = statusCode;
            this.view = view;
        }

        private void ValidateStatusCode(HttpStatusCode statusCode)
        {
            if ((int)statusCode >= 300 && (int)statusCode < 400)
            {
                throw new InvalidResponseException("View responses need a status code below 300 or above 400 (inclusive).");
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()}{this.view.View()}";
        }
    }
}
