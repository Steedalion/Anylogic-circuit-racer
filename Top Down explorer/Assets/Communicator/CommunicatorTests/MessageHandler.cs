using System.Collections.Generic;
using System.IO;

public abstract class MessageHandler
{
    protected StreamReader myStream;

    public void SetStream(StreamReader NetworkStream)
    {
        myStream = NetworkStream;
    }

    public abstract string ReadAndParse();

    public bool MoreData()
    {
        return myStream.Peek()>-1;
    }
}