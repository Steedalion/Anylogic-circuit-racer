using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMover;

public class RecieveAndMove : MonoBehaviour
{
    public MessageIntepretor MessageIntepretor { get; private set; }

    [SerializeField]
    public Moveable moveable;
    // Start is called before the first frame update
    void Start()
    {
        moveable = FindObjectOfType<Navigator>();
        MessageIntepretor = new MessageIntepretor(moveable);
    }
}
