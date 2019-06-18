using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawer : MonoBehaviour
{
    private static Spawer instance = null;

    [SerializeField]
    Transform[] Spawners;

    [SerializeField]
    GameObject Key;

    float MinSpawnTime = 10f, MaxSpawnTime = 15f;

    bool Activated = true;

    private bool[] occupied = new bool[10];
    private float counter = 20;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;
        for (int i = 0; i < 10; i++) occupied[i] = false;
        Spawn();
        Spawn();
    }

    private void Update()
    {
        if (MatchManager.getInstance().getTimer() >= 0.5f)
        {
            counter -= Time.deltaTime;
            if (counter <= 0)
            {
                Spawn();
                counter = 20;
            }
        }
    }

    public static Spawer getInstance()
    {
        return instance;
    }

    void Spawn()
    {
        if (Activated)
        {
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
        Invoke("Spawn", 5);
        occupied[pos] = false;
    }
}
