using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkActivateOnDistance : MonoBehaviour {

    public string Event;

    public GameObject Camera;

    float Range = 10f;

    bool Activated = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (Vector3.Distance(transform.position, Camera.transform.position) <= Range)
        {
            if (Activated == false)
            {
                AkSoundEngine.PostEvent(Event, gameObject);
            }
            Activated = true;
            
        }
        else
        {
            if (Activated == true)
            {
                AkSoundEngine.StopAll(gameObject);
            }
            Activated = false;

        }

    }
}
