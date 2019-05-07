using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrannyObstacle : MovingObstacle
{
    [SerializeField]
    float MinX, MaxX, MinZ, MaxZ, PauseTime, RotationSpeed;

    float PauseTimer;

    Vector3 Destination;

    void Start()
    {
        CalculateNewDestination();
    }
	
	// Update is called once per frame
	void Update () {

        if (PauseTimer <= PauseTime) PauseTimer += Time.deltaTime;
        else
        {
            if (Vector3.Distance(transform.position, Destination) < 0.1f) CalculateNewDestination();
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, Destination, Time.deltaTime * MovementSpeed);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Destination - transform.position), Time.deltaTime * RotationSpeed);
            } 
        }

    }

    protected override void Activate()
    {
        target.gameObject.GetComponent<PlayerObstacleManager>().Granny(transform.position);
    }

    void CalculateNewDestination()
    {
        Destination = new Vector3(Random.Range(MinX, MaxX), transform.position.y, Random.Range(MinZ, MaxZ));
        PauseTimer = 0f;
    }
}
