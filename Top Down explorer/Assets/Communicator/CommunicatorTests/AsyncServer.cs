using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NUnit.Framework;
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


        public void Close()
        {
            listener.Stop();
        }

        public static Task Sendmsg(TcpClient client, string msg)
        {
            using (StreamWriter writer = new StreamWriter(client.GetStream()))
            {
                Task task = writer.WriteLineAsync(msg);
                // task.GetAwaiter().OnCompleted(() => writer.Flush());
                return task;
            }
        }
        public static Task<string> RecieveMsg(TcpClient client)
        {
            using (StreamReader writer = new StreamReader(client.GetStream()))
            {
                Task<string> task = writer.ReadLineAsync();
                // task.GetAwaiter().OnCompleted(() => writer.Flush());
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

        public Task EchoOnce(TcpClient client)
        {
            Assert.IsTrue(client.Connected, "SHould still be conencted");
            using StreamReader reader = new StreamReader(client.GetStream());
            string msg = reader.ReadLineAsync().Result;
            using (StreamWriter writer = new StreamWriter(client.GetStream()))
            {
                Task task = writer.WriteLineAsync(msg);
                // task.GetAwaiter().OnCompleted(() => writer.Flush());
                return task;
            }
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