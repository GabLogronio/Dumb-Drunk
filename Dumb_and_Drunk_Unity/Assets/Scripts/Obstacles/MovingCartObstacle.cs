using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCartObstacle : MovingObstacle
{
    [SerializeField]
    float Min, Max;

    bool Direction = false, Moving = true;

    [SerializeField]
    float PauseTime;
    [SerializeField]
    float PauseTimer, CurrentDestination;

	// Use this for initialization
	void Start () {
        Stop();
	}

    // Update is called once per frame
    void Update() {

        if (PauseTimer <= PauseTime)
            PauseTimer += Time.deltaTime;
        else
        {
            if (transform.position.z - CurrentDestination < 0.1f && transform.position.z - CurrentDestination > -0.1f) Stop();
            else
            {
                Moving = true;
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, CurrentDestination), Time.deltaTime * MovementSpeed);

            }

        }
    }

    protected override void Activate()
    {
        if (Moving)
        {
            Stop();
            target.gameObject.GetComponent<PlayerObstacleManager>().MovingCart(transform.position);

        }

    }

    void Stop()
    {
        Direction = !Direction;

        if (Direction) CurrentDestination = Random.Range(Min, transform.position.z);
        else CurrentDestination = Random.Range(transform.position.z, Max);

        Moving = false;
        PauseTimer = 0f;
    }

}
