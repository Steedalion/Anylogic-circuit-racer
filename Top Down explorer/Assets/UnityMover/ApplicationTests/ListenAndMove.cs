using UnityEngine;

internal class ListenAndMove : MessageHandler
{
    public override string ReadAndParse()
    {
        if (myStream.Peek() < -1)
        {
            return null;
        }

        string msg = myStream.ReadLine();
        Debug.Log( "Reading message:" +msg);
        return msg;
    }
}