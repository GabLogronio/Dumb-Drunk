using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleObstacle : MonoBehaviour
{
    [SerializeField]
    private float SpreadSpeed, MaxScale, SinkSpeed;

    bool Spread = false, Disappear = false;

    public void Start()
    {
        Spread = true;
        Invoke("StartDisappearing", 5f);
        Invoke("DestroyThis", 6.5f);
    }

    public void StopSpread()
    {
        Spread = false;
    }

    // Increases balance bar speed
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer >= 9 && other.gameObject.layer <= 12)
        {
            other.gameObject.GetComponent<PlayerObstacleManager>().Fall();
            Destroy(gameObject);
        }
    }
	
	// Keeps expanding untill a maximum size is reached
	void Update () {

        if(transform.localScale.x < MaxScale && Spread)
            transform.localScale += new Vector3(SpreadSpeed * Time.deltaTime, 0, SpreadSpeed * Time.deltaTime);
        if (Disappear)
            transform.Translate(-Vector3.up * Time.deltaTime * SinkSpeed);

    }
    void StartDisappearing()
    {
        Disappear = true;
    }

    void DestroyThis()
    {
        Destroy(gameObject);
    }

}
