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
            server.StartListening();
            Task clientCon = client.StartConnection();
            clientCon.Wait();
            Assert.IsTrue(clientCon.IsCompleted);
        }

        // [Test, Timeout(100)]
        public void SendAndReceive()
        {
            Task<TcpClient> con = server.StartListening();
            Task.WhenAll(
                client.StartConnection(),
                client.SetReaderA(),
                con
            ).Wait();
            Assert.IsTrue(client.client.Connected, "Client should be connected");
            Assert.IsTrue(con.Result.Connected, "con.Result.Connected");

            EchoTcpMessage echoTcpMessage = new EchoTcpMessage(con.Result);
            var ec = echoTcpMessage.EchoOnce(con.Result);
            // client.WriteLineA("Hello from client").Wait();
            // ec.Wait();
            // Task<string> echo = client.ReadLineTask();
            // Assert.AreEqual("Hello", echo.Result);
            // Assert.AreEqual("Hello from client", client.ReadLineTask().Result);
        }

        [Test]
        public void UnconnectedClient()
        {
            var con = client.StartConnection();
            Assert.IsFalse(con.IsCompleted);
            Assert.IsFalse(con.IsCompleted);
            Assert.IsFalse(con.IsCompleted);
            Assert.IsFalse(con.IsCompleted);
            server.StartListening();
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
        public void ReceiveOneShotMessage()
        {
            var scon = server.StartListening();
            var ccon = client.StartConnection();
            ccon.Wait();
            Assert.IsTrue(ccon.IsCompleted);
            scon.Wait();
            Assert.IsTrue(client.client.Connected);
            Assert.IsTrue(scon.IsCompleted);
            var sndmsg = AsyncServer.Sendmsg(scon.Result, "Message from server");
            sndmsg.Wait();

            Task streamset = client.SetReaderA();
            streamset.Wait();
            var readLineTask = client.ReadLineTask();
            Debug.Log(readLineTask.Result);
            Assert.AreEqual("Message from server", readLineTask.Result);
        }

        [Test]
        public void ReceiveMultipleMessages()
        {
            var scon = server.StartListening();
            var ccon = client.StartConnection();
            ccon.Wait();
            Assert.IsTrue(ccon.IsCompleted);
            scon.Wait();
            Assert.IsTrue(client.client.Connected);
            Assert.IsTrue(scon.IsCompleted);
            MessageSender sender = new MessageSender(scon.Result);
            var sndmsg = sender.WriteLine("Message from server");
            var secondMessage = sender.WriteLine("Second message");

            secondMessage.Wait();
            sender.CloseStreams();
            Task streamset = client.SetReaderA();
            streamset.Wait();
            var readLineTask = client.ReadLineTask();
            Debug.Log(readLineTask.Result);
            Assert.AreEqual("Message from server", readLineTask.Result);
            var readSecondLine = client.ReadLineTask();
            Assert.AreEqual("Second message", readSecondLine.Result);
        }

        [Test]
        public void SendOneShotMessage()
        {
            var scon = server.StartListening();
            var ccon = client.StartConnection();
            ccon.Wait();
            scon.Wait();
            Assert.IsTrue(client.client.Connected);
            TcpClient clinetHandler = scon.Result;
            Assert.IsTrue(clinetHandler.Connected);
            var sndmsg = AsyncServer.Sendmsg(client.client, "Message from server");
            sndmsg.Wait();
            var readLineTask = AsyncServer.RecieveMsg(scon.Result);
            string message = readLineTask.Result;
            Assert.AreEqual("Message from server", message);
        }


        [Test, Timeout(100)]
        public void ReceiveMessage()
        {
            var scon = server.StartListening();
            var ccon = client.StartConnection();
            ccon.Wait();
            scon.Wait();

            MessageSender serverMessenger = new MessageSender(scon.Result);
            Task firstMessageWrite = serverMessenger.WriteLine("Message from server");
            firstMessageWrite.Wait();
            serverMessenger.WriteLine("Second message from server").Wait();
            serverMessenger.CloseStreams();
            Task streamset = client.SetReaderA();

            streamset.Wait();
            var readLineClient = client.ReadLineTask();
            Assert.AreEqual("Message from server", readLineClient.Result);
            Assert.AreEqual("Second message from server", client.ReadLineTask().Result);
        }


        [Test]
        public void SendMessageToServer()
        {
            var scon = server.StartListening();
            var ccon = client.StartConnection();
            ccon.Wait();
            Assert.IsTrue(ccon.IsCompleted);
            scon.Wait();
            Assert.IsTrue(client.client.Connected);
            Assert.IsTrue(scon.IsCompleted);
            Assert.IsTrue(scon.Result.Connected);
            Task<string> streamset = AsyncServer.RecieveMsg(scon.Result);

            MessageSender cmsg = new MessageSender(client.client);
            var sndmsg = cmsg.uWriteLine("Message from client");
            string message = streamset.Result;
            sndmsg.Wait();

            Debug.Log(message);
            Assert.AreEqual("Message from client", streamset.Result.Length > 0);
        }
    }
}