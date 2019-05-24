using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroscopeController : MonoBehaviour
{
    bool gyroEnabled = false;

    Gyroscope gyro;

    void Start()
    {
        gyroEnabled = EnableGyroscope();
    }

    bool EnableGyroscope()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Debug.Log("Gyro activated");
            gyro = Input.gyro;
            gyro.enabled = true;
            gyro.updateInterval = 0.0167f;
            return true;
        }
        Debug.Log("Gyro activated");
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gyroEnabled)
        {
            NetworkClientManager.instance.SendGyroscopeInfo(gyro.attitude);

        }
    }
}
