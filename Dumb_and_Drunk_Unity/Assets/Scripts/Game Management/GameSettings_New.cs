using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings_New : MonoBehaviour
{
    public static GameSettings_New instance;

    public float KeySpawnTime;

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

}
