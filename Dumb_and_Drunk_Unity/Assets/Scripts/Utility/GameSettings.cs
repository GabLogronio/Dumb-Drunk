using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour {

    public enum DifficultyLevel { Easy, Normal, Hard };

    [Header("The game difficulty selected")]
    public DifficultyLevel Player1Difficulty, Player2Difficulty;

    [Header("The axis relation selected")]
    public bool Player1GlobalAxis, Player2GlobalAxis;

    public int AxisOrientation;

    public static GameSettings SettingsInstance = null;

    void Start() {

        if (SettingsInstance == null) SettingsInstance = this;
        else if (SettingsInstance != this) Destroy(gameObject);

    }

    public void Setup()
    {
        if (Player1Difficulty == DifficultyLevel.Hard || Player2Difficulty == DifficultyLevel.Hard)
            AxisOrientation = ControlsRandomizer.RandomizerInstance.RandomizeAxis();
    }

}
