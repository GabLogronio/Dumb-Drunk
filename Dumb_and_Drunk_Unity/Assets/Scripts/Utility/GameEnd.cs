using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            GamePauseMenu.instance.EndGame(true);

        }
        if (collision.gameObject.layer == 10)
        {
            GamePauseMenu.instance.EndGame(false);

        }
    }
}
