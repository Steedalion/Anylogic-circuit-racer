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
        private float[] tableEntries = new float[4];
        private NetworkStream stream;

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
            stream = client.GetStream();

            Transform t = transform;
            Vector3 position;
            while (true)
            {
                yield return new WaitForSeconds(0.5f);
                position = transform.position;
                tableEntries[0] = position.x;
                tableEntries[1] = position.y;
                tableEntries[2] = position.z;
                tableEntries[3] = t.rotation.eulerAngles.y;
                foreach (float tableEntry in tableEntries)
                {
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(tableEntry.ToString() + " ");
                    stream.Write(msg, 0, msg.Length);
                }

                byte[] endline = System.Text.Encoding.ASCII.GetBytes("\n");
                stream.Write(endline, 0, endline.Length);
                stream.Flush();
                // client.Close();
            }
        }
    }
}