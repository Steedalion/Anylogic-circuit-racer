using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrowdController : MonoBehaviour
{
    private CrowdAgent[] agents;
    [SerializeField] private float radius = 5;
    private Vector3 destination;
    void Start()
    {
        destination = transform.position;
        agents = GetComponentsInChildren<CrowdAgent>();
    }


    public void SetCrowdCenterPoint(Vector3 center)
    {
        destination = center;
        foreach (CrowdAgent agent in agents)
        {
            agent.SetPosition(Distribute(center));
        }
    }

    private Vector3 Distribute(Vector3 center)
    {
        // return center;
        return new Vector3(center.x + Random.Range(-radius, radius), center.y, center.z + Random.Range(-radius, radius));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(destination, new Vector3(2*radius,1,2*radius));
    }
}
