using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmLights : MonoBehaviour {

    [SerializeField]
    float Time;

    bool TurnedOn = false;

    public void Activate()
    {
        Switch();
    }

    void Switch()
    {
        TurnedOn = !TurnedOn;
        if (TurnedOn) gameObject.SetActive(true);
        else gameObject.SetActive(false);
        Invoke("Switch", Time);
    }

}
