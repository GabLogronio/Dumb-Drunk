using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawer : MonoBehaviour
{
    [SerializeField]
    Transform[] Spawners;

    [SerializeField]
    GameObject Key1, Key2;

    float MinSpawnTime = 10f, MaxSpawnTime = 15f;

    bool Activated = true;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Spawn", 3f);
    }

    void Spawn()
    {
        if (Activated)
        {
            int Spawn1, Spawn2;
            //float RespawnTime = Random.Range(MinSpawnTime, MaxSpawnTime);
            Spawn1 = Random.Range(0, Spawners.Length);
            do Spawn2 = Random.Range(0, Spawners.Length); while (Spawn2 == Spawn1);

            Key1.transform.position = Spawners[Spawn1].position;
            Key2.transform.position = Spawners[Spawn2].position;
            Key1.SetActive(true);
            Key2.SetActive(true);

            Invoke("Spawn", (30f));
        }
    }
}
