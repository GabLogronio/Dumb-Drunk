using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrannyObstacle : MonoBehaviour
{
    [SerializeField]
    float MinX, MaxX, MinZ, MaxZ, PauseTime, MovementSpeed, RotationSpeed;

    float PauseTimer;

    Vector3 Destination;

    void Start()
    {
        CalculateNewDestination();
        
    }

    private void OnDestroy()
    {
        AkSoundEngine.StopAll(gameObject);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer >= 9 && collision.gameObject.layer <= 12)
        {
            AkSoundEngine.PostEvent("GrannyImpact", gameObject);
            collision.gameObject.GetComponent<PlayerObstacleManager>().Granny(transform.position);
            CalculateNewDestination();
        }
    }

    void CalculateNewDestination()
    {
        Destination = new Vector3(Random.Range(MinX, MaxX), transform.position.y, Random.Range(MinZ, MaxZ));
        PauseTimer = 0f;
    }
}
