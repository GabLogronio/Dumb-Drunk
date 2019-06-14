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
    //Text DebugText;

    bool BlockedControls = false, RightFootSet = true, LeftFootSet = true, MovingBack = false;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        foreach (LimbController Limb in ControllerStringDictionary.Keys)
        {
            Limb.UpdateDirection(new Vector3(Hor, 0f, Ver), MovingBack);
        }

        DirectionArrow.transform.LookAt(DirectionArrow.transform.position + new Vector3(Hor, 0, Ver).normalized);

    }

    // ----------------------------- INPUT -----------------------------
    public override void SetAnalogAxis(float ToSetHor, float ToSetVer)
    {
        Hor = ToSetHor;
        Ver = ToSetVer;
        //DebugText.instance.Log("ricevuto: " + Hor + ", " + Ver);
        float ForwardDirection = transform.InverseTransformDirection(new Vector3(Hor, transform.position.y, Ver)).z;
        if (ForwardDirection < 0f) MovingBack = true;
        else MovingBack = false;

    }

    public override void SetGyroscope(char Right, char Left, char Down, char Up)
    {
        BalanceManager.MoveBodyCenter(Right, Left, Down, Up);

    }

    public override void Fallen(bool ToSet)
    {
        if (ToSet)
        {
            BalanceManager.Fall();
        }
        else
        {
            BalanceManager.RecoverFromFall();
        }
    }

    public override void PressedButton(string ButtonName, bool Down)
    {
        if (!BlockedControls && RightFootSet && LeftFootSet)
        {
            if (Down)
            {
                ControllerStringDictionary.FirstOrDefault(x => x.Value == ButtonName).Key.Move();
                //DebugText.text += "Released " + ButtonName + "\n";
            }
            else
            {
                ControllerStringDictionary.FirstOrDefault(x => x.Value == ButtonName).Key.Release();
                //DebugText.text += "Pressed " + ButtonName + "\n";
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
