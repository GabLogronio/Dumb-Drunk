using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInputManager : InputManager
{
    float Hor = 0f, Ver = 0f;

    Dictionary<LimbController, string> ControllerStringDictionary = new Dictionary<LimbController, string>();
    Dictionary<LimbController, Vector3> ControllerToPositionDictionary = new Dictionary<LimbController, Vector3>();
    Dictionary<string, RawImage> StringToImageDictionary = new Dictionary<string, RawImage>();
    string[] ButtonsStrings = {"Blue", "Green", "Red", "Yellow" };

    [SerializeField]
    PlayerBalanceManager BalanceManager;

    Vector3 PreviousPosition;

    [SerializeField]
    LimbController[] LimbControllers = new LimbController[4];

    [SerializeField]
    RawImage[] ButtonsImages = new RawImage[4];

    [SerializeField]
    Vector3[] ButtonPositions = new Vector3[4];

    [SerializeField]
    GameObject DirectionArrow;

    [SerializeField]
    Text DebugText;

    bool BlockedControls = false, RightFootSet = true, LeftFootSet = true, MovingBack = false;

    private void Start()
    {
        Setup();
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

        foreach (LimbController Limb in ControllerStringDictionary.Keys)
        {
            Limb.UpdateDirection(new Vector2(Hor, Ver), MovingBack);
        }

        DirectionArrow.transform.LookAt(DirectionArrow.transform.position + new Vector3(-Ver, 0, Hor).normalized);

    }

    // ----------------------------- INPUT -----------------------------
    public override void SetAnalogAxis(float ToSetHor, float ToSetVer)
    {
        Hor = ToSetHor;
        Ver = ToSetVer;

        float ForwardDirection = transform.InverseTransformDirection(new Vector3(Hor, transform.position.y, Ver)).z;
        if (ForwardDirection < 0f) MovingBack = true;
        else MovingBack = false;

    }

    public override void SetGyroscope(float ToSetXRot, float ToSetZRot)
    {
        BalanceManager.MoveBodyCenter(ToSetXRot, ToSetZRot);

    }

    public override void PressedButton(string ButtonName, bool Down)
    {
        if (!BlockedControls)
        {
            if (Down)
            {
                ControllerStringDictionary.FirstOrDefault(x => x.Value == ButtonName).Key.Move();
                DebugText.text += "Released " + ButtonName + "\n";
            }
            else
            {
                ControllerStringDictionary.FirstOrDefault(x => x.Value == ButtonName).Key.Release();
                DebugText.text += "Pressed " + ButtonName + "\n";
            }
        }

    }

    public void SetFoot(bool RightFoot, bool ToSet)
    {

        if (RightFoot) RightFootSet = ToSet;
        else LeftFootSet = ToSet;

    }

    public void Detach()
    {
        foreach (LimbController Limb in ControllerStringDictionary.Keys)
        {
            Limb.Detach();
        }
    }

    public void SetCanAttach(bool ToSet)
    {
        foreach (LimbController Limb in ControllerStringDictionary.Keys)
        {
            Limb.Set(ToSet);
        }
    }

    public void BlockControls(bool ToSet)
    {
        BlockedControls = ToSet;
    }

    void Setup()
    {
        for (int i = 0; i < LimbControllers.Length; i++)
        {
            ControllerToPositionDictionary.Add(LimbControllers[i], ButtonPositions[i]);
            StringToImageDictionary.Add(ButtonsStrings[i], ButtonsImages[i]);
            ControllerStringDictionary.Add(LimbControllers[i], ButtonsStrings[i]);
        }

        RandomizeControls();
    }

    public void RandomizeControls()
    {
        // Shuffles the ButtonsStrings array
        for (int i = 0; i < ButtonsStrings.Length; i++ )
        {
            int ran = Random.Range(i, ButtonsStrings.Length);

            string temp = ButtonsStrings[i];
            ButtonsStrings[i] = ButtonsStrings[ran];
            ButtonsStrings[ran] = temp;

        }

        for (int i = 0; i < ButtonsStrings.Length; i++)
        {
            ControllerStringDictionary[ControllerStringDictionary.ElementAt(i).Key] = ButtonsStrings[i];
            StringToImageDictionary[ButtonsStrings[i]].GetComponent<RectTransform>().localPosition = ControllerToPositionDictionary[ControllerStringDictionary.ElementAt(i).Key];
        }

    }
}
