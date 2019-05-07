using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsAndCartSpawner : MonoBehaviour
{
    public static CarsAndCartSpawner instance = null;

    [SerializeField]
    GameObject[] Obstacles;

    [SerializeField]
    Transform SpawnPoint1, SpawnPoint2;

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
        int SelectedObstalce = Random.Range(0, Obstacles.Length - 1);
        if (Random.value > 0.5f)
        {
            Obstacles[SelectedObstalce].transform.position = SpawnPoint1.position;
            Obstacles[SelectedObstalce].transform.rotation = SpawnPoint1.rotation;

        }
        else
        {
            Obstacles[SelectedObstalce].transform.position = SpawnPoint2.position;
            Obstacles[SelectedObstalce].transform.rotation = SpawnPoint2.rotation;

        }
        Obstacles[SelectedObstalce].SetActive(true);

    }

}
