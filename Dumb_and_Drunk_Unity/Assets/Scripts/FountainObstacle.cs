using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FountainObstacle : MonoBehaviour
{
    [SerializeField]
    GameObject Water;

    [SerializeField]
    float FountaHeight, ShootHeight,MaxX, MinX, MaxY, MinY, ShootCooldown;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Shoot", ShootCooldown);

    }

    void Shoot()
    {
        float X = Random.Range(MinX, MaxX), Y = Random.Range(MinY, MaxY);
        if (X > 0f) X += 1.5f; else X -= 1.5f;
        if (Y > 0f) Y += 1.5f; else Y -= 1.5f;
        Vector3 ShootDirection = new Vector3(X, ShootHeight, Y);
        GameObject NewWater = Instantiate(Water, transform.position + Vector3.up * FountaHeight, Quaternion.identity);
        NewWater.GetComponent<Rigidbody>().velocity = ShootDirection;
        Invoke("Shoot", ShootCooldown);
    }
}
