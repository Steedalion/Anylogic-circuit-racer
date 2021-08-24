using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace UnityMover
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Navigator : MonoBehaviour, Moveable
    {
        public NavMeshAgent agent { get; private set; }
        private Vector3 myDestination;
        public UnityEvent<Vector3> onNextDestinationSet;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        public void SetNextDestination(Vector3 destination)
        {
            myDestination = destination;
        }

        public void MoveTo()
        {
            onNextDestinationSet?.Invoke(myDestination);
            agent.SetDestination(myDestination);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(transform.position, myDestination);
        }
    }
}