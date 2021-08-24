using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UpdateSpeed : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private float gain = 0.75f;
    [SerializeField] private float smoothing = 0.05f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        agent.speed = CalculateNewSpeed();
    }

    private float CalculateNewSpeed()
    {
        float previousSpeed = agent.speed;
        float newspeed = agent.remainingDistance / gain;
        return previousSpeed + (newspeed - previousSpeed)*smoothing;
    }
}