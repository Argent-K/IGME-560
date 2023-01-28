using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    [SerializeField]
    public GameObject character;

    [SerializeField]
    public float maxSpeed = 0.01f;
    [SerializeField]
    private float maxRotation = 5f;

    KinematicSteeringOutput moveValues;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveValues = GetSteering();
        character.transform.position += moveValues.velocity;
        character.transform.eulerAngles += new Vector3(0, 0, moveValues.rotation);
    }

    float DistToTarget(Vector3 _curPosition, Vector3 _target)
    {
        float testx = (_target.x - _curPosition.x) * (_target.x - _curPosition.x);
        float testy = (_target.y - _curPosition.y) * (_target.y - _curPosition.y);
        return Mathf.Sqrt(testx + testy);
    }

    float randomBinomial()
    {
        return Random.Range(0.0f, 1.0f) - Random.Range(0.0f, 1.0f);
    }


    KinematicSteeringOutput GetSteering()
    {
        KinematicSteeringOutput result = new KinematicSteeringOutput();

        // Get velocity from the vector form of the orientation
        // maxSpeed * unit vector of character direction
        result.velocity = maxSpeed * new Vector3(-Mathf.Sin(character.transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Cos(character.transform.eulerAngles.z * Mathf.Deg2Rad), 0.0f).normalized;

        result.rotation = randomBinomial() * maxRotation;

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