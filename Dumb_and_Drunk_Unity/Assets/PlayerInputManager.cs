using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInputManager : MonoBehaviour
{
    float Hor = 0f, Ver = 0f;

    Dictionary<string, LimbController> Limbs = new Dictionary<string, LimbController>();
    string[] Buttons = { "Red", "Blue", "Green", "Yellow" };

    BodyCenter bd;

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
        bd = GetComponent<BodyCenter>();
        RandomizeControls();
    }

    private void Update()
    {
        // FOR TESTING PURPOSE ONLY --------------------------------------------------

        if (Input.GetKeyDown(KeyCode.JoystickButton0)) PressedButton("Green", true);
        if (Input.GetKeyUp(KeyCode.JoystickButton0)) PressedButton("Green", false);

        if (Input.GetKeyDown(KeyCode.JoystickButton1)) PressedButton("Red", true);
        if (Input.GetKeyUp(KeyCode.JoystickButton1)) PressedButton("Red", false);

        if (Input.GetKeyDown(KeyCode.JoystickButton2)) PressedButton("Blue", true);
        if (Input.GetKeyUp(KeyCode.JoystickButton2)) PressedButton("Blue", false);

        if (Input.GetKeyDown(KeyCode.JoystickButton3)) PressedButton("Yellow", true);
        if (Input.GetKeyUp(KeyCode.JoystickButton3)) PressedButton("Yellow", false);

        Hor = Input.GetAxis("Horizontal");
        Ver = Input.GetAxis("Vertical");

        SetGyroscope(Input.GetAxis("RightStickHor"), Input.GetAxis("RightStickVer"));

        // FOR TESTING PURPOSE ONLY --------------------------------------------------

        foreach (LimbController Limb in Limbs.Values)
        {
            Limb.UpdateDirection(new Vector2(Hor, Ver), MovingBack);
        }

        DirectionArrow.transform.LookAt(DirectionArrow.transform.position + new Vector3(-Ver, 0, Hor).normalized);

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

    public void SetGyroscope(float ToSetXRot, float ToSetZRot)
    {
        bd.MoveBodyCenter(ToSetXRot, ToSetZRot);

    }

    public void PressedButton(string ButtonName, bool Down)
    {
        if (Down)
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
