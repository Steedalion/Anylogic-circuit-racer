using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ConnectToAnyLogic : MonoBehaviour
{
            Socket listener;
         Socket clientSocket;

    // Start is called before the first frame update
    void Start()
    {
        // IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
        // IPAddress ipAddress = ipHostEntry.AddressList[0];
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 9999);
        
        listener = new Socket( SocketType.Stream, ProtocolType.Tcp);

        listener.Bind(ipEndPoint);
         listener.Listen(10);
         Debug.Log("Waiting connection ... ");
         
         clientSocket = listener.Accept();
         Debug.Log("Connected ");
         byte[] messageSent = Encoding.ASCII.GetBytes(transform.position.ToString());
         Debug.Log("Sending "+transform.position.ToString());
         clientSocket.Send(messageSent);
          
    }

    // Update is called once per frame
    void Update()
    {
        if(clientSocket.Connected) return;
        clientSocket = listener.Accept();
        byte[] messageSent = Encoding.ASCII.GetBytes(transform.position+"<EOF>");
        Debug.Log("Sending "+transform.position.ToString());
        clientSocket.Send(messageSent);
    }
}
