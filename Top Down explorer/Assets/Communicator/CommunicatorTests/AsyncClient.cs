using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class AsyncClient
    {
        private IPAddress ip;
        private int port;
        public TcpClient client = new TcpClient();
        StreamReader reader;


        public AsyncClient(IPAddress ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        public Task StartConnection()
        {
            Task connection = client.ConnectAsync(ip, port);
            return connection;
        }

        public void OverideClient(TcpClient newClient)
        {
            client = newClient;
        }

        public Task SetReaderA()
        {
            return Task.Run(() =>
            {
                Debug.Log("Setting Stream");
                Assert.IsNotNull(client);
                reader = new StreamReader(client.GetStream());
                Debug.Log("Stream set");
            });
        }

        public Task WriteLineA(string message)
        {
            using StreamWriter writer = new StreamWriter(client.GetStream());
            return writer.WriteLineAsync(message);
        }
        public Task<string> ReadLineTask()
        {
            Assert.IsNotNull(reader);
            return reader.ReadLineAsync();
        }

        public void Close()
        {
            client.Close();
            reader?.Dispose();
        }
    }
}