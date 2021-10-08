using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class UpdateSpeed : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private float gain = 0.75f;
    [SerializeField][Range(0,1)] private float smoothing = 0.05f;
    public UnityEvent onUpdate;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        agent.speed = CalculateNewSpeed();
        onUpdate?.Invoke();
    }

    private float CalculateNewSpeed()
    {
        float previousSpeed = agent.speed;
        float newspeed = agent.remainingDistance * gain;
        return newspeed*(1-smoothing)  + previousSpeed*smoothing;
    }
}