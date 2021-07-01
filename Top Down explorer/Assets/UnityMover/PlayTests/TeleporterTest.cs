using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityMover;

namespace Tests
{
    public class TeleporterTest
    {
        private GameObject gameObject;
        private Teleporter teleporter;
        private MessageIntepretor messageIntepretor;
        [UnitySetUp]
        public IEnumerator CreateEnvironment()
        {
            gameObject = GameObject.Instantiate(new GameObject());
            teleporter = gameObject.AddComponent<Teleporter>();
            messageIntepretor = new MessageIntepretor(teleporter);
            
            yield return null;
        }

        [UnityTearDown]
        public IEnumerator TearDownEnvironment()
        {
            GameObject.Destroy(gameObject);
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator CreatedObjectsNotNull()
        {
            Assert.NotNull(gameObject);
            Assert.NotNull(teleporter);
            yield return null;
        }

        [UnityTest]
        public IEnumerator MoveAGameobjectToDestination()
        {
            
            string moveToDestination = "Move:(1,2,3)";
            Vector3 destination = new Vector3(1, 2, 3);
            Moveable moveable = messageIntepretor.Receive(moveToDestination);
            Assert.AreEqual(gameObject.transform.position, destination);
            yield return null;
            moveable.MoveTo();
            yield return null;
            
            Assert.AreEqual(gameObject.transform.position, destination);
        }
    }
}