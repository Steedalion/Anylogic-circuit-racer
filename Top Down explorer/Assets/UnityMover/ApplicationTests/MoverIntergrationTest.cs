using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using NUnit.Framework;
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

        GameObject.Destroy(uAgent);
        GameObject.Destroy(aAgent);
    }

}


internal class ListenAndMove : MessageHandler
{
    public override string ReadAndParse()
    {
        if (myStream.Peek() < 1)
        {
            return null;
        }

        string msg = myStream.ReadLine();
        Debug.Log( "Reading message:" +msg);
        return msg;
    }
}


public class MockSender : MonoBehaviour
{
    public AnySyncMock.ClientHandler myClientHandler = new SendPositions();
    private AnySyncMock sender;
    private static int port = 9999;

    private void Awake()
    {
        sender = new AnySyncMock(IPAddress.Loopback, port, myClientHandler);
        sender.Start();
        Debug.Log("Started sender");
    }

    private void OnDestroy()
    {
        sender.Stop();
    }
}

internal class SendPositions : AnySyncMock.ClientHandler
{
    public override void SendOne()
    {
        StreamWriter writer = new StreamWriter(stream);
        writer.WriteLine("Move : (1,1,1)");
        writer.WriteLine("Move : (1,2,1)");
        writer.WriteLine("Move : (1,3,1)");
        writer.Flush();
    }
    //   public virtual void SendOne()
    // {
    //      byte[] word = Encoding.ASCII.GetBytes("Hello \n");
    //     Debug.Log("Send hello");
    //     int length = word.Length;
    //     stream.Write(word, 0, length);
    // }
}
