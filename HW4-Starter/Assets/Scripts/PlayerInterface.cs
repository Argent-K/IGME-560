using UnityEngine;
using RPS;
using System.Collections.Generic;

/// <summary>
/// This class receives input from the player, queries the AI for predictions, and updates the total wins/losses.
/// </summary>
public class PlayerInterface : MonoBehaviour
{
    // Records the input from the user as a char: 'r', 'p', or 's'
    private char input = '0';

    // Records the number of times the player has won.
    private int playerWins = 0;

    // Records the number of times the AI has won.
    private int aiWins = 0;

    // Records the history of player inputs
    static int windowSize = 2;
    NGramPredictor nGram = new NGramPredictor(windowSize);
    List<RPSMove> hist = new List<RPSMove>();


    // Update is called once per frame
    void Update()
    {
        // Check if the mouse button was pressed.
        if (Input.GetMouseButtonDown(0))
        {
            // Grab the position that was clicked by the mouse.
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            // Use a raycast to determine whether a tile was clicked.
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            // If a tile with a collider was clicked...
            if (hit.collider != null)
            {
                // Begin an output string we will print to the log.
                string output = "You selected: " + hit.collider.gameObject.name;

                // Convert the collider clicked by the user to the r/p/s character in the input variable.
                if (hit.collider.gameObject.name == "Rock")
                {
                    input = 'r';
                }
                else if (hit.collider.gameObject.name == "Paper")
                {
                    input = 'p';
                }
                else if (hit.collider.gameObject.name == "Scissors")
                {
                    input = 's';
                }
                else return;

                
                char predicted = 'r';
                // Add inputs to a history array then register
                
                hist.Add(RockPaperScissors.CharToMove(input));
                if(hist.Count <= windowSize) // Count is total num of objects in hist
                {
                    // Not enough data in window just predict randomly
                    predicted = RockPaperScissors.RandomMove();
                    
                }
                else
                {
                    // at one more than window size => Register Sequence
                    // and set predicted to best move
                    
                    nGram.RegisterSequence(hist.ToArray());



                    Debug.Log("nGram.data keys");
                    foreach(KeyValuePair<RPSMove[], KeyDataRecord> kvp in nGram.data)
                    {
                        foreach(RPSMove rps in kvp.Key)
                        {
                            
                            Debug.Log(rps);
                            Debug.Log(kvp.Key.Length);
                        }
                    }
                    predicted = RockPaperScissors.MoveToChar(nGram.GetMostLikely(hist.GetRange(0,hist.Count - 1).ToArray())); // May need to only send all but last index


                    // Remove earliest move played
                    hist.RemoveAt(0);
                }




                Debug.Log(hist.Count);
                foreach(KeyValuePair<RPSMove[], KeyDataRecord> kvp in nGram.data)
                {
                    Debug.Log(kvp.Key);
                    Debug.Log(kvp.Value);
                }
                




                // Ask the ngram AI to predict what the player will choose..
                // You will need to implement this code and any history tracking it requires.
                // For now, we will predict a move at random.
                
                //List<char> historyList = new List<char>();
                //historyList.Add(input);
                //RPSMove[] windMoves = new RPSMove[nGram.nValue];
                //int index = 0;
                //foreach(char c in historyList.GetRange(historyList.Count - nGram.nValue + 1, nGram.nValue).ToArray())
                //{
                //    windMoves[index] = RockPaperScissors.CharToMove(c);
                //    index++;
                //}
                //// Need history of player moves
                //nGram.RegisterSequence(windMoves);
                //char predicted = RockPaperScissors.MoveToChar(nGram.GetMostLikely(windMoves));
                RPSMove predMove = RockPaperScissors.CharToMove(predicted);
                output += "\nThe NGram AI predicts you will play: " + predMove;

                // Given the predicted user move, choose the move that will win against it.
                RPSMove aiMove = RockPaperScissors.GetWinner(predMove);
                output += "\nThe NGram AI plays: " + aiMove;

                // Get the result of playing the user and AI moves.
                int result = RockPaperScissors.Play(RockPaperScissors.CharToMove(input), aiMove);

                // If the result is 1, the player wins.
                if (result > 0)
                {
                    output += "\nYou win!";
                    playerWins++;
                }
                // If the result is -1, the AI wins.
                else if (result < 0)
                {
                    output += "\nYou lose...";
                    aiWins++;
                }
                // If the result is 0, there is a tie.
                else output += "\nTie";
                
                // Print the total wins to the log.
                output += "\nPlayer Wins: " + playerWins;
                output += "\nAI Wins: " + aiWins;

                // Output the combined output string to the log.
                Debug.Log(output);
            }
        }
    }
}
