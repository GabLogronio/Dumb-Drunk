using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField]
    float RotationSpeed = 5f;

    private void Update()
    {
        transform.Rotate(Vector3.up * RotationSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10) // Player Layer
        {
            CancelInvoke();
            DestroyKey();
        }
    }

    void DestroyKey()
    {
        gameObject.SetActive(false);
    }
}
