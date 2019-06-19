using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerCamera : MonoBehaviour
{
    bool Moving = false;

    public float RotationSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Moving = true;
        if (Moving) transform.Rotate(Vector3.up * RotationSpeed);
    }
}
