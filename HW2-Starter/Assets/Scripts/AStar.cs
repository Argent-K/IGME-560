using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Performs search using A*.
/// </summary>
public class AStar : MonoBehaviour
{
    // Colors for the different search categories.
    public static Color openColor = Color.cyan;
    public static Color closedColor = Color.blue;
    public static Color activeColor = Color.yellow;
    public static Color pathColor = Color.yellow;

    // The stopwatch for timing search.
    private static Stopwatch watch = new Stopwatch();

    public static IEnumerator search(GameObject start, GameObject end, Heuristic heuristic, float waitTime, bool colorTiles = false, bool displayCosts = false, Stack<NodeRecord> path = null)
    {
        // Starts the stopwatch.
        watch.Start();

        // Add your A* code here.
        // Initialize the record for the start node
        NodeRecord startRecord = new NodeRecord();
        startRecord.Tile = start;
        startRecord.connection = null;
        startRecord.costSoFar = 0;
        startRecord.estimatedTotalCost = heuristic(start,startRecord.Tile,end);

        // retrieves the number used to scale the game world tiles
        // Your c# code should look similar but maybe not exact...
        float scale = startRecord.Tile.transform.localScale.x;

        // Initialize the open and closed lists
        List<NodeRecord> open = new List<NodeRecord>();
        open.Add(startRecord);
        List<NodeRecord> closed = new List<NodeRecord>();

        NodeRecord current = new NodeRecord();
        NodeRecord endNodeRecord = new NodeRecord();
        float endNodeHeuristic = 0.0f;

        //Iterate through processing each node
        while (open.Count > 0)
        {
            // Find the smallest element in the open list
            // Use estimatedTotalCost, not cost so far
            current = SmallestHeuristicElement(open);

            if (colorTiles)
            {
                current.ColorTile(activeColor);
            }

            // Pause the animation to show the new active tile
            // Actual C# command to use
            yield return new WaitForSeconds(waitTime);

            // If it is the goal node, then terminate
            if (current.Tile == end)
            {
                break;
            }

            // Otherwise get its outgoing connections
            List<Connection> connections = new List<Connection>();

            foreach (GameObject obj in current.Tile.GetComponent<Node>().Connections.Values)
            {
                Connection testCon = new Connection();
                testCon.Cost = 1.0f;
                testCon.toNode = obj;
                testCon.fromNode = current.Tile;
                connections.Add(testCon);
            }

            // Loop through each connection in turn
            foreach (Connection con in connections)
            {
                // Get the cost estimate for the end node
                // The cost of each connection is equal to the scale
                GameObject endNode = con.getToNode();
                float endNodeCost = current.costSoFar + scale;

                //If the node is closed we may have to skip
                // Or remove from the closed list
                bool inClosed = false;
                endNodeRecord = new NodeRecord();
                foreach(NodeRecord cl in closed)
                {
                    if(cl.Tile.Equals(endNode))
                    {
                        inClosed = true;

                        // Here we find the record in the closed list corresponding to the endNode
                        endNodeRecord = cl;
                        break;
                    }
                }

                // Check if the node is in the open list
                bool inOpen = false;
                foreach (NodeRecord ol in open)
                {
                    if (ol.Tile.Equals(endNode))
                    {
                        inOpen = true;

                        // Here we find the record in the open list corresponding to the endnode
                        endNodeRecord = ol;
                        break;
                    }
                }

                // We found it in the closed list
                if (inClosed)
                {
                    // If we didn't find a shorter route, skip
                    if(endNodeRecord.costSoFar <= endNodeCost)
                    {
                        continue;
                    }

                    // otherwise, remove it from the closed list
                    closed.Remove(endNodeRecord);

                    // We can use the node's old cost values to calculate its heuristic value without calling the possibly expensive function
                    endNodeHeuristic = endNodeRecord.estimatedTotalCost - endNodeRecord.costSoFar;
                } else if(inOpen)
                {
                    // if our route is no better, skip
                    if(endNodeRecord.costSoFar <= endNodeCost)
                    {
                        continue;
                    }

                    // Again calculate heuristic
                    endNodeHeuristic = endNodeRecord.estimatedTotalCost - endNodeRecord.costSoFar;
                }
                else
                {
                    //otherwise we know we've got an unvistied node,
                    // so make a record for it

                    endNodeRecord = new NodeRecord();
                    endNodeRecord.Tile = endNode;

                    // Calculate heuristic using function
                    endNodeHeuristic = heuristic(endNodeRecord.Tile, endNodeRecord.Tile, end);
                }

                // We're here if we need to update the node. Update the cost, estimate, and connection
                endNodeRecord.costSoFar = endNodeCost;
                endNodeRecord.connection = con;
                endNodeRecord.estimatedTotalCost = endNodeRecord.costSoFar + endNodeHeuristic;

                // if displaying costs, update the tile display
                if(displayCosts)
                {
                    endNodeRecord.Display(endNodeCost);
                }

                // And add the record to the open list.
                if(!inOpen)
                {
                    open.Add(endNodeRecord);
                }

                // if coloring tiles, update the open tile color
                if(colorTiles)
                {
                    endNodeRecord.ColorTile(openColor);
                }

                // Pause the animation to show the new open tile
                // this is the actual c# command to use
                yield return new WaitForSeconds(waitTime);
            }

            // we've finished looking at the connections for the current node, so add it to the closed list and remove it from the open list
            open.Remove(current);
            closed.Add(current);

            // If coloring tiles, update the closed tile color
            if(colorTiles)
            {
                current.ColorTile(closedColor);
            }
        }

        // Stops the stopwatch.
        watch.Stop();

        UnityEngine.Debug.Log("Seconds Elapsed: " + (watch.ElapsedMilliseconds / 1000f).ToString());
        UnityEngine.Debug.Log("Nodes Expanded: " + ((closed.Count + open.Count).ToString()));

        // Reset the stopwatch.
        watch.Reset();

        // Determine whether A* found a path and print it here.
        // We're here is we've either found the goal, or if we've no more nodes to search, find which
        if (current.Tile != end)
        {
            // We've run out of nodes without finding the goal, so there's no solution.
            UnityEngine.Debug.Log("Search Failed");

        } else
        {
            Stack<NodeRecord> tempStack = new Stack<NodeRecord>();
            while(current.Tile != start)
            {
                tempStack.Push(current);

                if (current.Tile.Equals(end) && colorTiles)
                {
                    current.ColorTile(Color.magenta);
                }

                NodeRecord tempRecord = new NodeRecord();
                foreach (NodeRecord nr in closed)
                {
                    if(nr.Tile.Equals(current.connection.getFromNode()))
                    {
                        tempRecord = nr;
                        break;
                    }
                }

                current = tempRecord;

                if (colorTiles)
                {
                    current.ColorTile(pathColor);

                    if (current.Tile.Equals(start))
                    {
                        current.ColorTile(Color.magenta);
                    }
                }

                yield return new WaitForSeconds(waitTime);
            }
            path = tempStack;
            path.Push(startRecord);
            UnityEngine.Debug.Log("Path Length: " + path.Count.ToString());

        }


        yield return null;
    }

