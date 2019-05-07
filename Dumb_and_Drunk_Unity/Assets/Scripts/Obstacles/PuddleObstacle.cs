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
        if (other.gameObject.layer == 9)
        {
            Player1BodyCounts++;
            if (Player1BodyCounts == 1) other.gameObject.GetComponent<PlayerObstacleManager>().Puddle(true);
        }
        if (other.gameObject.layer == 10)
        {
            Player2BodyCounts++;
            if (Player2BodyCounts == 1) other.gameObject.GetComponent<PlayerObstacleManager>().Puddle(true);
        }

    }

    // Decreases balance bar speed
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            Player1BodyCounts--;
            if (Player1BodyCounts == 0) other.gameObject.GetComponent<PlayerObstacleManager>().Puddle(false);
        }
        if (other.gameObject.layer == 10)
        {
            Player2BodyCounts--;
            if (Player2BodyCounts == 0) other.gameObject.GetComponent<PlayerObstacleManager>().Puddle(false);
        }
    }
	
	// Keeps expanding untill a maximum size is reached
	void Update () {

        if(transform.localScale.x < MaxScale && Spread)
        transform.localScale += new Vector3(SpreadSpeed * Time.deltaTime, 0, SpreadSpeed * Time.deltaTime);

    }

}
