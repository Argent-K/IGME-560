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
        startRecord.tileConnection = null;
        startRecord.costSoFar = 0;

        // Initialize the open and closed lists
        List<NodeRecord> open = new List<NodeRecord>();
        open.Add(startRecord);
        List<NodeRecord> closed = new List<NodeRecord>();

        // Iterate through processing each node
        while (open.Count > 0)
        {
            // find the smallest element in the open list
            NodeRecord current = SmallestElement(open);

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
            // email
            current.connections = Graph.GetConnections(current);
        }
        


        // Stops the stopwatch.
        watch.Stop();

        UnityEngine.Debug.Log("Seconds Elapsed: " + (watch.ElapsedMilliseconds / 1000f).ToString());
        UnityEngine.Debug.Log("Nodes Expanded: " + "print the number of nodes expanded here.");

        // Reset the stopwatch.
        watch.Reset();

        // Determine whether Dijkstra found a path and print it here.

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
    public Connection tileConnection = new Connection();

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

    public float cost = 0.0f;

    public float GetCost()
    {
        return cost;
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
