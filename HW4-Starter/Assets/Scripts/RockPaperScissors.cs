using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

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
        public Dictionary<string, KeyDataRecord> data = new Dictionary<string, KeyDataRecord>();
        Hashtable hdata = new Hashtable();
        
        
        // The size of the window + 1.
        public int nValue;

        public NGramPredictor(int windowSize)
        {
            nValue = windowSize + 1;

            
        }


        // Register a set of actions with predictor, updating its data.
        // We assume actions have exactly nValue elements in it

        public void RegisterSequence(char[] actions)
        {
            char[] tempArr = new char[nValue - 1];
            //string tempArr = ArrToStrKey(ref actions); // assuming actions is char array
            for(int i = 0; i < actions.Length -1; i++)
            {
                tempArr[i] = actions[i];
            }
            string key = ArrToStrKey(ref tempArr);
            char val = actions[nValue - 1];

            if (!data.ContainsKey(key))
            {
                data[key] = new KeyDataRecord();
                
            }
            KeyDataRecord kdr = data[key];

            kdr.counts[val]++;
            kdr.total++;










            //// split the sequence into a key and value  key = prev moves value = current move
            //RPSMove[] key = new RPSMove[nValue - 1];
            //for(int i = 0; i < actions.Length - 1; i++)
            //{
            //    key[i] = actions[i];
            //}

            //RPSMove value = actions[nValue - 1]; // may need to - 1 this

            //KeyDataRecord keyData;
            //// Make sure we've got storage
            //if (!hdata.ContainsKey(key))
            //{
            //    // *base* keyData = data[key] = new KeyDataRecord();
            //    keyData = (KeyDataRecord<T>)(data[key] = new KeyDataRecord<T>());
            //}
            //else
            //{
            //    keyData = (KeyDataRecord)data[key];
            //}

            ////Add to the total, an to the count for the value
            //keyData.counts[value] += 1;
            //keyData.total += 1;

        }

        public char GetMostLikely(char[] actions)
        {
            string key = ArrToStrKey(ref actions);
            KeyDataRecord kdr = data[key];

            int highestVal = 0;
            char bestAction = RockPaperScissors.RandomMove();

            // get list of actions in the store


            foreach (KeyValuePair<char, int> kvp in kdr.counts)
            {
                if (kvp.Value > highestVal)
                {
                    bestAction = kvp.Key;
                    highestVal = kvp.Value;
                }
            }

            return bestAction;














            //// dictionary uses hashcodes need to write comparer
            //KeyDataRecord keyData = (KeyDataRecord)data[actions];


            //// Find the highest probability
            //int highestValue = 0;
            //RPSMove bestAction = RockPaperScissors.CharToMove(RockPaperScissors.RandomMove());

            //// Get the list of actions in the store
            //actions = new RPSMove[keyData.counts.Count];
            //int index = 0;
            //foreach (RPSMove key in keyData.counts.Keys)
            //{
            //    actions[index] = key;
            //    index++;
            //}

            //// go through each action
            //foreach (RPSMove action in actions)
            //{
            //    // check for the highest value
            //    if ((int)keyData.counts[action] > highestValue)
            //    {
            //        // store the action
            //        highestValue = (int)keyData.counts[action];
            //        bestAction = action;
            //    }
            //}

            //// We've looked through all actions, if best action is still null
            //// then its because we have no data on the given window.
            //// otherwise we have best action to take
            //return bestAction;
        }



        public static string ArrToStrKey(ref char[] actions)
        {
            StringBuilder builder = new StringBuilder();
            foreach(char a in actions)
            {
                builder.Append(a.ToString());
            }
            return builder.ToString();
        }

    }

    public class KeyDataRecord
    {
        // the counts for each successor action  Takes any key and returns int
        public Dictionary<char, int> counts = new Dictionary<char, int>()
        {
            { 'r', 0 },
            { 's', 0 },
            { 'p', 0}
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
