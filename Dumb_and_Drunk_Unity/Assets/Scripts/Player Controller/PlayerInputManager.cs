using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInputManager : InputManager
{
    Dictionary<LimbController, string> ControllerStringDictionary = new Dictionary<LimbController, string>();
    Dictionary<LimbController, Vector3> ControllerToPositionDictionary = new Dictionary<LimbController, Vector3>();
    Dictionary<string, RawImage> StringToImageDictionary = new Dictionary<string, RawImage>();
    string[] ButtonsStrings = {"Blue", "Green", "Red", "Yellow" };

    [SerializeField]
    PlayerBalanceManager BalanceManager;

    [SerializeField]
    LimbController[] LimbControllers = new LimbController[4];

    [SerializeField]
    RawImage[] ButtonsImages = new RawImage[4];

    [SerializeField]
    Vector3[] ButtonPositions = new Vector3[4];

    [SerializeField]
    GameObject DirectionArrow;

    float Hor = 0f, Ver = 0f;


    bool BlockedControls = false, RightFootSet = true, LeftFootSet = true;

    private void Start()
    {
        Setup();
    }

    // ----------------------------- INPUT -----------------------------
    public override void SetAnalogAxis(float ToSetHor, float ToSetVer)
    {
        Hor = ToSetHor;
        Ver = ToSetVer;

        Vector3 NewDirection = new Vector3(ToSetHor, 0f, ToSetVer);

        foreach (LimbController Limb in LimbControllers)
        {
            Limb.UpdateDirection(NewDirection);
        }

        //DebugText.instance.Set(NewDirection.x + ", " + NewDirection.y + ", " + NewDirection.z);

        if (NewDirection.magnitude > 0.01f)
        {
            DirectionArrow.SetActive(true);
            DirectionArrow.transform.LookAt(DirectionArrow.transform.position + NewDirection.normalized);
        } 
        else DirectionArrow.SetActive(false);

    }

    public override void SetGyroscope(char Right, char Left, char Up, char Down)
    {
        BalanceManager.MoveBodyCenter(Right, Left, Up, Down);

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
        if (!BlockedControls)
        {
            if (Down && RightFootSet && LeftFootSet)
            {
                ControllerStringDictionary.FirstOrDefault(x => x.Value == ButtonName).Key.Move();
                //DebugText.text += "Released " + ButtonName + "\n";
            }
            else if (!Down)
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
