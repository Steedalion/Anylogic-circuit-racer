using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public class PositionListenerAsync : MonoBehaviour
{
    [SerializeField] private int port;
    private AListener listener;
    private Task StartListenning;
    private bool wasConnected;

    private void Start()
    {
        listener = new AListener(IPAddress.Loopback, port);
        StartListenning = listener.ConnectAsync();
        Debug.Log("Waiting for Connection");
    }

    private void Update()
    {
        if(wasConnected) return;
        if (StartListenning.IsCompleted)
        {
            Debug.Log("Connected ");
            wasConnected = true;
        }
    }
}