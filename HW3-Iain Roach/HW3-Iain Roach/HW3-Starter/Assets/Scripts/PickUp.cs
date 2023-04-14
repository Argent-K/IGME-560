using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace BehaviorTree
{
    [Serializable]
    public class PickUp : Task
    {
        private Character pickuper;
        public Character Pickuper
        {
            get { return pickuper; }
            set { pickuper = value; }
        }

        private Thing pickupThis;
        public Thing PickupThis
        {
            get { return pickupThis; }
            set { pickupThis = value; }
        }

        public PickUp() : this(Character.None, Thing.None) { }

        public PickUp (Character pickuper, Thing pickupThis)
        {
            Pickuper = pickuper;
            PickupThis = pickupThis;
        }

        public override bool run(WorldState state)
        {
            if(state.ThingPosition[PickupThis] == state.CharacterPosition[Pickuper])
            {
                state.Has.Add(Pickuper, PickupThis);
                if (state.Debug) Debug.Log(this + " Success");
                Debug.Log(state.Has[Pickuper]);
                return true;
            }
            else
            {
                if (state.Debug) Debug.Log(this + " Fail");
                return false;
            }
            
        }

        public override string ToString()
        {
            return pickuper + " Picksup " + pickupThis;
        }

    }
}
