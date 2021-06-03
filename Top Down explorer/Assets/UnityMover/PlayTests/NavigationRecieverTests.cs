using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;
using UnityMover;

namespace Tests
{
    public class NavigationRecieverTests
    {
        private GameObject gameObject;
        private NavigationReciever navigationReciever;
        private Receiver receiver;

        [UnitySetUp]
        public IEnumerator SetupResources()
        {
            gameObject = GameObject.Instantiate(new GameObject());
            navigationReciever = gameObject.AddComponent<NavigationReciever>();
            receiver = new Receiver(navigationReciever);
            yield return null;
        }

        [UnityTearDown]
        public IEnumerator TeardownEnvironment()
        {
            GameObject.Destroy(gameObject);
            yield return null;
        }
        

        [UnityTest]
        public IEnumerator AutomaticallyAddsNavmeshAgent()
        {
            NavMeshAgent agent = gameObject.GetComponent<NavMeshAgent>();
            Assert.IsNotNull(agent);
            yield return null;
        }
        
        
        [UnityTest]
        public IEnumerator MoveSetsNavmeshDestination()
        {
            NavMeshAgent agent = gameObject.GetComponent<NavMeshAgent>();

            string moveCommand = "Move: (1,2,3)";
            Moveable moveable = receiver.Receive(moveCommand);
            Vector3 destination = new Vector3(1,2,3);
            Assert.AreNotEqual(agent.destination, destination);
            yield return null;
            moveable.MoveTo();
            yield return null;
            Assert.AreEqual(agent.destination, destination);
        }
    }
}