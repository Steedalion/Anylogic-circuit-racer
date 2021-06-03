using UnityEngine;
using UnityMover;

public class SenderMessage : MonoBehaviour
{
    public string message = "Move (1,2,3)";

    public RecieveAndMove recieveAndMove;

    [ContextMenu(nameof(Send))]
    public void Send()
    {
        Moveable command = recieveAndMove.receiver.Receive(message);
        command.MoveTo();
    }
}
