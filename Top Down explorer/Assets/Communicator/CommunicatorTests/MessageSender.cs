using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Tests
{
    public class MessageSender
    {
        private TcpClient client;
        private StreamWriter writer;
        private Stream stream;

        public MessageSender(TcpClient client)
        {
            this.client = client;
            stream = client.GetStream();
            writer = new StreamWriter(stream);
        }

        public Task WriteLine(string msg)
        {
            Task t = writer.WriteLineAsync(msg);
            t.GetAwaiter().OnCompleted(() =>writer.Flush());
            return t;
        }  
        public Task Flush()
        {
            Task t = new Task(() =>writer.Flush());
            return t;
        } 
        
        public Task uWriteLine(string msg)
        {
            using (StreamWriter localWriter = new StreamWriter(stream))
            {
                return localWriter.WriteLineAsync(msg);
            }
        }
        public Task cWriteLine(string msg)
        {
            using (StreamWriter thWriter = new StreamWriter(stream))
            {
                return thWriter.WriteLineAsync(msg);
            }
        }


        public void CloseStreams()
        {
            writer.Dispose();
        }
    }
}