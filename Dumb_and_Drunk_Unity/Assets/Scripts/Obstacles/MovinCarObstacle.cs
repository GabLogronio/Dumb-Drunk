using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovinCarObstacle : MonoBehaviour
{
    [SerializeField]
    float MovementSpeed = 5f, WaitTime = 7.5f;

    bool Waiting = false;

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(transform.forward * MovementSpeed * Time.deltaTime);
        if(!Waiting) transform.Translate(Vector3.forward * MovementSpeed * Time.deltaTime);
        if ((transform.position.x > 31.5f) || (transform.position.z < -27.5f) || (transform.position.z > 22f) || (transform.position.x < -28f)) Despawn();

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
            Waiting = true;
            Invoke("StopWaiting", WaitTime);
        }
    }

    void StopWaiting()
    {
        Waiting = false;
    }

}
