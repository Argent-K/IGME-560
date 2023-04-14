using System;
using UnityEngine;

namespace BehaviorTree
{
    [Serializable]
    public class MoveTo : Task
    {
        // The character that is moving to a different location.
        private Character mover;
        public Character Mover
        {
            get { return mover; }
            set { mover = value; }
        }

        // The location the character is moving to.
        private Location where;
        public Location Where
        {
            get { return where; }
            set { where = value; }
        }

        // This constructor defaults the character and location to None.
        public MoveTo() : this(Character.None, Location.None) { }

        // This constructor takes parameters for the character and location.
        public MoveTo (Character mover, Location where)
        {
            Mover = mover;
            Where = where;
        }

        // This method runs the MoveTo action on the given WorldState object.
        public override bool run (WorldState state)
        {
            // Fill in your conditional logic here:

            // 1) Where is not equal to the Mover's current location

            // 2) There is a connection between the Mover's current location and Where

            // 2a) There is a door between the Mover's current location and Where
            //      i. The door is open, then success.
            //      ii. The door is closed, then failure
            // 2b)  There is no door,then success

            // Conditions 1 and two must be true.  If either is false, the action fails
            // If 1 and 2 are true -> check wheter a door exists. If no door then the action succeeds. 
            // If there is a door, it must be open. if it is open then success. Otherwise failure.
            // Use the Open, CharacterPosition, ConnectedLocations, and BetweenLocations dictionaries to check these conditions
            // If the action succeeds the CharacterDictionary should be updated so that the Mover is at Where.

            // Check one
            if(!(state.CharacterPosition[Mover] == Where))
            {
                // Check 2) if there is a connection between the Mover's current location and Where
                if(state.ConnectedLocations[Where].Contains(state.CharacterPosition[Mover]))
                {
                    


                    // Check if door exists // Use betweenLocations Checks if location on other side of door is Where
                    // Between locations only has location data for entrance and hallway
                    // check if BetweenLocations contians charPos
                    // if(state.BetweenLocations.ContainsKey(state.CharacterPosition[Mover]))
                    if(state.BetweenLocations.ContainsKey(state.CharacterPosition[Mover]) && state.BetweenLocations[state.CharacterPosition[Mover]].Item2 == Where)
                    {
                        if(state.Open[state.BetweenLocations[state.CharacterPosition[Mover]].Item1])
                        {
                            // Door is open return success
                            // Update char pos
                            state.CharacterPosition[Mover] = Where;
                            if (state.Debug) Debug.Log(this + " Success");
                            return true;
                        }
                        else
                        {
                            if (state.Debug) Debug.Log(this + " Fail");
                            return false;
                        }
                    }
                    else
                    {
                        // There is no door
                        // Update char pos
                        state.CharacterPosition[Mover] = Where;
                        if (state.Debug) Debug.Log(this + " Success");
                        return true;
                    }




                }
            }


            if (state.Debug) Debug.Log(this + " Success");
            return true;

            if (state.Debug) Debug.Log(this + " Fail");
            return false;
        }

        // Creates and returns a string describing the MoveTo action.
        public override string ToString()
        {
            return mover + " moves to " + where;
        }
    }
}
