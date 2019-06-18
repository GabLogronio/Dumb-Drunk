using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastWallBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer >= 9 && collision.gameObject.layer <= 12)
        {
            MatchManager.getInstance().scene2End(collision.gameObject.layer);
        }
    }
}
