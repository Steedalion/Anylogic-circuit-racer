using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class TestAsyncCon
    {
        AsyncClient client;
        AsyncServer server;

        [SetUp]
        public void CreateClientAndServer()
        {
            server = new AsyncServer(IPAddress.Loopback, 9999);
            client = new AsyncClient(IPAddress.Loopback, 9999);
        }

        [TearDown]
        public void CloseConnections()
        {
            client.Close();
            server.Close();
        }


        [Test]
        public void ConnectDisconnect()
        {
            Task<TcpClient> serverCon = server.StartListening();
            Assert.False(serverCon.IsCompleted);
            Assert.False(serverCon.IsCompleted);
            Assert.False(serverCon.IsCompleted);
            Task clientCon = client.StartConnection();
            Assert.False(serverCon.IsCompleted);
            Assert.False(clientCon.IsCompleted);
            clientCon.Wait();
            Assert.IsTrue(clientCon.IsCompleted);
            serverCon.Wait();
            Assert.IsTrue(serverCon.IsCompleted);
            Assert.IsFalse(serverCon.IsFaulted);
            Assert.IsFalse(clientCon.IsFaulted);
        }

        [Test]
        public void ConnectDisconnectAsync()
        {
            server.StartListenAsync();
            Task clientCon = client.StartConnection();
            clientCon.Wait();
            Assert.IsTrue(clientCon.IsCompleted);
        }

        [Test]
        public void SendAndReceive()
        {
            server.StartListenAsync();
            client.StartConnectionAsync();
            Task<string> line = client.ReadLineTask();
            Debug.Log(line.Result);
        }

        [Test]
        public void UnconnectedClient()
        {
            var con = client.StartConnection();
            Assert.IsFalse(con.IsCompleted);
            Assert.IsFalse(con.IsCompleted);
            Assert.IsFalse(con.IsCompleted);
            Assert.IsFalse(con.IsCompleted);
            server.StartListenAsync();
            con.Wait();
            Assert.IsTrue(con.IsCompleted);
        }

        [Test]
        public void ConnectAndOpenStream()
        {
            var scon = server.StartListening();
            var ccon = client.StartConnection();
            ccon.Wait();
            Assert.IsTrue(ccon.IsCompleted);
            scon.Wait();
            Assert.IsTrue(client.client.Connected);
            Assert.IsTrue(scon.IsCompleted);


            Task streamset = client.SetStreamTask();
            streamset.Wait();
        }

        [Test]
        public void ReceiveMsgFromServer()
        {
            var scon = server.StartListening();
            var ccon = client.StartConnection();
            ccon.Wait();
            Assert.IsTrue(ccon.IsCompleted);
            scon.Wait();
            Assert.IsTrue(client.client.Connected);
            Assert.IsTrue(scon.IsCompleted);
            var sndmsg = server.Sendmsg(scon.Result);
            sndmsg.Wait();

            Task streamset = client.SetStreamTask();
            streamset.Wait();
            var readLineTask = client.ReadLineTask();
            Debug.Log(readLineTask.Result);
        }

        [Test]
        public void MultipleConnection()
        {
            AsyncServer server2 = new AsyncServer(IPAddress.Loopback, 9998);
            AsyncClient client2 = new AsyncClient(IPAddress.Loopback, 9998);

            Task completeConnections = Task.WhenAll(
                server2.StartListening(),
                server.StartListening(),
                client2.StartConnection(),
                client.StartConnection()
            );
            completeConnections.Wait();
            Assert.IsTrue(client2.client.Connected);
            Assert.IsTrue(client.client.Connected);
        }

        [Test]
        public void MultipleConnections()
        {
            AsyncServer server2 = new AsyncServer(IPAddress.Loopback, 9998);
            AsyncClient client2 = new AsyncClient(IPAddress.Loopback, 9998);
            server2.StartListening();
            Task completeConnections = Task.WhenAll(
                server.StartListening(),
                client.StartConnection()
            );

            completeConnections.Wait();
            Assert.IsFalse(client2.client.Connected);
            Assert.IsTrue(client.client.Connected);
            client2.StartConnection().Wait();
            Assert.IsTrue(client2.client.Connected);
        }
    }

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

        public void SetStream()
        {
            reader = new StreamReader(client.GetStream());
            Debug.Log("Stream set");
        }

        public Task SetStreamTask()
        {
            return Task.Run(() =>
            {
                Debug.Log("Setting Stream");
                Assert.IsNotNull(client);
                reader = new StreamReader(client.GetStream());
                Debug.Log("Stream set");
            });
        }

        public async void StartConnectionAsync()
        {
            await client.ConnectAsync(ip, port);
            reader = new StreamReader(client.GetStream());
        }

        public Task<string> ReadLineTask()
        {
            Assert.IsNotNull(reader);
            return reader.ReadLineAsync();
        }

        public void Close()
        {
            client.Close();
        }
    }

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
            NetworkStream stream = tcpClient.GetStream();
            BufferedStream buf = new BufferedStream(stream);
            StreamWriter writer = new StreamWriter(stream);
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
}