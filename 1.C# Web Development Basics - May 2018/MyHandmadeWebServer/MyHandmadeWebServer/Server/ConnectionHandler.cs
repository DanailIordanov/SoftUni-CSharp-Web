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

            var httpContext = new HttpContext(httpRequest);
            var httpHandler = new HttpHandler(this.serverRouteConfig);

            var httpResponse = httpHandler.Handle(httpContext);
            var responseSegments = new ArraySegment<byte>(Encoding.ASCII.GetBytes(httpResponse.ToString()));

            await this.client.SendAsync(responseSegments, SocketFlags.None);

            Console.WriteLine("-------REQUEST-------");
            Console.WriteLine(httpRequest);
            Console.WriteLine("-------RESPONSE------");
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

                if (readBytesCount < 1024)
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