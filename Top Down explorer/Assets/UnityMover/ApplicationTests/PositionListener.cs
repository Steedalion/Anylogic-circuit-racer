using System;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityMover;

public class PositionListener : MonoBehaviour
{
    public LocalListener listener = new LocalListener(IPAddress.Loopback, port, new ListenAndMove());
    private static int port = 9999;
    private MessageIntepretor interpretor;
    public float refreshRate = 1;
    private WaitForSeconds refreshWait;
    private MessageHandler myMessageHandler;

    private void Awake()
    {
        interpretor = new MessageIntepretor(GetComponent<Teleporter>());
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
        Debug.Log("Started listener");
        myMessageHandler = listener.MessageHandler();
        yield return UpdateDestination();
    }

    private IEnumerator UpdateDestination()
    {
        while (true)
        {
            yield return refreshWait;
            if (!myMessageHandler.MoreData())
            {
                Debug.Log("Message Not ready");
                continue;
            }
            interpretor.Receive(myMessageHandler.ReadAndParse());
        Debug.Log("listener Moved");
        }
    }

    private void OnDestroy()
    {
        listener.Stop();
    }
}