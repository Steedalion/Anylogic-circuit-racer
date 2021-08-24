using System;
using UnityEngine;

namespace UnityMover
{
    [Serializable]
    public class MessageIntepretor
    {
        private Moveable moveable;

        public MessageIntepretor(Moveable moveable)
        {
            this.moveable = moveable;
        }

        public Moveable Receive(string instruction)
        {
            return GetCommand(instruction);
        }

        private Moveable GetCommand(string instruction)
        {
            string[] split = instruction.Replace(" ", "").Trim(' ').Split(':');
            if (split[0] == "Move")
            {
                string coordinates = split[1];

                Vector3 destination = ToVector3(coordinates);
                moveable.SetNextDestination(destination);
                moveable.MoveTo();
                Debug.Log("Moving too: " + destination + " with" + moveable);
                return moveable;
            }
            else
            {
                throw new RecieverMessageNotUnderstood(instruction);
            }
        }

        private static Vector3 ToVector3(string coordinates)
        {
            string[] coords = coordinates.Trim('(').Trim(')').Split(',');

            Vector3 output = new Vector3(float.Parse(coords[0]), float.Parse(coords[1]), float.Parse(coords[2]));
            return output;
        }

        public class RecieverMessageNotUnderstood : Exception 
        {
            public RecieverMessageNotUnderstood(string message) : base(message)
            {
                Debug.Log("Message could not be parsed:"+message);
            }
        }
    }

    public interface Moveable
    {
        void SetNextDestination(Vector3 destination);
        void MoveTo();
    }
}