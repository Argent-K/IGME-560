using System;
using UnityEngine;

namespace BehaviorTree
{
    [Serializable]
    public class IsOpen : Task
    {
        // We will determine whether this thing is open.
        private Thing what;
        public Thing What
        {
            get { return what; }
            set { what = value; }
        }

        // This constructor defaults the thing to None.
        public IsOpen() : this(Thing.None) { }

        // This constructor takes a parameter for the thing.
        public IsOpen (Thing what)
        {
            What = what;
        }

        // This method runs the IsOpen condition on the given WorldState object.
        // Must return true or false based on wheter an enum item of type Thing stored in the What public property
        // is open in the given WorldState Object state, which is passed in as a parameter of the run method.
        // You can use the Open dictionary, which maps a Thing to a Boolean Value, to determine wheter What is open
        // in the state.
        public override bool run (WorldState state)
        {
            // Fill in your condition check here:
            //state.Open<thing, bool>
            if(state.Open.ContainsKey(What))
            {
                // the world state contains this object
                if(state.Open[What] == true)
                {
                    // What is open
                    if (state.Debug) Debug.Log(this + " is open? Yes.");
                    return true;
                }
                else
                {
                    if (state.Debug) Debug.Log(this + " is open? No.");
                    return false;
                }

            } 
            
            if (state.Debug) Debug.Log(this + " Doesn't exists within the worldstate.");
            return false;
            



            
        }

        // Creates and returns a string describing the IsOpen condition.
        public override string ToString()
        {
            return "Is " + what + " open?";
        }
    }
}
