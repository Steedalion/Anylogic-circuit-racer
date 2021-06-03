using System;
using UnityEngine;

namespace UnityMover
{
    public class Receiver
    {
        private Moveable moveable;

        public Receiver(Moveable moveable)
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
                moveable.SetDestination(destination);
                return moveable;
            }
            else
            {
                throw new RecieverMessageNotUnderstood();
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
        }
    }

    public interface Moveable
    {
        void SetDestination(Vector3 destination);
        void MoveTo();
    }

    
}