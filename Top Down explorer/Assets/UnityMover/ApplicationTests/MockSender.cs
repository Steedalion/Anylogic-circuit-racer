using System.Net;
using UnityEngine;

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