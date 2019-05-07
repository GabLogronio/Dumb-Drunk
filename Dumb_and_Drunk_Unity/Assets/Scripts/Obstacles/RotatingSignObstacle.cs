using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSignObstacle : StationaryObstacle
{
    [SerializeField]
    private float RotationSpeed;

    // If it hits a player, pushes him away
    protected override void Activate()
    {
        target.GetComponent<PlayerObstacleManager>().Taxi(transform.position);
    }

    // Keep rotating around the "forward" ax
    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.forward, RotationSpeed * Time.deltaTime);
    }
}
