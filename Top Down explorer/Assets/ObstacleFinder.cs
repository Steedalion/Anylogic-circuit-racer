using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFinder : MonoBehaviour
{
    public Transform eye;
    public LayerMask lm;
private float rayDistance = 10;    

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit = new RaycastHit();
        bool didHit = Physics.Raycast(eye.position, eye.forward, out hit, rayDistance, lm);
        if (didHit)
        {
            Debug.Log("Hit an object:" + hit.collider.name);
        }
        Debug.DrawRay(eye.position, eye.forward*rayDistance, Color.red);
    }

    private RaycastHit? FireRay()
    {
        RaycastHit hit = new RaycastHit();
        bool didHit = Physics.Raycast(eye.position, eye.forward, out hit, rayDistance, lm);
        if (didHit) return hit;
     return null;
    }
    
    

    public int GetNumberOfObstacles()
    {
        return FireRay() != null ? 1 : 0;
    }
    
    
}
