using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests
{
    public class TestMultipleAsyncConnections : TestAsyncCon
    {
        AsyncServer server2 = new AsyncServer(IPAddress.Loopback, 9998);
        AsyncClient client2 = new AsyncClient(IPAddress.Loopback, 9998);

        [Test, Timeout(100)]
        public void MultipleConnection()
        {
            Task<TcpClient> shandler;
            Task completeConnections = Task.WhenAll(
                shandler = server2.StartListening(),
                server.StartListening(),
                client2.StartConnection(),
                client.StartConnection()
            );
            completeConnections.Wait();
            Assert.IsTrue(client2.client.Connected);
            Assert.IsTrue(client.client.Connected);
            shandler.Result.Close();
            client2.Close();
            server2.Close();
        }

        [Test, Timeout(100)]
        public void ReuseMultipleConnections()
        {
            client2 = new AsyncClient(IPAddress.Loopback, 9998);
            Task<TcpClient> shandler =
            server2.StartListening();
            Task completeConnections = Task.WhenAll(
                server.StartListening(),
                client.StartConnection()
            );

            completeConnections.Wait();
            Assert.IsTrue(client.client.Connected);
            client2.StartConnection().Wait();
            Assert.IsTrue(client2.client.Connected);
            Assert.IsNotNull(shandler.Result);
            client2.Close();
            server2.Close();
        }
    }
}