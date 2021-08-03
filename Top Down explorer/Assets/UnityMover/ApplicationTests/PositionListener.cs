using System;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityMover;

public class PositionListener : MonoBehaviour
{
    public LocalListener listener;
    public int port = 9999;
    private MessageIntepretor interpretor;
    public float refreshRate = 1;
    private WaitForSeconds refreshWait;
    private MessageHandler myMessageHandler;
    private GameObject marker;

    private void Awake()
    {
        listener = new LocalListener(IPAddress.Loopback, port, new ListenAndMove());
        interpretor = new MessageIntepretor(GetComponent<Moveable>());
    }

    private void Start()
    {
        refreshWait = new WaitForSeconds(refreshRate);
        StartCoroutine(StartListener());
    }

    private IEnumerator StartListener()
    {
        yield return null;
        listener.Start();
        Debug.Log("Started listener" + port);
        myMessageHandler = listener.MessageHandler();
        yield return UpdateDestination();
    }

    private IEnumerator UpdateDestination()
    {
        while (true)
        {
            yield return refreshWait;

            interpretor.Receive(myMessageHandler.ReadAndParse());
            Debug.Log("listener Moved");
        }
    }

    private void OnDestroy()
    {
        listener.Stop();
    }
}