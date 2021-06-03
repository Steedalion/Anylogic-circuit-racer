using UnityEngine;
using UnityEngine.AI;

namespace UnityMover
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavigationReciever : MonoBehaviour, Moveable
    {
        public NavMeshAgent agent;
        private Vector3 myDestination;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        public void SetDestination(Vector3 destination)
        {
            myDestination = destination;
        }

        public void MoveTo()
        {
            agent.SetDestination(myDestination);
        }
    }
}