    public static NodeRecord SmallestHeuristicElement(List<NodeRecord> _open)
    {
        NodeRecord smallVal = _open[0];
        foreach (NodeRecord nr in _open)
        {
            if (nr.estimatedTotalCost < smallVal.estimatedTotalCost)
            {
                smallVal = nr;
            }
        }
        return smallVal;
    }

    public delegate float Heuristic(GameObject start, GameObject tile, GameObject goal);

    public static float Uniform (GameObject start, GameObject tile, GameObject goal)
    {
        return 0f;
    }

    public static float Manhattan (GameObject start, GameObject tile, GameObject goal)
    {
        float dx = Mathf.Abs(tile.transform.position.x - goal.transform.position.x);
        float dy = Mathf.Abs(tile.transform.position.y - goal.transform.position.y);

        return dx + dy;
    }

    public static float CrossProduct (GameObject start, GameObject tile, GameObject goal)
    {
        float dx1 = tile.transform.position.x - goal.transform.position.x;
        float dy1 = tile.transform.position.y - goal.transform.position.y;
        float dx2 = start.transform.position.x - goal.transform.position.x;
        float dy2 = start.transform.position.y - goal.transform.position.y;
        float cross = Mathf.Abs(dx1 * dx2 - dx2 * dy1);
        return Manhattan(start, tile,goal) + cross * 0.001f;
    }

}


