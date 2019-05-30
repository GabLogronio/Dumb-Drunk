using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleObstacle : MonoBehaviour
{

    [SerializeField]
    private float SpreadSpeed, MaxScale;

    int Player1BodyCounts = 0, Player2BodyCounts = 0;

    bool Spread = false;

    public void Activate()
    {
        Spread = true;
    }

    public void StopSpread()
    {
        Spread = false;
    }

    // Increases balance bar speed
    private void OnTriggerEnter(Collider other)
    {
        // Fall

    }
	
	// Keeps expanding untill a maximum size is reached
	void Update () {

        if(transform.localScale.x < MaxScale && Spread)
        transform.localScale += new Vector3(SpreadSpeed * Time.deltaTime, 0, SpreadSpeed * Time.deltaTime);

    }

}
