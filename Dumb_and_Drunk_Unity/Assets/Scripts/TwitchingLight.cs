using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchingLight : MonoBehaviour
{
    float Min = 5f, Max = 10f, ChangeTime;

    bool LightOn = true;

    // Start is called before the first frame update
    void Start()
    {
        if (Random.value > 0.5f)
        {
            LightOn = true;
        }
        else
        {
            LightOn = false;
        }
        ChangeTime = Random.Range(Min, Max);
        Invoke("SwitchLight", ChangeTime);

    }

    void SwitchLight()
    {
        LightOn = !LightOn;
        gameObject.SetActive(LightOn);
        Invoke("SwitchLight", ChangeTime);

    }

}
