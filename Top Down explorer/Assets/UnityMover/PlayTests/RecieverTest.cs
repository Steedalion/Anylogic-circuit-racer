using System;
using NUnit.Framework;
using UnityEngine;
using UnityMover;

namespace Tests
{
    public class RecieverTest
    {
        private MessageIntepretor messageIntepretor;
        private string moveMessage = "Move:(1, 2, 3)";
        private string moveWithSpace = "Move : (1, 2, 3)";
        private Vector3 destination = new Vector3(1, 2, 3);
        private TeleportMover teleportMover;
        
        [SetUp]
        public void CreateNewReciever()
        {
            teleportMover = new TeleportMover();
            messageIntepretor = new MessageIntepretor(teleportMover);
        }

        [TearDown]
        public void RemoveReciever()
        {
            messageIntepretor = null;
        }

        [Test]
        public void RecieverNotNull()
        {
            Assert.IsNotNull(messageIntepretor);
        }

        [Test]
        public void RecieverMessageNotUnderstood()
        {
            Assert.Catch<MessageIntepretor.RecieverMessageNotUnderstood>(() => messageIntepretor.Receive("bogus message"));
        }

        [Test]
        public void MoveShouldNotThrowException()
        {
            messageIntepretor.Receive(moveMessage);
        }

        [Test]
        public void MoveWithSpaceShouldNotThrowException()
        {
            messageIntepretor.Receive(moveWithSpace);
        }


        [Test] public void InitialMoverPositionIsZero()
        {
            Assert.AreEqual(teleportMover.location, Vector3.zero);
        }
        
        [Test]
        public void ShouldMoveToPosition()
        {
            Moveable moveable = messageIntepretor.Receive(moveMessage);
            moveable.MoveTo();
            Assert.AreNotEqual(teleportMover.location, Vector3.zero,"Should have moved");
            Assert.AreEqual(teleportMover.location, destination, "Should be at destination"); 
        }

        [Test]
        public void StringEquals()
        {
            Assert.IsTrue("a".Equals("a"));
            Assert.IsTrue("a" == "a");
            Assert.IsFalse("a".Equals("b"));
            Assert.IsFalse("A".Equals("a"));
        }
        
    }

    internal class TeleportMover: Moveable
    {
        public Vector3 location = Vector3.zero;
        private Vector3 mydestination;
        public void SetNextDestination(Vector3 destination)
        {
            mydestination = destination;
        }

        public void MoveTo()
        {
            location = mydestination;
        }
    }
}