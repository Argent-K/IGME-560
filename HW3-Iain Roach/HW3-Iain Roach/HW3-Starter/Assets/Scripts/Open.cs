using System;
using UnityEngine;

namespace BehaviorTree
{
    [Serializable]
    public class Open : Task
    {
        // The character that is opening the thing.
        private Character opener;
        public Character Opener
        {
            get { return opener; }
            set { opener = value; }
        }

        // The thing that is being opened.
        private Thing openThis;
        public Thing OpenThis
        {
            get { return openThis; }
            set { openThis = value; }
        }

        // This constructor defaults the character and thing to None.
        public Open() : this(Character.None, Thing.None) { }

        // This constructor takes parameters for the character and thing.
        public Open (Character opener, Thing openThis)
        {
            Opener = opener;
            OpenThis = openThis;
        }

        // This method runs the Open action on the given WorldState object.
        public override bool run(WorldState state)
        {
            // Fill in your conditional logic here:
            // In order for action to succeed two things must be true
            // Opener and OpenThis must be at the same location
            // OpenThis must be closed

            //Opener and OpenThis are at the same location
            // CharacterPosition, ThingPosition and Open dictionary
            if(state.CharacterPosition[Opener] == state.ThingPosition[OpenThis])
            {
                //OpenThis is closed
                if(!state.Open[OpenThis])
                {
                    // In state open dictionary updated so that OpenThis is true
                    state.Open[OpenThis] = true;
                    if (state.Debug) Debug.Log(this + " Success");
                    return true;
                }
            }
            if (state.Debug) Debug.Log(this + " Fail");
            return false;
        }

        // Creates and returns a string describing the Open action.
        public override string ToString()
        {
            return opener + " opens " + openThis;
        }
    }
}
