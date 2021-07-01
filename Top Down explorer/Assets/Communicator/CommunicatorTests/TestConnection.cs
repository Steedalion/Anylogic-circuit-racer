using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

public class TestConnection
{
    private bool recievedString = false;
    int port = 9999;
    private AnySyncMock anySync;

    [SetUp]
    public void CreateClient()
    {
        anySync = new AnySyncMock(IPAddress.Loopback, port);
    }

    [TearDown]
    public void CloseExistingConnections()
    {
        anySync.Stop();
        anySync = null;
    }

    [Test]
    public void StartAndStopClient()
    {
        anySync.Start();
        anySync.Stop();
    }

    [Test]
    public void ClientConnectsToAnyListener()
    {
        anySync.Start();
        LocalListener listener = new LocalListener(IPAddress.Any, port, new DoNothing());
        listener.Start();
        listener.Stop();
    }

    [Test]
    public void LocalListenerPrints()
    {
        anySync.Start();
        LocalListener listener = new LocalListener(IPAddress.Any, port, new StoreMessage());
        listener.Start();
        StoreMessage storeMessage = listener.MessageHandler() as StoreMessage;
        Assert.NotNull(storeMessage);
        storeMessage.ReadAndParse();
        Assert.AreEqual("Hello ", storeMessage.lastMessage);
    }

    // [Test]
    public void ListenerStartsWithoutSender()
    {
        LocalListener listener = new LocalListener(IPAddress.Any, port, new DoNothing());
        listener.Start();
        listener.Stop();
    }

    public class StoreMessage : MessageHandler
    {
        public string lastMessage;

        public void ParseAndAct(string message)
        {
            Debug.Log(message);
            lastMessage = message;
        }

        public override string ReadAndParse()
        {
            string mes = myStream.ReadLine();
            ParseAndAct(mes);
            return mes;
        }
    }

    public class DoNothing : MessageHandler
    {
        public override string ReadAndParse()
        {
            return null;
        }
    }
}


public class LocalListener
{
    private int port;
    protected TcpListener listener;
    protected TcpClient client;
    protected StreamReader reader;
    protected MessageHandler myMessageHandler;

    public LocalListener(IPAddress ip, int port, MessageHandler messageHandler)
    {
        listener = new TcpListener(ip, port);
        this.port = port;
        this.myMessageHandler = messageHandler;
    }

    public void Start()
    {
        listener.Start();
        Console.WriteLine("Waiting for client to connect " + port);
        client = listener.AcceptTcpClient();
        Debug.Log("ClientAccepted");
        NetworkStream stream = client.GetStream();
        reader = new StreamReader(stream);

        myMessageHandler.SetStream(reader);
        //TODO, should not hang if there is no line.
    }

    public void Stop()
    {
        listener.Stop();
        client.Close();
    }

    public MessageHandler MessageHandler()
    {
        return myMessageHandler;
    }
}


public class AnySyncMock
{
    private int myport = 9999;
    private IPAddress ip;
    private readonly ClientHandler clientHandler;

    private Thread t1;

    public void Start()
    {
        if (clientHandler != null)
        {
            t1 = new Thread(() => clientHandler.Connect(ip, myport));
            t1.Start();
        }
    }

    public AnySyncMock(IPAddress ip, int port)
    {
        myport = port;
        this.ip = ip;
        clientHandler = new ClientHandler();
    }

    public AnySyncMock(IPAddress ip, int port, ClientHandler sclientHandler)
    {
        myport = port;
        this.ip = ip;
        clientHandler = sclientHandler;
    }

    public void Stop()
    {
        t1?.Interrupt();
        clientHandler?.Close();
    }

    public class ClientHandler
    {
        public NetworkStream stream;
        private TcpClient myClient = new TcpClient();


        internal void Connect(IPAddress ip, int myport)
        {
            myClient.Connect(ip, myport);
            stream = myClient.GetStream();
            SendOne();
        }

        public virtual void SendOne()
        {
            byte[] word = Encoding.ASCII.GetBytes("Hello \n");
            Debug.Log("Send hello");
            int length = word.Length;
            stream.Write(word, 0, length);
        }

        public void Close()
        {
            myClient.Close();
        }
    }
}