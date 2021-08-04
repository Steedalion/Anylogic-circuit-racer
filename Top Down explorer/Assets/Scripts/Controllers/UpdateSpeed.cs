using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class UpdateSpeed : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private float gain = 1;
    [SerializeField] private float smoothing = 0.1f;
    public UnityEvent<float> updateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.speed = CalculateNewSpeed();
        updateSpeed.Invoke(agent.speed);
    }

    private float CalculateNewSpeed()
    {
        float previousSpeed = agent.speed;
        float newspeed = agent.remainingDistance / gain;
        return previousSpeed + (newspeed - previousSpeed)*smoothing;
    }
}