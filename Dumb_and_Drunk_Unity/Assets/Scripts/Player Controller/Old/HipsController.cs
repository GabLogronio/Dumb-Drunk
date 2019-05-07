using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HipsController : MonoBehaviour {

    [SerializeField]
    float RotationSpeed;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

	// Update is called once per frame
	void Update () {

        if ((transform.localEulerAngles.x >= 180f && transform.localEulerAngles.x < 360f) || (transform.localEulerAngles.x > 0f && transform.localEulerAngles.x < 15f))
        {
            rb.AddTorque(transform.right * RotationSpeed, ForceMode.VelocityChange);

        }

    }
}
