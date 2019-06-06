using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{
    private static MatchManager instance;
    //cheng, dennis, maurice, mike
    private int[] scores = { 0, 0, 0, 0 };
    private int[] keyCollected = { 0, 0, 0, 0 };
    private int[] teams = { 1, 1, 2, 2 };
    private bool isFirstScene = false;
    private bool isMatchmakingScene = false;
    private int lastPicked = 0;
    private float timer = 30.0f;
    public GameObject gameCanvas;
    public GameObject teamCanvas;
    private Vector3[] spawnPointsFirstScene = new Vector3[4];
    private Vector3[] spawnPointsSecondScene = new Vector3[4];
    public GameObject[] PlayersGameObjects = new GameObject[4];
    private Vector3[] teamsFacesPos = new Vector3[4];
    private int maxPoints = 10;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this);
        DontDestroyOnLoad(gameObject);
        spawnPointsFirstScene[0] = new Vector3(-2, 0, -6.66f);
        spawnPointsFirstScene[1] = new Vector3(-0.75f, 0, -5.66f);
        spawnPointsFirstScene[2] = new Vector3(0.75f, 0, -5.66f);
        spawnPointsFirstScene[3] = new Vector3(2, 0, -6.66f);
        spawnPointsSecondScene[0] = new Vector3(-1.2f, 0, -1);
        spawnPointsSecondScene[1] = new Vector3(1.2f, 0, -1);
        spawnPointsSecondScene[2] = new Vector3(-2, 0, 0);
        spawnPointsSecondScene[3] = new Vector3(2, 0, 0);
        teamsFacesPos[0] = new Vector3(-400, 155, 0);
        teamsFacesPos[1] = new Vector3(-100, 155, 0);
        teamsFacesPos[2] = new Vector3(70, -275, 0);
        teamsFacesPos[3] = new Vector3(373, -275, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFirstScene || isMatchmakingScene)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                if (isFirstScene) LoadSecondScene();
                else if (isMatchmakingScene) LoadFirstGameScene();
            }
        }
    }

    public static MatchManager getInstance()
    {
        return instance;
    }

    public void LoadMatchmakingScene()
    {
        gameCanvas.SetActive(false);
        teamCanvas.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
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
        SceneManager.LoadScene("Matchmaking Scene");
        int team1Comp = 0, team2Comp = 0;
        for (int i = 0; i < 4; i++)
        {
            if (teams[i] == 1)
            {
                teamCanvas.transform.GetChild(3).GetChild(i).gameObject.GetComponent<RectTransform>().localPosition = teamsFacesPos[team1Comp];
                team1Comp++;
            }
            else
            {
                teamCanvas.transform.GetChild(3).GetChild(i).gameObject.GetComponent<RectTransform>().localPosition = teamsFacesPos[team2Comp + 2];
                team2Comp++;
            }
        }
        timer = 5.0f;
        isMatchmakingScene = true;
    }

    private void LoadFirstGameScene()
    {
        teamCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        timer = 30.0f;
        isFirstScene = true;
        NetworkServerManager.getInstance().ServerStringMessageSenderToAll("Scene1");
        SceneManager.LoadScene("Game Scene 1");
        for (int i = 0; i < 4; i++)
        {
            Vector3 move = spawnPointsFirstScene[i] - PlayersGameObjects[i].transform.GetChild(2).GetChild(0).position;
            PlayersGameObjects[i].transform.position += move;
            PlayersGameObjects[i].transform.rotation = Quaternion.identity;
        }
    }

    public void KeyCollected(int layer)
    {
        keyCollected[layer - 9]++;
        lastPicked = layer - 9;
    }

    private void LoadSecondScene()
    {
        isFirstScene = false;
        int team1Score = 0, team2Score = 0;
        for (int i = 0; i < 4; i++)
        {
            if (teams[i] == 1) team1Score += keyCollected[i];
            else team2Score += keyCollected[i];
        }
        if (team1Score == team2Score) {
            if (teams[lastPicked] == 1) team1Score++;
            else team2Score++;
        }
        if (team1Score > team2Score) scene1End(1);
        else scene1End(2);
        NetworkServerManager.getInstance().ServerStringMessageSenderToAll("Scene2");
        SceneManager.LoadScene("Game Scene 2");
    }

    private void scene1End(int win)
    {
        int winner = 0, loser = 0;
        for (int i = 0; i < 4; i++)
        {
            if (teams[i] != win)
            {
                gameCanvas.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
                gameCanvas.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);
                NetworkServerManager.getInstance().SwitchInputManager(i, false);

                Vector3 move = spawnPointsSecondScene[loser + 2] - PlayersGameObjects[i].transform.GetChild(2).GetChild(0).position;
                PlayersGameObjects[i].transform.position += move;
                PlayersGameObjects[i].transform.rotation = Quaternion.identity;
                loser++;
            }
            else
            {
                if (scores[i] < maxPoints - 1) scores[i]++;
                Vector3 move = spawnPointsSecondScene[winner] - PlayersGameObjects[i].transform.GetChild(2).GetChild(0).position;
                PlayersGameObjects[i].transform.position += move;
                PlayersGameObjects[i].transform.rotation = Quaternion.identity;
                winner++;
            }
        }
    }

    public void scene2End(int layer)
    {
        scores[layer - 9]++;
        if (scores[layer - 9] >= maxPoints) SceneManager.LoadScene("Victory Scene");
        else LoadMatchmakingScene();
    }
}
