namespace MyHandmadeWebServer.Server.Http.Response
{
    using MyHandmadeWebServer.Server.Enums;
    using MyHandmadeWebServer.Server.Http.Contracts;

    using System.Text;

    public abstract class HttpResponse : IHttpResponse
    {
        protected HttpResponse()
        {
            this.Headers = new HttpHeaderCollection();
        }

        public IHttpHeaderCollection Headers { get; }

        public HttpStatusCode StatusCode { get; protected set; }

        private string StatusMessage => this.StatusCode.ToString();

        public void AddHeader(string key, string value)
        {
            var header = new HttpHeader(key, value);
            this.Headers.Add(header);
        }

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
