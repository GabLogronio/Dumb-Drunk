using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [SerializeField]
    Text TimeText;

    [SerializeField]
    float MaxTime = 180f;

    bool Started = false;

    public static GameManager GameManagerInstance = null;

    // Use this for initialization
    void Start () {

        if (GameManagerInstance == null) GameManagerInstance = this;
        else if (GameManagerInstance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

    }

    // Update is called once per frame
    void Update () {

        if (Started)
        {
            MaxTime -= Time.deltaTime;
            TimeText.text = Mathf.Floor(MaxTime / 60).ToString("00") + ":" + (MaxTime % 60).ToString("00");

            //SOUND SET RTPC TIME
            AkSoundEngine.SetRTPCValue("Time", MaxTime, gameObject);

            if (MaxTime <= 0f)
                EndGame();

        }
	}

    void EndGame()
    {
        GamePauseMenu.instance.GameOver();

        //SOUND SET STATE LOSE
        AkSoundEngine.SetState("GameState", "End");

        TimeText.text = "YOU LOSE";

    }

    public void StartTimer()
    {
        Started = true;
    }

    public void AddTime(float ToAdd)
    {
        MaxTime += ToAdd;
    }

    public bool IsStarted()
    {
        return Started;
    }
}
