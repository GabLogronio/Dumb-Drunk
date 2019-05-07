using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class TestController : MonoBehaviour
{
    [SerializeField]
    string PlayerNumber = "0";

    [SerializeField]
    Text DebugText;

    float speed = 1f, rotationspeed = 100f;

    // Update is called once per frame
    void Update()
    {
        switch (PlayerNumber)
        {
            case "1":
                transform.Translate(0, 0, VirtualAxisManager.P1Ver * speed * Time.deltaTime);
                transform.Rotate(0, VirtualAxisManager.P1Hor * rotationspeed * Time.deltaTime, 0);
                break;
            case "2":
                transform.Translate(0, 0, VirtualAxisManager.P2Ver * speed * Time.deltaTime);
                transform.Rotate(0, VirtualAxisManager.P2Hor * rotationspeed * Time.deltaTime, 0);
                break;
            case "3":
                transform.Translate(0, 0, VirtualAxisManager.P3Ver * speed * Time.deltaTime);
                transform.Rotate(0, VirtualAxisManager.P3Hor * rotationspeed * Time.deltaTime, 0);
                break;
            case "4":
                transform.Translate(0, 0, VirtualAxisManager.P4Ver * speed * Time.deltaTime);
                transform.Rotate(0, VirtualAxisManager.P4Hor * rotationspeed * Time.deltaTime, 0);
                break;
        }
    }

}
