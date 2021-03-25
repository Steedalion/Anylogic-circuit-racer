using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace DefaultNamespace
{
    public class AnyListener : MonoBehaviour
    {
        private TcpListener server;
        private TcpClient client;

        private void Start()
        {
            IPAddress address = IPAddress.Any;
            server = new TcpListener(address, 9999);
            server.Start();

            client = server.AcceptTcpClient();
            StartCoroutine(SendCords());
        }

        private IEnumerator SendCords()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f);
                NetworkStream stream = client.GetStream();
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(transform.position.ToString() + "\n");
                stream.Write(msg, 0, msg.Length);
                stream.Flush();
                // client.Close();
                Debug.Log("Sending "+transform.position);
                
            }
        }
    }
}