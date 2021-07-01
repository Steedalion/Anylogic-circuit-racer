using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;
using UnityMover;

namespace Tests
{
    public class NavigatorTests
    {
        private GameObject gameObject;
        private Navigator navigator;
        private MessageIntepretor messageIntepretor;

        [UnitySetUp]
        public IEnumerator SetupResources()
        {
            gameObject = GameObject.Instantiate(new GameObject());
            navigator = gameObject.AddComponent<Navigator>();
            messageIntepretor = new MessageIntepretor(navigator);
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


        // [UnityTest]
        public IEnumerator MoveSetsNavmeshDestination()
        {
            NavMeshAgent agent = gameObject.GetComponent<NavMeshAgent>();
            yield return null;
            string moveCommand = "Move: (1,2,3)";
            Moveable moveable = messageIntepretor.Receive(moveCommand);
            Vector3 destination = new Vector3(1, 2, 3);
            Assert.AreNotEqual(agent.destination, destination);
            yield return null;
            moveable.MoveTo();
            yield return null;
            Assert.AreEqual(agent.destination, destination);
        }
    }
}