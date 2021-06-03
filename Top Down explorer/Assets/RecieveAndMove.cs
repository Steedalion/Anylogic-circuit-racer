using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMover;

public class RecieveAndMove : MonoBehaviour
{
    public Receiver receiver { get; private set; }

    [SerializeField]
    public Moveable moveable;
    // Start is called before the first frame update
    void Start()
    {
        moveable = FindObjectOfType<Navigator>();
        receiver = new Receiver(moveable);
    }
}
