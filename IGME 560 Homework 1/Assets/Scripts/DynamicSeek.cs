using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class DynamicSeek : MonoBehaviour
{
    [SerializeField]
    public GameObject character;


    [SerializeField]
    public GameObject target;

    [SerializeField]
    public float maxAcceleration = 1f;

    DynamicSteeringOutput moveValues;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveValues = GetSteering();
        character.GetComponent<Rigidbody2D>().velocity += new Vector2(moveValues.linear.x, moveValues.linear.y);
        
        //character.transform.eulerAngles += new Vector3(0, 0, moveValues.angular);
    }

    float DistToTarget(Vector3 _curPosition, Vector3 _target)
    {
        float testx = (_target.x - _curPosition.x) * (_target.x - _curPosition.x);
        float testy = (_target.y - _curPosition.y) * (_target.y - _curPosition.y);
        return Mathf.Sqrt(testx + testy);
    }




    DynamicSteeringOutput GetSteering()
    {
        DynamicSteeringOutput result = new DynamicSteeringOutput();

        // Get the direction to the target.
        result.linear = target.transform.position - character.transform.position;
        

        // The Velocity is along this direction, at full speed.
        result.linear.Normalize();
        result.linear *= maxAcceleration;

        // Face in the direction we want to move.
        character.transform.eulerAngles = new Vector3(0, 0, -NewOrientation(character.transform.rotation.z, result.linear));

        result.angular = 0;
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

internal class DynamicSteeringOutput
{
    public Vector3 linear;
    public float angular;
}
