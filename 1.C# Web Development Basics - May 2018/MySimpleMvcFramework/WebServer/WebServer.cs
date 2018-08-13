namespace WebServer
{
    using Contracts;

    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    public class WebServer : IRunnable
    {
        private const string localHostIpAddress = "127.0.0.1";

        private readonly int port;

        private readonly IHandleable mvcRequestHandler;

        private readonly TcpListener tcpListener;

        private bool isRunning;

        public WebServer(int port, IHandleable mvcRequestHandler)
        {
            this.port = port;
            this.tcpListener = new TcpListener(IPAddress.Parse(localHostIpAddress), port);

            this.mvcRequestHandler = mvcRequestHandler;
        }

        public void Run()
        {
            this.tcpListener.Start();
            this.isRunning = true;

            Console.WriteLine($"Server running on {localHostIpAddress}:{this.port}");

            var task = Task.Run(this.ListenLoop);
            task.Wait();
        }

        private async Task ListenLoop()
        {
            while (this.isRunning)
            {
                var client = await this.tcpListener.AcceptSocketAsync();
                var connectionHandler = new ConnectionHandler(client, this.mvcRequestHandler);
                await connectionHandler.ProcessRequestAsync();
            }
        }
    }
}
