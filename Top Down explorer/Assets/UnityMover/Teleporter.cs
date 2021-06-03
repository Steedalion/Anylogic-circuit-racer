using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMover;

public class Teleporter : MonoBehaviour, Moveable
{
    private Vector3 myDestination;

    public void SetDestination(Vector3 destination)
    {
         myDestination = destination;
    }

    public void MoveTo()
    {
        transform.position = myDestination;
    }
}
