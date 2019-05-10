using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsAndCartSpawner : MonoBehaviour
{
    public static CarsAndCartSpawner instance = null;

    [SerializeField]
    GameObject[] Obstacles;

    [SerializeField]
    Transform[] SpawnPoints;

    int LastSpawned = 10;

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnObstacle();
    }

    public void SpawnObstacle()
    {
        int SelectedObstacle;
        do SelectedObstacle = Random.Range(0, Obstacles.Length - 1);
        while (SelectedObstacle == LastSpawned);
        LastSpawned = SelectedObstacle;
        for (int i = 0; i < Obstacles.Length; i++)
        {
            if (i == SelectedObstacle)
            {
                int SelectedSpawnPoint = Random.Range(0, SpawnPoints.Length - 1);
                Obstacles[i].transform.position = SpawnPoints[SelectedSpawnPoint].position;
                Obstacles[i].transform.rotation = SpawnPoints[SelectedSpawnPoint].rotation;
                Obstacles[i].SetActive(true);

            }
            else Obstacles[i].SetActive(false);

        }

    }

}
