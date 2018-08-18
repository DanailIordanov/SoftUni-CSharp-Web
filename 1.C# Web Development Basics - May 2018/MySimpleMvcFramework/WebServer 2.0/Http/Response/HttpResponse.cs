namespace WebServer.Http.Response
{
    using Contracts;
    using Enums;

    using System.Text;

    public abstract class HttpResponse : IHttpResponse
    {
        protected HttpResponse()
        {
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();
        }
        
        public IHttpHeaderCollection Headers { get; }

        public IHttpCookieCollection Cookies { get; }

        public HttpStatusCode StatusCode { get; protected set; }

        private string StatusMessage => this.StatusCode.ToString();

        public override string ToString()
        {
            var response = new StringBuilder();
            var statusCode = (int)this.StatusCode;

            response.AppendLine($"HTTP/1.1 {statusCode} {this.StatusMessage}");
            response.AppendLine(this.Headers.ToString());

            return response.ToString();
        }
    }
}
