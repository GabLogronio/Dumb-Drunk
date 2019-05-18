using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{

    // THE ORDER IS RIGHT FOOT, LEFT FOOT, RIGHT HAND, LEFT HAND
    [SerializeField]
    KeyCode[] PlayerControls;

    Vector3 PlayerCurrentDirection = Vector3.zero;

    [SerializeField]
    FootController PlayerRightFoot, PlayerLeftFoot;

    [SerializeField]
    HandController PlayerRightHand, PlayerLeftHand;

    [SerializeField]
    GameObject DirectionArrow;

    [Header("Player1 = true, Player2 = false")]
    [SerializeField]
    bool Player1;
    bool BlockedControls = false, RightFootSet = false, LeftFootSet = false, MovingBack = false, Initialized = false;

    Vector3 PreviousPosition;
    //PlayerBarsManager BarsManager;

    private void Start()
    {
        PreviousPosition = transform.position;
        //BarsManager = GetComponent<PlayerBarsManager>();
        RerollControls();

    }


    void Update()
    {
        GetDirection();
        CheckMovement();
        if (!BlockedControls) GetInput();

    }

    void GetInput()
    {/*
        // ----------------------------------- PLAYER RIGHT FOOT ----------------------------------- //
        if (( (Player1 && (Input.GetKeyDown(KeyCode.L))) || (!Player1 && (Input.GetKeyDown(KeyCode.Keypad3))) || (Input.GetKeyDown(PlayerControls[0])) ) && RightFootSet && LeftFootSet)
            PlayerRightFoot.Detach();
        if ( (Player1 && (Input.GetKey(KeyCode.L))) || (!Player1 && (Input.GetKey(KeyCode.Keypad3))) || (Input.GetKey(PlayerControls[0])) )
            PlayerRightFoot.Move(PlayerCurrentDirection, MovingBack);
        if ( (Player1 && (Input.GetKeyUp(KeyCode.L))) || (!Player1 && (Input.GetKeyUp(KeyCode.Keypad3))) || (Input.GetKeyUp(PlayerControls[0])) )
            PlayerRightFoot.Release();

        // ----------------------------------- PLAYER LEFT FOOT ----------------------------------- //
        if (( (Player1 && (Input.GetKeyDown(KeyCode.K))) || (!Player1 && (Input.GetKeyDown(KeyCode.Keypad1))) || (Input.GetKeyDown(PlayerControls[1])) ) && RightFootSet && LeftFootSet)
            PlayerLeftFoot.Detach();
        if ( (Player1 && (Input.GetKey(KeyCode.K))) || (!Player1 && (Input.GetKey(KeyCode.Keypad1))) || (Input.GetKey(PlayerControls[1])) )
            PlayerLeftFoot.Move(PlayerCurrentDirection, MovingBack);
        if ( (Player1 && (Input.GetKeyUp(KeyCode.K))) || (!Player1 && (Input.GetKeyUp(KeyCode.Keypad1))) || (Input.GetKeyUp(PlayerControls[1])) )
            PlayerLeftFoot.Release();

        // ----------------------------------- PLAYER RIGHT HAND ----------------------------------- //
        if ((Player1 && (Input.GetKey(KeyCode.O))) || (!Player1 && (Input.GetKey(KeyCode.Keypad6))) || (Input.GetKey(PlayerControls[2])))
            PlayerRightHand.Move(PlayerCurrentDirection);
        if ((Player1 && (Input.GetKeyUp(KeyCode.O))) || (!Player1 && (Input.GetKeyUp(KeyCode.Keypad6))) || (Input.GetKeyUp(PlayerControls[2])))
            PlayerRightHand.SetDetachable(true);

        // ----------------------------------- PLAYER LEFT HAND ----------------------------------- //
        if ((Player1 && (Input.GetKey(KeyCode.I))) || (!Player1 && (Input.GetKey(KeyCode.Keypad4))) || (Input.GetKey(PlayerControls[3])))
            PlayerLeftHand.Move(PlayerCurrentDirection);
        if ((Player1 && (Input.GetKeyUp(KeyCode.I))) || (!Player1 && (Input.GetKeyUp(KeyCode.Keypad4))) || (Input.GetKeyUp(PlayerControls[3])))
            PlayerLeftHand.SetDetachable(true);

        if ((Player1 && (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Joystick1Button4))) || (!Player1 && (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Joystick2Button4))))
        {
            PlayerLeftHand.Detach();
            PlayerRightHand.Detach();
        }

        if ((Player1 && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button5))) || (!Player1 && (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Joystick2Button5))))
            BarsManager.ThrowUp();
            */
    }

    void GetDirection()
    {
        float x = 0f, z = 0f;

        if (Player1 && GameSettings.SettingsInstance.Player1Difficulty == GameSettings.DifficultyLevel.Hard ||
          !Player1 && GameSettings.SettingsInstance.Player2Difficulty == GameSettings.DifficultyLevel.Hard)
        {
            switch (GameSettings.SettingsInstance.AxisOrientation)
            {
                case 0:                                         // INVERTED AXIS
                    if (Player1)
                    {
                        x = Mathf.Clamp(-Input.GetAxis("Player1HorizontalKeyboard") - Input.GetAxis("Player1HorizontalController"), -1.0f, 1.0f);
                        z = Mathf.Clamp(-Input.GetAxis("Player1VerticalKeyboard") - Input.GetAxis("Player1VerticalController"), -1.0f, 1.0f);
                    }
                    else
                    {
                        x = Mathf.Clamp(-Input.GetAxis("Player2HorizontalKeyboard") - Input.GetAxis("Player2HorizontalController"), -1.0f, 1.0f);
                        z = Mathf.Clamp(-Input.GetAxis("Player2VerticalKeyboard") - Input.GetAxis("Player2VerticalController"), -1.0f, 1.0f);
                    }
                    break;

                case 1:                                         // CLOCKWISE ROTATED AXIS
                    if (Player1)
                    {
                        x = Mathf.Clamp(Input.GetAxis("Player1VerticalKeyboard") + Input.GetAxis("Player1VerticalController"), -1.0f, 1.0f);
                        z = Mathf.Clamp(-Input.GetAxis("Player1HorizontalKeyboard") - Input.GetAxis("Player1HorizontalController"), -1.0f, 1.0f);
                    }
                    else
                    {
                        x = Mathf.Clamp(Input.GetAxis("Player2VerticalKeyboard") + Input.GetAxis("Player2VerticalController"), -1.0f, 1.0f);
                        z = Mathf.Clamp(-Input.GetAxis("Player2HorizontalKeyboard") - Input.GetAxis("Player2HorizontalController"), -1.0f, 1.0f);
                    }
                    break;

                case 2:                                         // COUNTERCLOCKWISE ROTATED AXIS
                    if (Player1)
                    {
                        x = Mathf.Clamp(-Input.GetAxis("Player1VerticalKeyboard") - Input.GetAxis("Player1VerticalController"), -1.0f, 1.0f);
                        z = Mathf.Clamp(Input.GetAxis("Player1HorizontalKeyboard") + Input.GetAxis("Player1HorizontalController"), -1.0f, 1.0f);
                    }
                    else
                    {
                        x = Mathf.Clamp(-Input.GetAxis("Player2VerticalKeyboard") - Input.GetAxis("Player2VerticalController"), -1.0f, 1.0f);
                        z = Mathf.Clamp(Input.GetAxis("Player2HorizontalKeyboard")  + Input.GetAxis("Player2HorizontalController"), -1.0f, 1.0f);
                    }
                    break;

            }
        }
        else
        {
            // REGUALR AXIS
            if (Player1)
            {
                x = Mathf.Clamp(Input.GetAxis("Player1HorizontalKeyboard") + Input.GetAxis("Player1HorizontalController"), -1.0f, 1.0f);
                z = Mathf.Clamp(Input.GetAxis("Player1VerticalKeyboard") + Input.GetAxis("Player1VerticalController"), -1.0f, 1.0f);
            }
            else
            {
                x = Mathf.Clamp(Input.GetAxis("Player2HorizontalKeyboard") + Input.GetAxis("Player2HorizontalController"), -1.0f, 1.0f);
                z = Mathf.Clamp(Input.GetAxis("Player2VerticalKeyboard") + Input.GetAxis("Player2VerticalController"), -1.0f, 1.0f);
            }
        }

        PlayerCurrentDirection = new Vector3(x, 0, z).normalized;

        DirectionArrow.transform.LookAt(DirectionArrow.transform.position + PlayerCurrentDirection);

    }

    void CheckMovement()
    {
        /*
        float LateralMovement = transform.InverseTransformDirection(transform.position - PreviousPosition).x;

        PreviousPosition = transform.position;

        if (LateralDirection > 0.25f && LateralMovement > 0.01f) BarsManager.AddBalance(true);
        if (LateralDirection < -0.25f && LateralMovement < -0.01f) BarsManager.AddBalance(false);
        */

        float ForwardDirection = transform.InverseTransformDirection(PlayerCurrentDirection).z;
        if (ForwardDirection < 0f) MovingBack = true;
        else MovingBack = false;

        float LateralMovement = transform.position.x - PreviousPosition.x;
        float LateralDirection = PlayerCurrentDirection.x;

        PreviousPosition = transform.position;

        //if (LateralDirection > 0.25f && LateralMovement > 0.01f) BarsManager.AddBalance(false);
        //if (LateralDirection < -0.25f && LateralMovement < -0.01f) BarsManager.AddBalance(true);

    }

    public void SetFoot(bool RightFoot, bool ToSet)
    {

        if (RightFoot) RightFootSet = ToSet;
        else LeftFootSet = ToSet;

    }

    public void DetachFeet()
    {
        //PlayerRightFoot.Detach();
        //PlayerLeftFoot.Detach();

    }

    public void DetachHands()
    {
        //PlayerRightHand.Detach();
        //PlayerLeftHand.Detach();

    }

    public void SetFeetCanAttach(bool ToSet)
    {
        //PlayerRightFoot.SetFootCanAttach(ToSet);
        //PlayerLeftFoot.SetFootCanAttach(ToSet);

    }

    public void BlockControls(bool ToSet)
    {
        BlockedControls = ToSet;
    }

    public KeyCode[] RerollControls()
    {
        if (!Initialized || (Player1 && GameSettings.SettingsInstance.Player1Difficulty == GameSettings.DifficultyLevel.Normal) || (Player1 && GameSettings.SettingsInstance.Player1Difficulty == GameSettings.DifficultyLevel.Hard)
            || (!Player1 && GameSettings.SettingsInstance.Player2Difficulty == GameSettings.DifficultyLevel.Normal) || (!Player1 && GameSettings.SettingsInstance.Player2Difficulty == GameSettings.DifficultyLevel.Hard))
        {
            PlayerControls = ControlsRandomizer.RandomizerInstance.RandomizeButtons(Player1);
            Initialized = true;
        }
        else ControlsRandomizer.RandomizerInstance.SetupImages(Player1, PlayerControls);
        return PlayerControls;

    }
}


