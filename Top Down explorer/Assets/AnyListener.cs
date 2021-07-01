using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(ObstacleFinder))]
    public class AnyListener : MonoBehaviour
    {
        private TcpListener listener;
        private TcpClient client;
        private float[] tableEntries = new float[14];
        private NetworkStream stream;
        private ObstacleFinder finder;

        private void Start()
        {
            finder = GetComponent<ObstacleFinder>();
            IPAddress address = IPAddress.Any;
            int port  = 9999;
            listener = new TcpListener(address, port);
            listener.Start();
            Debug.Log("Waiting for client to connect @"+address+" : "+port);
            client = listener.AcceptTcpClient();
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

                Vector3[] obstaclePositions = finder.GetNearestObstacles(3);
                Debug.Log(obstaclePositions[0]);
                
                
                tableEntries[0] = position.x;
                tableEntries[1] = position.y;
                tableEntries[2] = position.z;
                tableEntries[3] = t.rotation.eulerAngles.y;
                tableEntries[4] = finder.GetNumberOfObstacles();
                tableEntries[5] = obstaclePositions[0].x;
                tableEntries[6] = obstaclePositions[0].y;
                tableEntries[7] = obstaclePositions[0].z;
                tableEntries[8] = obstaclePositions[1].x;
                tableEntries[9] = obstaclePositions[1].y;
                tableEntries[10] = obstaclePositions[1].z;
                tableEntries[11] = obstaclePositions[2].x;
                tableEntries[12] = obstaclePositions[2].y;
                tableEntries[13] = obstaclePositions[2].z;
                
                foreach (float tableEntry in tableEntries)
                {
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(tableEntry.ToString() + " ");
                    stream.Write(msg, 0, msg.Length);
                }

                byte[] endline = System.Text.Encoding.ASCII.GetBytes("\n");
                stream.Write(endline, 0, endline.Length);
                stream.Flush();
            }
        }
    }
}