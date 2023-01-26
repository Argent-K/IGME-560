using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class KinematicSeek : MonoBehaviour
{
    [SerializeField]
    public GameObject character;


    public List<Vector3> targetList;
    public Vector3 curTarget;

    [SerializeField]
    public float maxSpeed = 0.01f;
    

    // Start is called before the first frame update
    void Start()
    {
        targetList.Add(new Vector3(7, 4, 0));
        targetList.Add(new Vector3(-7, 4, 0));
        targetList.Add(new Vector3(-7, -4, 0));
        targetList.Add(new Vector3(7, -4, 0));
    }

    // Update is called once per frame
    void Update()
    {
        character.transform.position += GetSteering().velocity;
    }

    KinematicSteeringOutput GetSteering()
    {
        KinematicSteeringOutput result = new KinematicSteeringOutput();
        // Get the direction to the target.
        result.velocity = curTarget - character.transform.position;
        

        // The Velocity is along this direction, at full speed.
        result.velocity.Normalize();
        result.velocity *= maxSpeed;

        Debug.Log(result.velocity);
        // Face in the direction we want to move.
        character.transform.eulerAngles = new Vector3(0, 0, -NewOrientation(character.transform.rotation.z, result.velocity));

        result.rotation = 0;
        return result;
    }

    float NewOrientation(float currentOrientation, Vector3 velocity)
    {
        if (velocity.magnitude > 0)
        {
            return Mathf.Atan2(velocity.x, velocity.y) * Mathf.Rad2Deg;
        }
        else
        {
            return currentOrientation;
        }
    }
}

internal class KinematicSteeringOutput
{
    public Vector3 velocity;
    public float rotation;
}