using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSignObstacle : MonoBehaviour
{
    [SerializeField]
    private float RotationSpeed;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer >= 9 && collision.gameObject.layer <= 12)
        {
            collision.gameObject.GetComponent<PlayerObstacleManager>().RotatingSign(transform.position);
        }
    }

    // Keep rotating around the "forward" ax
    private void Update()
    {
        transform.RotateAround(transform.position, transform.forward, RotationSpeed * Time.deltaTime);
    }
}
