using System;
using System.Net;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestAsyncCon
    {
        protected AsyncClient client;
        protected AsyncServer server;

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
    }
}