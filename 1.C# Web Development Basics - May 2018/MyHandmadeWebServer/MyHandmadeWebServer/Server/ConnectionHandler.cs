namespace MyHandmadeWebServer.Server
{
    using MyHandmadeWebServer.Server.Handlers;         
    using MyHandmadeWebServer.Server.Http;
    using MyHandmadeWebServer.Server.Http.Contracts;
    using MyHandmadeWebServer.Server.Routing.Contracts;

    using System;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    public class ConnectionHandler
    {
        private readonly Socket client;

        private readonly IServerRouteConfig serverRouteConfig;

        public ConnectionHandler(Socket client, IServerRouteConfig serverRouteConfig)
        {
            this.client = client;
            this.serverRouteConfig = serverRouteConfig;
        }

        public async Task ProcessRequestAsync()
        {
            var httpRequest = await this.ReadRequest();

            var context = new HttpContext(httpRequest);
            var handler = new HttpHandler(this.serverRouteConfig);

            var httpResponse = handler.Handle(context);
            var responseSegments = new ArraySegment<byte>(Encoding.ASCII.GetBytes(httpResponse.ToString()));

            await this.client.SendAsync(responseSegments, SocketFlags.None);

            Console.WriteLine(httpRequest);
            Console.WriteLine(httpResponse.ToString());

            this.client.Shutdown(SocketShutdown.Both);
        }

        private async Task<IHttpRequest> ReadRequest()
        {
            var requestString = new StringBuilder();
            var data = new ArraySegment<byte>(new byte[1024]);

            while (true)
            {
                var readBytesCount = await this.client.ReceiveAsync(data, SocketFlags.None);

                if (readBytesCount == 0)
                {
                    break;
                }

                var bytesAsString = Encoding.ASCII.GetString(data.Array);
                requestString.Append(bytesAsString);

                if (readBytesCount < 1023)
                {
                    break;
                }
            }

            if (requestString.Length == 0)
            {
                return null;
            }

            return new HttpRequest(requestString.ToString());
        }
    }
}
