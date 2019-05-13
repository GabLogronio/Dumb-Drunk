using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    float Hor = 0f, Ver = 0f;
    float XRot = 0f, YRot = 0f, ZRot = 0f;

    public void SetAnalogAxis(float ToSetHor, float ToSetVer)
    {
        Hor = ToSetHor;
        Ver = ToSetVer;
    }

    public void SetGyroscope(float ToSetXRot, float ToSetYRot, float ToSetZRot)
    {
        XRot = ToSetXRot;
        YRot = ToSetYRot;
        ZRot = ToSetZRot;

    }

    public void PressedButton(string ButtonName)
    {
        switch (ButtonName)
        {
            case "Red":
                break;
            case "Blue":
                break;
            case "Green":
                break;
            case "Yellow":
                break;

        }
    }

}
