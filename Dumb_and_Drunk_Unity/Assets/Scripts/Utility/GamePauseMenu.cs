using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauseMenu : MonoBehaviour {

    [SerializeField]
    GameObject PauseMenu, Player1Wins, Player2Wins, YouLose;

    public static GamePauseMenu instance = null;

    bool Paused = false;

    void Start()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    // Update is called once per frame
    void Update () {

        if((Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.Joystick2Button7)) && GameManager.GameManagerInstance.IsStarted()) Pause();

    }

    void Pause()
    {
        Paused = !Paused;
        if (Paused)
        {
            Time.timeScale = 0f;
            PauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            PauseMenu.SetActive(false);

        }
    }

    public void EndGame(bool Player1)
    {
        Time.timeScale = 0f;

        if(Player1) Player1Wins.SetActive(true);
        else Player2Wins.SetActive(true);

    }

    public void GameOver()
    {
        Time.timeScale = 0f;

        YouLose.SetActive(true);
    }
}
