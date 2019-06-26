using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{
    private static MatchManager instance;
    //cheng, dennis, maurice, mike
    private int[] scores = { 0, 0, 0, 0 };
    public int[] keyCollected = { 0, 0, 0, 0 };
    private int[] teams = { 1, 1, 2, 2 };
    private bool isFirstScene = false;
    private bool isMatchmakingScene = false;
    private int lastPicked = 0;
    private float timer = 0f, FirstSceneDuration = 96.0f, MatchMakingSceneDuration = 5f;
    public GameObject gameCanvas, teamCanvas, loadingCanvas, victoryCanvas;
    private Vector3[] spawnPointsFirstScene = new Vector3[4];
    private Vector3[] spawnPointsSecondScene = new Vector3[4];
    public GameObject[] PlayersGameObjects = new GameObject[4];
    private Vector3[] teamsFacesPos = new Vector3[4];
    private int maxPoints = 4;
    private int maxPlayers = 4;
    public Text CounterText, CountdownText, ScoreText;
    private int teamWin = 1;

    // Start is called before the first frame update
    void Start()
    {
        Debug.developerConsoleVisible = true;

        if (instance == null) instance = this;
        else if (instance != this) Destroy(this);
        DontDestroyOnLoad(gameObject);
        spawnPointsFirstScene[0] = new Vector3(-4.53252f, 3.25f, -9.023758f);
        spawnPointsFirstScene[1] = new Vector3(-1.530971f, 3.25f, -9.023758f);
        spawnPointsFirstScene[2] = new Vector3(1.530971f, 3.25f, -9.023758f);
        spawnPointsFirstScene[3] = new Vector3(4.53252f, 3.25f, -9.023758f);
        spawnPointsSecondScene[0] = new Vector3(-1.468635f, 3.25f, 17.71417f);
        spawnPointsSecondScene[1] = new Vector3(1.468635f, 3.25f, 17.71417f);
        spawnPointsSecondScene[2] = new Vector3(-1.468635f, 3.25f, 90);
        spawnPointsSecondScene[3] = new Vector3(1.468635f, 3.25f, 90);
        teamsFacesPos[0] = new Vector3(-400, 155, 0);
        teamsFacesPos[1] = new Vector3(-100, 155, 0);
        teamsFacesPos[2] = new Vector3(70, -275, 0);
        teamsFacesPos[3] = new Vector3(373, -275, 0);
        DontDestroyOnLoad(gameCanvas.transform.parent.gameObject);
        DontDestroyOnLoad(teamCanvas);
        DontDestroyOnLoad(victoryCanvas);
        for (int i = 0; i < maxPlayers; i++)
        {
            DontDestroyOnLoad(PlayersGameObjects[i]);
        }
        ScoreText.supportRichText = true;
    }



    // Update is called once per frame
    void Update()
    {
        //DebugText.instance.Set("1");
        if (isFirstScene)
        {
            //DebugText.instance.Set("2");
            timer -= Time.deltaTime;

            int ftScore = 0, stScore = 0;
            for (int i = 0; i < maxPlayers; i++)
            {
                if (teams[i] == 1) ftScore += keyCollected[i];
                else stScore += keyCollected[i];
            }
            ScoreText.text = "<color=red>" + ftScore + "</color><color=white> - </color><color=#0099ff>" + stScore + "</color>";

            //DebugText.instance.Set("3");

            if (timer < FirstSceneDuration - 3f)
            {
                CounterText.text = "Get Ready!";
                CountdownText.text = (int)(timer - 60) + "";
                //DebugText.instance.Set("4");
            }
            if (timer < FirstSceneDuration - 5.9f && timer > 0f)
            {
                Spawer.getInstance().StartSpawning();
                CountdownText.text = "";
                CounterText.text = Mathf.Floor(timer).ToString("00");
                //CounterText.text = Mathf.Floor(timer / 60).ToString("00") + ":" + (timer % 60).ToString("00");
                //DebugText.instance.Set("5");
            }
            if (timer <= 0) LoadSecondScene();

        }

        else if (isMatchmakingScene)
        {
            timer -= Time.deltaTime;
            if (timer <= 0) LoadFirstGameScene();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && !isFirstScene && !isMatchmakingScene)
        {
            NetworkServerManager.getInstance().StartGame();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && isFirstScene && !isMatchmakingScene)
        {
            LoadSecondScene();
        }

    }

    public static MatchManager getInstance()
    {
        return instance;
    }

    public void LoadMatchmakingScene()
    {
        AkSoundEngine.SetState("GameState", "None");
        gameCanvas.SetActive(false);
        teamCanvas.SetActive(true);
        for (int i = 0; i < maxPlayers; i++)
        {
            PlayersGameObjects[i].SetActive(false);
            NetworkServerManager.getInstance().SwitchInputManager(i, true);
        }
        teams[0] = Random.Range(1, 3);
        teams[1] = Random.Range(1, 3);
        if (teams[0] == teams[1])
        {
            if (teams[0] == 1)
            {
                teams[2] = 2;
                teams[3] = 2;
            }
            else
            {
                teams[2] = 1;
                teams[3] = 1;
            }
        }
        else
        {
            teams[2] = Random.Range(1, 3);
            if (teams[2] == 1) teams[3] = 2;
            else teams[3] = 1;
        }
        //teams = new int[] { 1, 2, 1, 2 };
        keyCollected = new int[] { 0, 0, 0, 0 };
        SceneManager.LoadScene("Matchmaking Scene");
        //DebugText.instance.Log("Loaded Matchmaking Scene");
        int team1Comp = 0, team2Comp = 0;
        for (int i = 0; i < maxPlayers; i++)
        {
            if (teams[i] == 1)
            {
                teamCanvas.transform.GetChild(1).GetChild(i).gameObject.GetComponent<RectTransform>().localPosition = teamsFacesPos[team1Comp];

                team1Comp++;
            }
            else
            {
                teamCanvas.transform.GetChild(1).GetChild(i).gameObject.GetComponent<RectTransform>().localPosition = teamsFacesPos[team2Comp + 2];
                team2Comp++;
            }
        }
        timer = MatchMakingSceneDuration;
        isMatchmakingScene = true;

    }

    private void LoadFirstGameScene()
    {
        Debug.Log("1 CURRENT " + NetworkServerManager.getInstance().CurrentConnections.Count);
        teamCanvas.SetActive(false);
        gameCanvas.SetActive(true);

        CounterText.text = "";
        timer = FirstSceneDuration;
        isFirstScene = true;
        isMatchmakingScene = false;
        NetworkServerManager.getInstance().ServerStringMessageSenderToAll("Scene1");
        //DebugText.instance.Log("Loaded First Scene");
        SceneManager.LoadScene("Game Scene 1");
        Loading(3f);
        for (int i = 0; i < maxPlayers; i++)
        {
            if (teams[i] == 1)
            {
                gameCanvas.transform.GetChild(i).GetChild(0).GetChild(2).gameObject.SetActive(true);
                gameCanvas.transform.GetChild(i).GetChild(0).GetChild(1).gameObject.SetActive(false);

                PlayersGameObjects[i].transform.GetChild(0).GetChild(2).GetChild(0).gameObject.SetActive(true);
                PlayersGameObjects[i].transform.GetChild(0).GetChild(2).GetChild(1).gameObject.SetActive(false);

            }
            else
            {
                gameCanvas.transform.GetChild(i).GetChild(0).GetChild(2).gameObject.SetActive(false);
                gameCanvas.transform.GetChild(i).GetChild(0).GetChild(1).gameObject.SetActive(true);

                PlayersGameObjects[i].transform.GetChild(0).GetChild(2).GetChild(0).gameObject.SetActive(false);
                PlayersGameObjects[i].transform.GetChild(0).GetChild(2).GetChild(1).gameObject.SetActive(true);
            }


            PlayersGameObjects[i].SetActive(true);
            PlayersGameObjects[i].GetComponent<PlayerInputManager>().Detach();
            Vector3 move = spawnPointsFirstScene[i] - PlayersGameObjects[i].transform.GetChild(2).GetChild(0).position;

            PlayersGameObjects[i].transform.GetChild(2).GetChild(0).GetComponent<PlayerBalanceManager>().SetRandomMoving(true);
            PlayersGameObjects[i].transform.GetChild(2).GetChild(0).GetComponent<PlayerBalanceManager>().RecoverFromFall();
            if(PlayersGameObjects[i].GetComponent<PlayerInputManager>())PlayersGameObjects[i].GetComponent<PlayerInputManager>().Detach();

            PlayersGameObjects[i].transform.position += move;
            PlayersGameObjects[i].transform.rotation = Quaternion.identity;
            gameCanvas.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
            gameCanvas.transform.GetChild(i).GetChild(2).gameObject.SetActive(false);
        }

    }

    public void KeyCollection(int layer)
    {
        keyCollected[layer - 9]++;
        lastPicked = layer - 9;
        // DebugText.instance.Add(layer + " collect key");
    }

    public float getTimer()
    {
        return timer;
    }

    private void LoadSecondScene()
    {
        CountdownText.text = "";
        isFirstScene = false;
        CounterText.text = "";
        int team1Score = 0, team2Score = 0;
        for (int i = 0; i < maxPlayers; i++)
        {
            if (teams[i] == 1) team1Score += keyCollected[i];
            else team2Score += keyCollected[i];
        }
        if (team1Score == team2Score) {
            if (teams[lastPicked] == 1) team1Score++;
            else team2Score++;
        }
        if (team1Score > team2Score)
        {
            scene1End(1);
            teamWin = 1;
        }
        else
        {
            scene1End(2);
            teamWin = 2;
        }
        //DebugText.instance.Log("Loaded Second Scene");
        SceneManager.LoadScene("Game Scene 2");
        Loading(3f);
        AkSoundEngine.PostEvent("OpenGate", gameObject);
    }

    public GameObject[] GetWinnersObjects()
    {
        int count = 0;
        GameObject[] winners = new GameObject[2];
        for (int i = 0; i < maxPlayers; i++)
        {
            if (teams[i] == teamWin)
            {
                winners[count] = PlayersGameObjects[i].transform.GetChild(2).GetChild(0).gameObject;
                count++;
            }
        }
        return winners;
    }

    private void scene1End(int win)
    {
        int winner = 0, loser = 0;
        for (int i = 0; i < maxPlayers; i++)
        {
            PlayersGameObjects[i].transform.GetChild(2).GetChild(0).GetComponent<PlayerBalanceManager>().SetRandomMoving(false);
            if (teams[i] != win)
            {
                NetworkServerManager.getInstance().ServerStringMessageSender(i, "Scene2");
                gameCanvas.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
                gameCanvas.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);
                NetworkServerManager.getInstance().SwitchInputManager(i, false);

                PlayersGameObjects[i].GetComponent<PlayerInputManager>().Detach();
                Vector3 move = spawnPointsSecondScene[loser + 2] - PlayersGameObjects[i].transform.GetChild(2).GetChild(0).position;

                PlayersGameObjects[i].transform.GetChild(2).GetChild(0).GetComponent<PlayerBalanceManager>().ResetForScene2();
                if (PlayersGameObjects[i].GetComponent<PlayerInputManager>()) PlayersGameObjects[i].GetComponent<PlayerInputManager>().Detach();

                PlayersGameObjects[i].transform.position += move;
                PlayersGameObjects[i].transform.rotation = Quaternion.identity;
                loser++;
            }
            else
            {
                if (scores[i] < maxPoints - 1)
                {
                    scores[i]++;
                    gameCanvas.transform.GetChild(i).GetChild(0).GetChild(3).gameObject.GetComponent<UnityEngine.UI.Text>().text = scores[i].ToString();
                }
                PlayersGameObjects[i].GetComponent<PlayerInputManager>().Detach();
                Vector3 move = spawnPointsSecondScene[winner] - PlayersGameObjects[i].transform.GetChild(2).GetChild(0).position;

                PlayersGameObjects[i].transform.GetChild(2).GetChild(0).GetComponent<PlayerBalanceManager>().RecoverFromFall();
                if (PlayersGameObjects[i].GetComponent<PlayerInputManager>()) PlayersGameObjects[i].GetComponent<PlayerInputManager>().Detach();

                PlayersGameObjects[i].transform.position += move;
                PlayersGameObjects[i].transform.rotation = Quaternion.identity;
                winner++;
            }
        }
    }

    public void scene2End(int layer)
    {
        scores[layer - 9]++;
        gameCanvas.transform.GetChild(layer - 9).GetChild(0).GetChild(3).gameObject.GetComponent<UnityEngine.UI.Text>().text = scores[layer - 9].ToString();
        if (scores[layer - 9] >= maxPoints)
        {
            AkSoundEngine.StopAll(gameObject);
            AkSoundEngine.PostEvent("Gate_Opera", gameObject);
            AkSoundEngine.SetSwitch("Music", "Start", gameObject);
            gameCanvas.SetActive(false);
            teamCanvas.SetActive(false);
            victoryCanvas.SetActive(true);
            victoryCanvas.transform.GetChild(layer - 7).gameObject.SetActive(true);
            for (int i = 1; i < 5; i++)
            {
                if (i == layer - 8) NetworkServerManager.getInstance().ServerStringMessageSender(i, "Won");
                else NetworkServerManager.getInstance().ServerStringMessageSender(i, "Lost");
            }

        }
        else LoadMatchmakingScene();
    }

    void Loading(float Duration)
    {
        loadingCanvas.SetActive(true);
        for (int i = 0; i < maxPlayers; i++)
        {
            PlayersGameObjects[i].transform.GetChild(2).GetChild(0).GetComponent<PlayerBalanceManager>().BlockBar(true, Duration + 3f);
            if (PlayersGameObjects[i].GetComponent<PlayerInputManager>()) PlayersGameObjects[i].GetComponent<PlayerInputManager>().BlockControls(true);
        }

        Invoke("FinishLoading", Duration);
    }

    void FinishLoading()
    {
        loadingCanvas.SetActive(false);
        for (int i = 0; i < maxPlayers; i++)
        {
            if (PlayersGameObjects[i].GetComponent<PlayerInputManager>()) PlayersGameObjects[i].GetComponent<PlayerInputManager>().BlockControls(false);
        }

    }
}
