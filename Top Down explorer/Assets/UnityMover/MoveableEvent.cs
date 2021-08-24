using UnityEngine;
using UnityEngine.Events;

namespace UnityMover
{
    public class MoveableEvent : MonoBehaviour, Moveable
    {
        [SerializeField] private UnityEvent<Vector3> onSetDestination;
        private Vector3 currentDestination;

        public void SetNextDestination(Vector3 destination)
        {
            currentDestination = destination;
        }


        public void MoveTo()
        {
            onSetDestination?.Invoke(currentDestination);
        }
    }
}