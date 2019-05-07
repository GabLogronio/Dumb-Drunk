using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsRandomizer : MonoBehaviour {

    [System.Serializable]
    public struct Control 
    {
        public KeyCode ButtonKeyCode;
        public Image ButtonImage;

    }

    [Header("KeyCode connected to Image")]
    [SerializeField]
    Control[] Player1Controls = new Control[]
    {
        new Control { ButtonKeyCode = KeyCode.Joystick1Button0, ButtonImage = null },
        new Control { ButtonKeyCode = KeyCode.Joystick1Button1, ButtonImage = null },
        new Control { ButtonKeyCode = KeyCode.Joystick1Button2, ButtonImage = null },
        new Control { ButtonKeyCode = KeyCode.Joystick1Button3, ButtonImage = null },

    }, Player2Controls = new Control[]
    {
        new Control { ButtonKeyCode = KeyCode.Joystick2Button0, ButtonImage = null },
        new Control { ButtonKeyCode = KeyCode.Joystick2Button1, ButtonImage = null },
        new Control { ButtonKeyCode = KeyCode.Joystick2Button2, ButtonImage = null },
        new Control { ButtonKeyCode = KeyCode.Joystick2Button3, ButtonImage = null },
    };

    [SerializeField]
    Text AxisOrientationText;

    [SerializeField]
    PlayerGUIControlsManager Player1GUI, Player2GUI;

    public static ControlsRandomizer RandomizerInstance = null;

    void Start()
    {

        if (RandomizerInstance == null) RandomizerInstance = this;
        else if (RandomizerInstance != this) Destroy(gameObject);

    }

    public KeyCode[] RandomizeButtons(bool Player1)
    {

        KeyCode[] NewControls = new KeyCode[4];
        Image[] NewGUI = new Image[4];

        for (int i = 0; i < 4; i++)
        {
            if (Player1)
            {
                Control temp = Player1Controls[i];
                int ran = Random.Range(i, Player1Controls.Length);
                Player1Controls[i] = Player1Controls[ran];
                NewControls[i] = Player1Controls[ran].ButtonKeyCode;
                NewGUI[i] = Player1Controls[ran].ButtonImage;
                Player1Controls[ran] = temp;

            }
            else
            {
                Control temp = Player2Controls[i];
                int ran = Random.Range(i, Player2Controls.Length);
                Player2Controls[i] = Player2Controls[ran];
                NewControls[i] = Player2Controls[ran].ButtonKeyCode;
                NewGUI[i] = Player2Controls[ran].ButtonImage;
                Player2Controls[ran] = temp;

            }
        }
        if (Player1) Player1GUI.SetImages(NewGUI);
        else Player2GUI.SetImages(NewGUI);
        return NewControls;

    }

    public void SetupImages(bool Player1, KeyCode[] Controls)
    {
        Image[] NewGUI = new Image[4];

        for (int i = 0; i < 4; i++)
        {
            if (Player1)
            {
                NewGUI[i] = Player1Controls[i].ButtonImage;

            }
            else
            {
                NewGUI[i] = Player2Controls[i].ButtonImage;

            }
        }
        if (Player1) Player1GUI.SetImages(NewGUI);
        else Player2GUI.SetImages(NewGUI);
    }

    public int RandomizeAxis()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                AxisOrientationText.text = "INVERTED AXIS";
                return 0;
            case 1:
                AxisOrientationText.text = "CLOCKWISE ROTATED AXIS";
                return 1;
            case 2:
                AxisOrientationText.text = "COUNTER CLOCKWISE ROTATED AXIS";
                return 2;
            default:
                return 0;

        }
    }

    /*
    public Image[] GetButtonsImages(bool Player1, KeyCode[] ButtonsKeyCodes)
    {
        Image[] ButtonsImages = new Image[Player1Controls.Length];

        for (int i = 0; i < ButtonsImages.Length; i++)
        {
            for (int j = 0; j < Player1Controls.Length; j++)
            {
                if (Player1)
                {
                    if (Player1Controls[j].ButtonKeyCode == ButtonsKeyCodes[i]) ButtonsImages[i] = Player1Controls[j].ButtonImage;
                }
                else
                {
                    if (Player2Controls[j].ButtonKeyCode == ButtonsKeyCodes[i]) ButtonsImages[i] = Player2Controls[j].ButtonImage;
                }
            }
        }
        return ButtonsImages;
    }
    */
}
