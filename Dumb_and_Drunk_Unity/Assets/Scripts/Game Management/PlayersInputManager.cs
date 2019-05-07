using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersInputManager : MonoBehaviour
{
    public static PlayersInputManager instance;

    [SerializeField]
    PlayerController[] Players;

    // ------------------------------ Analog Axis ------------------------------
    public float P1Hor = 0f, P1Ver = 0f, P2Hor = 0f, P2Ver = 0f, P3Hor = 0f, P3Ver = 0f, P4Hor = 0f, P4Ver = 0f;

    // ------------------------------ Gyroscope Input ------------------------------
    public float P1RotX = 0f, P1RotY = 0f, P1RotZ = 0f, P2RotX = 0f, P2RotY = 0f, P2RotZ = 0f, P3RotX = 0f, P3RotY = 0f, P3RotZ = 0f, P4RotX = 0f, P4RotY = 0f, P4RotZ = 0f;

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void PressedButton(int Player, string Button)
    {
        switch (Player)
        {
            case 1:
                Players[0].ReceivedButton(Button);
                break;
            case 2:
                Players[1].ReceivedButton(Button);
                break;
            case 3:
                Players[2].ReceivedButton(Button);
                break;
            case 4:
                Players[3].ReceivedButton(Button);
                break;
            default: break;

        }
    }

}
