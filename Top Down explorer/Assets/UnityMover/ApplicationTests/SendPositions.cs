using System.IO;

public class SendPositions : AnySyncMock.ClientHandler
{
    public override void SendOne()
    {
        StreamWriter writer = new StreamWriter(stream);
        writer.WriteLine("Move : (1,1,1)");
        writer.WriteLine("Move : (1,2,1)");
        writer.WriteLine("Move : (1,3,1)");
        writer.Flush();
    }
    //   public virtual void SendOne()
    // {
    //      byte[] word = Encoding.ASCII.GetBytes("Hello \n");
    //     Debug.Log("Send hello");
    //     int length = word.Length;
    //     stream.Write(word, 0, length);
    // }
}