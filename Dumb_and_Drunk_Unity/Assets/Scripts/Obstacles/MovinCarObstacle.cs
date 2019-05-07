using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovinCarObstacle : MonoBehaviour
{
    [SerializeField]
    float MovementSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(transform.forward * MovementSpeed * Time.deltaTime);
        transform.Translate(Vector3.forward * MovementSpeed * Time.deltaTime);
        if ((transform.position.x > 26f) || (transform.position.z < -15f)) Despawn();

    }

    void Despawn()
    {
        CarsAndCartSpawner.instance.SpawnObstacle();
        gameObject.SetActive(false);
    }
    
}
