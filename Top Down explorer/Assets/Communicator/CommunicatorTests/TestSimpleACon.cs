using System.Net.Sockets;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class TestSimpleACon : TestAsyncCon
    {
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

        [Test, Timeout(100)]
        public void SendAndReceive()
        {
            var con = server.StartListening();
            client.StartConnection().Wait();
            client.SetReaderA().Wait();
            server.Sendmsg(con.Result);
            Task<string> line = client.ReadLineTask();
            Assert.NotNull(line.Result);
            Debug.Log(line.Result);
            EchoTcpMessage echoTcpMessage = new EchoTcpMessage(con.Result);
            echoTcpMessage.MainLoop();
            client.WriteLineA("Hello");
            Task<string> echo = client.ReadLineTask();
            Assert.AreEqual("Hello", echo.Result);
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


            Task streamset = client.SetReaderA();
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

            Task streamset = client.SetReaderA();
            streamset.Wait();
            var readLineTask = client.ReadLineTask();
            Debug.Log(readLineTask.Result);
        }
    }
}