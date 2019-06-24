using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovinCarObstacle : MonoBehaviour
{
    [SerializeField]
    float MovementSpeed = 5f, WaitTime = 7.5f;

    [SerializeField]
    bool Scene1 = true;
    bool Waiting = false;

    private void Start()
    {
        AkSoundEngine.SetSwitch("NYCObstacles2", "Car", gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(transform.forward * MovementSpeed * Time.deltaTime);
        if(!Waiting) transform.Translate(Vector3.forward * MovementSpeed * Time.deltaTime);
        else transform.Translate( - Vector3.forward * MovementSpeed * 0.25f * Time.deltaTime);
        if (Scene1)
        {
            if ((transform.position.x > 31.5f) || (transform.position.z < -27.5f) || (transform.position.z > 22f) || (transform.position.x < -28f)) Despawn();
        }
        else
        {
            if ((transform.position.x > 56f) || (transform.position.z < -0f) || (transform.position.z > 78f) || (transform.position.x < -28f)) Despawn();

        }


    }

    void Despawn()
    {
        CarsAndCartSpawner.instance.SpawnObstacle();
        // gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer >= 9 && collision.gameObject.layer <= 12)
        {
            AkSoundEngine.PostEvent("ObstacleNYC2", gameObject);
            Waiting = true;
            collision.gameObject.GetComponent<PlayerObstacleManager>().MovingCart(transform.position);
            Invoke("StopWaiting", WaitTime);
        }
    }

    void StopWaiting()
    {
        Waiting = false;
    }

}
