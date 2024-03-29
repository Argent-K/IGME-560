using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Performs search using Dijkstra's algorithm.
/// </summary>
public class Dijkstra : MonoBehaviour
{
    // Colors for the different search categories.
    public static Color openColor = Color.cyan;
    public static Color closedColor = Color.blue;
    public static Color activeColor = Color.yellow;
    public static Color pathColor = Color.yellow;

    // The stopwatch for timing search.
    private static Stopwatch watch = new Stopwatch();


    public static IEnumerator search(GameObject start, GameObject end, float waitTime, bool colorTiles = false, bool displayCosts = false, Stack<NodeRecord> path = null)
    {
        // Starts the stopwatch.
        watch.Start();  

        // Add your Dijkstra code here.

        // Initialize the record for the start node
        NodeRecord startRecord = new NodeRecord();
        startRecord.Tile = start;
        startRecord.connection = null;
        startRecord.costSoFar = 0;

        // Initialize the open and closed lists
        List<NodeRecord> open = new List<NodeRecord>();
        open.Add(startRecord);
        List<NodeRecord> closed = new List<NodeRecord>();

        NodeRecord current = new NodeRecord();
        NodeRecord endNodeRecord = new NodeRecord();

        // Iterate through processing each node
        while (open.Count > 0)
        {
            // find the smallest element in the open list
            current = SmallestElement(open);

            if(colorTiles)
            {
                current.ColorTile(activeColor);
            }

            // Pause the animation to show the new active tile
            // This is the actual c# command to use
            yield return new WaitForSeconds(waitTime);

            // If it is the goal node, then terminate
            if (current.Tile == end)
            {
                break;
            }



            // otherwise get its outgoing connections
            List<Connection> connections = new List<Connection>();

            foreach (GameObject obj in current.Tile.GetComponent<Node>().Connections.Values)
            {
                Connection testCon = new Connection();
                testCon.Cost = 1.0f;
                testCon.toNode = obj;
                testCon.fromNode = current.Tile;
                connections.Add(testCon);
            }

            // Loop through connection in turn (gameobject? direction?)
            foreach (Connection con in connections)
            {
                // get the cost estimate for the end node
                GameObject endNode = con.getToNode();
                float endNodeCost = current.costSoFar + con.GetCost();

                // if the node is closed skip
                bool inClosed = false;
                bool inOpen = false;
                endNodeRecord = new NodeRecord();
                foreach(NodeRecord cl in closed)
                {
                    if(cl.Tile.Equals(endNode))
                    {
                        inClosed = true;

                        // Here we find the record in the closed list
                        // corresponding to the endNode.
                        endNodeRecord = cl;
                    }
                }

                foreach(NodeRecord ol in open)
                {
                    if(ol.Tile.Equals(endNode))
                    {
                        inOpen = true;
                        // WE find the record in the open list 
                        endNodeRecord = ol;
                    }
                }

                // Skip if node is closed
                if (inClosed)
                {
                    continue;
                }
                else if (inOpen)
                {
                    
                    if (endNodeRecord.costSoFar <= endNodeCost)
                    {
                        continue;
                    }
                    
                } else
                {
                    // Otherwise we know we've got an unvisited novde,
                    // make a record for it
                    endNodeRecord = new NodeRecord();
                    endNodeRecord.Tile = endNode;
                }

                // we're here if we need to update the node
                // Update the cost and connection
                endNodeRecord.costSoFar = endNodeCost;
                endNodeRecord.connection = con;

                // if displaying costs, update the tile display
                if(displayCosts)
                {
                    endNodeRecord.Display(endNodeCost);
                }

                // And add it to the open list
                if(!inOpen)
                {
                    open.Add(endNodeRecord);
                }

                // if coloring tiles, update the open tile color.
                if(colorTiles)
                {
                    endNodeRecord.ColorTile(openColor);
                }

                // Pause the animation to show the new open tile.
                // This is the actual c# command to use.
                yield return new WaitForSeconds(waitTime);
            }

            // We've finished looking at the connections for the current node, so add it to the closed list and remove it from the open list
            open.Remove(current);
            closed.Add(current);

            // if coloring tiles update the closed tile color.
            if(colorTiles)
            {
                current.ColorTile(closedColor);
            }

        }
        


        // Stops the stopwatch.
        watch.Stop();

        UnityEngine.Debug.Log("Seconds Elapsed: " + (watch.ElapsedMilliseconds / 1000f).ToString());
        UnityEngine.Debug.Log("Nodes Expanded: " +  ((closed.Count + open.Count).ToString()));

        // Reset the stopwatch.
        watch.Reset();

        // Determine whether Dijkstra found a path and print it here.
        if(current.Tile != end)
        {
            // We've run out of nodes without finding the goal,
            // So there's no solution.
            // your c# code should look very similar, but not exact...
            UnityEngine.Debug.Log("Search Failed");
        }
        else
        {
            // Work back along the path, accumulating connections
            //foreach(Connection con in current.connections)
            //{
            //    UnityEngine.Debug.Log(con.ToString());
            //}
            Stack<NodeRecord> test = new Stack<NodeRecord>();
            while (current.Tile != start)
            {

                // Pushes current nodeRecord onto the path
                test.Push(current);

                // keeps end magenta
                if (current.Tile.Equals(end) && colorTiles)
                {
                    current.ColorTile(Color.magenta);
                }

                // convert the connection gameobject to noderecord
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

                    if(current.Tile.Equals(start))
                    {
                        current.ColorTile(Color.magenta);
                    }
                }


                yield return new WaitForSeconds(waitTime);
            }

            path = test;
            path.Push(startRecord);
            UnityEngine.Debug.Log("Path Length: " + path.Count.ToString());

        }

        yield return null;
    }
    public static NodeRecord SmallestElement(List<NodeRecord> _open)
    {   
        NodeRecord smallVal = _open[0];
        foreach (NodeRecord nr in _open)
        {
            if(nr.costSoFar < smallVal.costSoFar)
            {
                smallVal = nr;
            }
        }

        return smallVal;
    }
}

/// <summary>
/// A class for recording search statistics.
/// </summary>
public class NodeRecord
{
    // The tile game object.
    public GameObject Tile { get; set; } = null;

    // Set the other class properties here.
    //public Connection connection { get; set; } = null;
    public Connection connection { get; set; } = null;

    public float estimatedTotalCost { get; set; } = 0.0f;
    public float costSoFar { get; set; } = 0.0f;
    // Sets the tile's color.
    public void ColorTile (Color newColor)
    {
        SpriteRenderer renderer = Tile.GetComponentInChildren<SpriteRenderer>();
        renderer.material.color = newColor;
    }

    // Displays a string on the tile.
    public void Display (float value)
    {
        TextMesh text = Tile.GetComponent<TextMesh>();
        text.text = value.ToString();
    }
}

public class Connection
{
    public GameObject fromNode { get; set; } = null;

    public GameObject toNode { get; set; } = null;

    public float Cost { get; set; } = 1.0f;

    public float GetCost()
    {
        return Cost;
    }

    public GameObject getFromNode()
    {
        return fromNode;
    }

    public GameObject getToNode()
    {
        return toNode;
    }
}
