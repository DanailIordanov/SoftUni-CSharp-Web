namespace MyHandmadeWebServer.Server.Http.Response
{
    using Common;
    using Enums;

    using System;

    public class InternalServerErrorResponse : ViewResponse
    {
        public InternalServerErrorResponse(Exception exception) 
            : base(HttpStatusCode.InternalServerError, new InternalServerErrorView(exception))
        {
        }
    }
}
