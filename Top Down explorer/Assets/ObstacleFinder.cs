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

    public Vector3[] GetNearestObstacles(int size)
    {
        Collider[] obstacles = Physics.OverlapSphere(gameObject.transform.position, rayDistance, lm);
        return NearestObstacles(obstacles, 3);
    }
    

    public int GetNumberOfObstacles()
    {
        Collider[] obstacles = Physics.OverlapSphere(gameObject.transform.position, rayDistance, lm);
        return obstacles.Length;
    }

    private Vector3[] NearestObstacles(Collider[] obstacles, int size)
    {
        Vector3[] positions = new Vector3[size];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = new Vector3();
        }
        
        List<Collider> colliderList = new List<Collider>(obstacles);

        for (int i = 0; i < size; i++)
        {
            Collider nearest = GetNearest(colliderList);
            if(nearest == null) return positions;
            positions[i] = nearest.transform.position;
            colliderList.Remove(nearest);
        }

        return positions;
    }

    private Collider GetNearest(List<Collider> colliderList)
    {
        float bestDistance = float.PositiveInfinity;
        Collider currentBest = null;

        foreach (Collider collider1 in colliderList)
        {
            float thisDistance = Vector3.Distance(collider1.transform.position, gameObject.transform.position);
            if (thisDistance< bestDistance)
            {
                bestDistance = thisDistance;
                currentBest = collider1;
            }
        }
        return currentBest;
    }
}
