using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Wander : MonoBehaviour
{
    [SerializeField]
    public GameObject character;


    public List<Vector3> targetList;
    public Vector3 curTarget;
    private int target = 0;

    [SerializeField]
    public float maxSpeed = 0.01f;
    

    // Start is called before the first frame update
    void Start()
    {
        targetList.Add(new Vector3(7, 4, 0));
        targetList.Add(new Vector3(-7, 4, 0));
        targetList.Add(new Vector3(-7, -4, 0));
        targetList.Add(new Vector3(7, -4, 0));

        curTarget = targetList[target];
    }

    // Update is called once per frame
    void Update()
    {
        character.transform.position += GetSteering().velocity;
    }

    float DistToTarget(Vector3 _curPosition, Vector3 _target)
    {
        float testx = (_target.x - _curPosition.x) * (_target.x - _curPosition.x);
        float testy = (_target.y - _curPosition.y) * (_target.y - _curPosition.y);
        return Mathf.Sqrt(testx + testy);
    }




    KinematicSteeringOutput GetSteering()
    {
        KinematicSteeringOutput result = new KinematicSteeringOutput();

        if(DistToTarget(character.transform.position, curTarget) < 0.1f)
        {
            target++;
            if (target > 3)
            {
                target = 0;
            }
            curTarget = targetList[target];
   
        }


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