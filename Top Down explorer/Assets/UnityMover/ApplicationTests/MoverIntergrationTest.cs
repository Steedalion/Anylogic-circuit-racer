using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

public class MoverIntergrationTest
{
    [Test]
    public void MoverIntergrationTestSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    [UnityTest]
    public IEnumerator SingleMovement()
    {
        GameObject uAgent = Object.Instantiate(new GameObject());
        GameObject aAgent = Object.Instantiate(new GameObject());
        yield return null;
        MockSender sender = aAgent.AddComponent<MockSender>();
        yield return null;
        uAgent.AddComponent<Teleporter>();
        PositionListener listen = uAgent.AddComponent<PositionListener>();

        yield return null;
        yield return null;
        yield return null;

        Assert.AreNotEqual(Vector3.zero, uAgent.transform.position);

        Object.Destroy(uAgent);
        Object.Destroy(aAgent);
    }

    [UnityTest]
    public IEnumerator SingleMovementAsync()
    {
        GameObject uAgent = Object.Instantiate(new GameObject());
        GameObject aAgent = Object.Instantiate(new GameObject());
        yield return null;
        MockSender sender = aAgent.AddComponent<MockSender>();
        yield return null;
        uAgent.AddComponent<Teleporter>();
        PositionListenerAsync listener = uAgent.AddComponent<PositionListenerAsync>();

        yield return null;
        yield return null;
        yield return null;
        yield return null;

        Assert.AreNotEqual(Vector3.zero, uAgent.transform.position);
    }
}

public class AsyncMoverIntergrationTest
{
}

internal class AListener
{
    public TcpListener listener;
    public TcpClient connection;
    private IPAddress ipaddress;
    private int port;

    public AListener(IPAddress ipAddress, int port)
    {
        this.ipaddress = ipAddress;
        this.port = port;
        listener = new TcpListener(ipAddress,port);
        listener.Start();
    }

    public Task ConnectAsync()
    {
        return listener.AcceptTcpClientAsync();
    }
    
    
}