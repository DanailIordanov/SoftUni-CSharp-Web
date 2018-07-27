namespace MyHandmadeWebServer.Server
{
    using Common;
    using Handlers;         
    using Http;
    using Http.Contracts;
    using Routing.Contracts;

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
            CoreValidator.ThrowIfNull(client, nameof(client));
            CoreValidator.ThrowIfNull(serverRouteConfig, nameof(serverRouteConfig));

            this.client = client;
            this.serverRouteConfig = serverRouteConfig;
        }

        public async Task ProcessRequestAsync()
        {
            var httpRequest = await this.ReadRequest();

            if (httpRequest != null)
            {
                var httpContext = new HttpContext(httpRequest);
                var httpHandler = new HttpHandler(this.serverRouteConfig);

                var httpResponse = httpHandler.Handle(httpContext);
                var responseSegments = new ArraySegment<byte>(Encoding.ASCII.GetBytes(httpResponse.ToString()));

                await this.client.SendAsync(responseSegments, SocketFlags.None);

                Console.WriteLine("-------REQUEST-------");
                Console.WriteLine(httpRequest);
                Console.WriteLine("-------RESPONSE------");
                Console.WriteLine(httpResponse);
                Console.WriteLine();
            }

            this.client.Shutdown(SocketShutdown.Both);
        }

        private async Task<IHttpRequest> ReadRequest()
        {
            var requestString = new StringBuilder();
            var data = new ArraySegment<byte>(new byte[1024]);

            while (true)
            {
                var readBytesCount = await this.client.ReceiveAsync(data.Array, SocketFlags.None);

                if (readBytesCount == 0)
                {
                    break;
                }

                var bytesAsString = Encoding.UTF8.GetString(data.Array, 0, readBytesCount);
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