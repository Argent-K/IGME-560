using System;
using System.Collections;
using System.Collections.Generic;

namespace RPS
{
    /// <summary>
    /// This is a C# class that provides helper methods for playing Rock Paper Scissors.
    /// </summary>
    public static class RockPaperScissors
    {
        // Given a character r/p/s, returns a corresponding RPSMove enum.
        public static RPSMove CharToMove (char c)
        {
            if (c == 'r') return RPSMove.Rock;
            else if (c == 'p') return RPSMove.Paper;
            else return RPSMove.Scissors;
        }

        public static char MoveToChar (RPSMove move)
        {
            if (move == RPSMove.Rock) return 'r';
            else if (move == RPSMove.Paper) return 'p';
            else return 's';
        }

        

        // Given two RPSMoves, returns the winner.
        public static int Play (RPSMove player1, RPSMove player2)
        {
            if (player2 == GetWinner(player1)) return -1;
            else if (player1 == GetWinner(player2)) return 1;
            else return 0;
        }

        // Given a RPSMove, returns the move that would win against it.
        public static RPSMove GetWinner (RPSMove original)
        {
            if (original == RPSMove.Rock) return RPSMove.Paper;
            else if (original == RPSMove.Paper) return RPSMove.Scissors;
            else return RPSMove.Rock;
        }

        // Returns a random r/p/s move.
        public static char RandomMove()
        {
            Random rand = new Random();
            int roll = rand.Next(0,3);
            if (roll == 0) return 'r';
            else if (roll == 1) return 'p';
            else return 's';
        }
    }


    public class NGramPredictor
    {
        // The frequency data : takes an array and returns a KeyDataRecord
        //Hashtable data = new Hashtable();
        public Dictionary<RPSMove[], KeyDataRecord> data = new Dictionary<RPSMove[], KeyDataRecord>();
        Hashtable hdata = new Hashtable();
        
        
        // The size of the window + 1.
        public int nValue;

        public NGramPredictor(int windowSize)
        {
            nValue = windowSize + 1;
            
        }


        // Register a set of actions with predictor, updating its data.
        // We assume actions have exactly nValue elements in it

        public void RegisterSequence(RPSMove[] actions)
        {
            // split the sequence into a key and value  key = prev moves value = current move
            RPSMove[] key = new RPSMove[nValue - 1];
            for(int i = 0; i < actions.Length - 1; i++)
            {
                key[i] = actions[i];
            }

            RPSMove value = actions[nValue - 1]; // may need to - 1 this

            KeyDataRecord keyData;
            // Make sure we've got storage
            if (!hdata.ContainsKey(key))
            {
                // *base* keyData = data[key] = new KeyDataRecord();
                keyData = (KeyDataRecord)(hdata[key] = new KeyDataRecord());
            }
            else
            {
                keyData = (KeyDataRecord)hdata[key];
            }

            //Add to the total, an to the count for the value
            keyData.counts[value] += 1;
            keyData.total += 1;

        }

        public RPSMove GetMostLikely(RPSMove[] actions)
        {
            

            // Get the key data
            // *base *keyData = data[actions];

            //for(int i = 0; i < actions.Length; i++)
            //{
            //    UnityEngine.Debug.Log(actions[i]);
            //}

            //UnityEngine.Debug.Log(data.Keys);
            //UnityEngine.Debug.Log(actions);

            // dictionary uses hashcodes need to write comparer
            KeyDataRecord keyData = (KeyDataRecord)hdata[actions];


            // Find the highest probability
            int highestValue = 0;
            RPSMove bestAction = RockPaperScissors.CharToMove(RockPaperScissors.RandomMove());

            // Get the list of actions in the store
            actions = new RPSMove[keyData.hcounts.Count];
            int index = 0;
            foreach (RPSMove key in keyData.hcounts.Keys)
            {
                actions[index] = key;
                index++;
            }

            // go through each action
            foreach (RPSMove action in actions)
            {
                // check for the highest value
                if ((int)keyData.hcounts[action] > highestValue)
                {
                    // store the action
                    highestValue = (int)keyData.hcounts[action];
                    bestAction = action;
                }
            }

            // We've looked through all actions, if best action is still null
            // then its because we have no data on the given window.
            // otherwise we have best action to take
            return bestAction;
        }

    }

    public class KeyDataRecord
    {
        // the counts for each successor action  Takes any key and returns int
        public Dictionary<RPSMove, int> counts = new Dictionary<RPSMove, int>()
        {
            { RPSMove.Rock, 0 },
            { RPSMove.Scissors, 0 },
            { RPSMove.Paper, 0}
        };

        public Hashtable hcounts = new Hashtable()
        {
            {RPSMove.Rock, 0 },
            {RPSMove.Scissors, 0 },
            {RPSMove.Paper, 0 }
        };
       
        
            
        // The number of times the window has been used
        public int total;

    }

    // Enumeration of possible RPS moves.
    public enum RPSMove
    {
        Rock = 0, Paper = 1, Scissors = 2
    }
}
