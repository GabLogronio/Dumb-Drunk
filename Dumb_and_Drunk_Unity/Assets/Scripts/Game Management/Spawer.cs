using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawer : MonoBehaviour
{
    private static Spawer instance = null;

    int CurrentKeys = 0;

    [SerializeField]
    Transform[] Spawners;

    [SerializeField]
    GameObject Key;

    float MinSpawnTime = 10f, MaxSpawnTime = 15f;

    private bool[] occupied;
    bool Started = false;
    private float counter = 20;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;
        occupied = new bool[Spawners.Length];
        for (int i = 0; i < occupied.Length; i++) occupied[i] = false;

    }

    public void StartSpawning()
    {
        if (!Started)
        {
            Spawn();
            Spawn();

            Started = true;
        }

    }

    private void Update()
    {
        if (Started)
        {
            if (MatchManager.getInstance().getTimer() >= 0.5f)
            {
                counter -= Time.deltaTime;
                if (counter <= 0f)
                {
                    Spawn();
                    Spawn();
                    counter = 20f;
                }
            }
        }
    }

    public static Spawer getInstance()
    {
        return instance;
    }

    void Spawn()
    {
        if (CurrentKeys <= 8 )
        {
            CurrentKeys++;
            int Spawn1;
            do
            {
                Spawn1 = Random.Range(0, Spawners.Length);
            } while (occupied[Spawn1]);

            Instantiate(Key, Spawners[Spawn1].position, Quaternion.identity).GetComponent<Key>().pos = Spawn1;

            occupied[Spawn1] = true;
        }
    }

    public void KeyCollected(int pos)
    {
        CurrentKeys--;
        //Invoke("Spawn", 5);
        occupied[pos] = false;
    }
}
