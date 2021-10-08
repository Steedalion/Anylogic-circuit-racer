using System;
using System.Collections;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityMover;

[RequireComponent(typeof(Moveable))]
// [RequireComponent(typeof(MessageIntepretor))]
public class PositionListenerAsync : MonoBehaviour
{
    [SerializeField] private int port;
    [SerializeField] private float period = 0.1f;
    
    private AListener listener;
    private Task StartListenning;
    private Task<string> cmd;
    private bool wasConnected;
    private MessageIntepretor interpretor;
    private WaitForSeconds wait;


    private void Awake()
    {
        interpretor = new MessageIntepretor(GetComponent<Moveable>());
    }

    private void Start()
    {
        
        wait = new WaitForSeconds(period);
        listener = new AListener(IPAddress.Loopback, port);
        StartListenning = listener.ConnectAsync();
        Debug.Log("Waiting for Connection");
        StartCoroutine(WaitForConnection());
    }

    private IEnumerator WaitForConnection()
    {
        yield return wait;
        if (StartListenning.IsCompleted)
        {
            Debug.Log("Connected ");
            listener.SetStreamSync();
            cmd = listener.ReadLine();
            // StartListenning = listener.SetStream();
            // StartListenning.Start();
            yield return WaitForCommand();
        }

        yield return WaitForConnection();
    }

    private IEnumerator SetStream()
    {
        yield return wait;
        if (StartListenning.IsCompleted)
        {
            Debug.Log("Stream set to +" + listener);
            cmd = listener.ReadLine();
            yield return (WaitForCommand());
        }

        yield return SetStream();
    }

    private IEnumerator WaitForCommand()
    {
        yield return wait;
        if (cmd.IsCompleted)
        {
            Debug.Log(cmd.Result);
            interpretor.Receive(cmd.Result);
            cmd = listener.ReadLine();
        }

        yield return WaitForCommand();
    }

    private void OnDestroy()
    {
        listener.Close();
    }
}