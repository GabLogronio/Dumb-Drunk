using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    float Hor = 0f, Ver = 0f;
    float XRot = 0f, YRot = 0f, ZRot = 0f;

    Dictionary<string, LimbController> Limbs;

    Vector3 PreviousPosition;

    [SerializeField]
    FootController PlayerRightFoot, PlayerLeftFoot;

    [SerializeField]
    HandController PlayerRightHand, PlayerLeftHand;

    [SerializeField]
    GameObject DirectionArrow;

    bool MovingRightFoot = false, MovingLeftFoot = false, MovingRightHand = false, MovingLeftHand = false;
    bool BlockedControls = false, RightFootSet = false, LeftFootSet = false, MovingBack = false, Initialized = false;

    // ----------------------------- INPUT -----------------------------
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

    public void PressedButton(string ButtonName, bool Up)
    {
        if (Up)
        {
            Limbs[ButtonName].Detach();
            // Moving = true;
        }
        else
        {
            //MovingBack = false;
        }

    }

}
