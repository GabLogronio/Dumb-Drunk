using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputManager: MonoBehaviour
{
    public abstract void SetAnalogAxis(float ToSetHor, float ToSetVer);

    public abstract void SetGyroscope(float ToSetXRot, float ToSetZRot);

    public abstract void PressedButton(string ButtonName, bool Down);
}
