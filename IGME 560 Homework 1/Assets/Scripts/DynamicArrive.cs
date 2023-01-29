using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class DynamicArrive : MonoBehaviour
{
    [SerializeField]
    private GameObject character;
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private float maxAcceleration = 1f;
    [SerializeField]
    private float maxSpeed = 0.01f;
    private float targetSpeed = 0.0f;

    GameObject tr;
    GameObject sr;
    float slowRadius = 3.0f;
    float targetRadius = 1f;

    float timeToTarget = 0.1f;

    

    DynamicSteeringOutput moveValues;

    // Start is called before the first frame update
    void Start()
    {
        tr = new GameObject { name = "TargetRadius" };
        tr.DrawCircle(targetRadius, 0.02f);
        tr.transform.Rotate(90f, 0, 0);

        sr = new GameObject { name = "SlowRadius" };
        sr.DrawCircle(slowRadius, 0.02f);
        sr.transform.Rotate(90f, 0, 0);
    }

    private void Update()
    {
        tr.transform.position = character.transform.position;
        sr.transform.position = character.transform.position;
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
        //result.linear = target.transform.position - character.transform.position;
        Vector3 dirToTarget = target.transform.position - character.transform.position;
        float distance = dirToTarget.magnitude;

        // Check if we are there, return no steering
        if (distance < targetRadius)
        {
            result.linear = Vector3.zero;
            result.angular = 0;
            character.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            
            return result;
        }

        if(distance > slowRadius)
        {
            targetSpeed = maxSpeed;
        } else
        {
            targetSpeed = maxSpeed * distance / slowRadius;
        }

        // The target velocity combines speed and direction
        Vector3 targetVelocity = dirToTarget;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        // Acceleration tries to get to the target velocity
        result.linear = targetVelocity - new Vector3(character.GetComponent<Rigidbody2D>().velocity.x, character.GetComponent<Rigidbody2D>().velocity.y, 0);
        result.linear /= timeToTarget;

        // Check if the acceleration is too fast.
        if (result.linear.magnitude > maxAcceleration)
        {
            result.linear.Normalize();
            result.linear *= maxAcceleration;
        }

        // Face in the direction we want to move.
        character.transform.eulerAngles = new Vector3(0, 0, -NewOrientation(character.transform.rotation.z, new Vector3(character.GetComponent<Rigidbody2D>().velocity.x, character.GetComponent<Rigidbody2D>().velocity.y, 0)));

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
