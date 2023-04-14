using System;
using UnityEngine;

namespace BehaviorTree
{
    [Serializable]
    public class IsHere : Task
    {
        // Determine if thing is at Character Location
        private Thing what;
        public Thing What
        {
            get { return what; }
            set { what = value; }
        }

        private Character grabber;
        public Character Grabber
        {
            get { return grabber; }
            set { grabber = value; }
        }


        // Constructor blah
        public IsHere() : this(Thing.None) { }

        // More Constructor
        public IsHere (Thing what)
        {
            What = what;
        }

        public override bool run(WorldState state)
        {
            if(state.CharacterPosition[Grabber] == state.ThingPosition[What])
            {
                if (state.Debug) Debug.Log(this + " Success");
                return true;
            } else
            {
                if (state.Debug) Debug.Log(this + " Fail");
                return false;
            }

            
        }

        public override string ToString()
        {
            return "Is " + what + " here?";
        }



    }
}