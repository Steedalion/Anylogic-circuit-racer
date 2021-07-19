using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

namespace Tests
{
    public class AsyncServer
    {
        private IPAddress ip;
        private int port;
        TcpListener listener;


        public AsyncServer(IPAddress ip, int port)
        {
            this.ip = ip;
            this.port = port;
            listener = new TcpListener(ip, port);
        }

        public Task<TcpClient> StartListening()
        {
            listener.Start();
            Debug.Log("Waiting for connection");
            Task<TcpClient> result = listener.AcceptTcpClientAsync();
            return result;
        }

        public async void StartListenAsync()
        {
            listener.Start();
            try
            {
                while (true)
                {
                    Accept(await listener.AcceptTcpClientAsync());
                }
            }
            finally
            {
                listener.Stop();
            }
        }


        private async Task Accept(TcpClient tcpClient)
        {
            await Task.Yield();
            using NetworkStream stream = tcpClient.GetStream();
            using BufferedStream buf = new BufferedStream(stream);
            using StreamWriter writer = new StreamWriter(stream);
            await writer.WriteLineAsync("Connected @ " + ip);
        }


        public void Close()
        {
            listener.Stop();
        }

        public Task Sendmsg(TcpClient client)
        {
            using (StreamWriter writer = new StreamWriter(client.GetStream()))
            {
                var task = writer.WriteLineAsync("Connected @ " + ip + "\n");
                task.GetAwaiter().OnCompleted(() => writer.Flush());
                return task;
            }
        }
    }

    public class EchoTcpMessage
    {
        private readonly TcpClient client;
        private bool running;

        public EchoTcpMessage(TcpClient client)
        {
            this.client = client;
        }

        public async void MainLoop()
        {
            using Stream stream = client.GetStream();
            using StreamReader reader = new StreamReader(stream);
            using StreamWriter writer = new StreamWriter(stream);
            while (running)
            {
                string msg = reader.ReadLineAsync().Result;
                await writer.WriteLineAsync(msg);
            }
        }
    }
}