namespace MyHandmadeWebServer.Server.Common
{
    using Server.Contracts;

    using System;

    public class InternalServerErrorView : IView
    {
        private readonly Exception exception;

        public InternalServerErrorView(Exception exception)
        {
            this.exception = exception;
        }

        public string View() => $"<h1>500 {this.exception.Message}</h1><h2>{this.exception.StackTrace}</h2>";
    }
}
