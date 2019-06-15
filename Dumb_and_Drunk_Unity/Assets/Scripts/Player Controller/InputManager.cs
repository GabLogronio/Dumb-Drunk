using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputManager: MonoBehaviour
{
    public abstract void SetAnalogAxis(float ToSetHor, float ToSetVer);

    public abstract void SetGyroscope(char Right, char Left, char Down, char Up);

    public abstract void PressedButton(string ButtonName, bool Down);

    public abstract void Fallen(bool Down);

}
