using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInputManager : MonoBehaviour
{
    float Hor = 0f, Ver = 0f;
    float XRot = 0f, YRot = 0f, ZRot = 0f;

    Dictionary<string, LimbController> Limbs = new Dictionary<string, LimbController>();
    string[] Buttons = { "Red", "Blue", "Green", "Yellow" };

    Vector3 PreviousPosition;

    [SerializeField]
    LimbController[] LimbControllers;

    [SerializeField]
    GameObject DirectionArrow;

    [SerializeField]
    Text DebugText;

    bool BlockedControls = false, RightFootSet = true, LeftFootSet = true, MovingBack = false;

    private void Start()
    {
        RandomizeControls();
    }

    private void Update()
    {
        foreach (LimbController Limb in Limbs.Values)
        {
            Limb.UpdateDirection(new Vector2(Hor, Ver), MovingBack);
        }
    }

    // ----------------------------- INPUT -----------------------------
    public void SetAnalogAxis(float ToSetHor, float ToSetVer)
    {
        Hor = ToSetHor;
        Ver = ToSetVer;

        float ForwardDirection = transform.InverseTransformDirection(new Vector3(Hor, transform.position.y, Ver)).z;
        if (ForwardDirection < 0f) MovingBack = true;
        else MovingBack = false;

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
            Limbs[ButtonName].Move();
            DebugText.text += "Released " + ButtonName + "\n";
        }
        else
        {
            Limbs[ButtonName].Release();
            DebugText.text += "Pressed " + ButtonName + "\n";
        }
    }

    public void SetFoot(bool RightFoot, bool ToSet)
    {

        if (RightFoot) RightFootSet = ToSet;
        else LeftFootSet = ToSet;

    }

    void RandomizeControls()
    {
        for (int i = 0; i < Buttons.Length; i++ )
        {
            int ran = Random.Range(i, Buttons.Length);

            string temp = Buttons[i];
            Buttons[i] = Buttons[ran];
            Buttons[ran] = temp;

        }

        for (int i = 0; i < Buttons.Length; i++)
        {
            foreach (Transform ColorTransform in transform)
            {
                if(ColorTransform.gameObject.name == Buttons[i])
                {
                    ColorTransform.gameObject.SetActive(true);
                    ColorTransform.SetParent(LimbControllers[i].gameObject.transform);
                    ColorTransform.gameObject.transform.position = LimbControllers[i].gameObject.transform.position;

                }
            }

            Limbs.Add(Buttons[i], LimbControllers[i]);

        }
    }
}
