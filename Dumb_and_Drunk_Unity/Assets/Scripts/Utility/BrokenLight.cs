using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenLight : MonoBehaviour {

    [SerializeField]
    float MinOn, MaxOn, MinOff, MaxOff;

    [SerializeField]
    GameObject OnFrame, OffFrame;

	// Use this for initialization
	void Start () {

        OnFrame.SetActive(true);
        OffFrame.SetActive(false);

        Invoke("SwitchOff", Random.Range(MinOn, MaxOn));
	}
	
	// Update is called once per frame
	void SwitchOff ()
    {

        OnFrame.SetActive(false);
        OffFrame.SetActive(true);
        Invoke("SwitchOn", Random.Range(MinOff, MaxOff));

    }

    void SwitchOn()
    {

        OnFrame.SetActive(true);
        OffFrame.SetActive(false);
        Invoke("SwitchOff", Random.Range(MinOn, MaxOn));

    }
}
