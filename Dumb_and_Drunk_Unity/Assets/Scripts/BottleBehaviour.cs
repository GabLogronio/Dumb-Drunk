using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleBehaviour : MonoBehaviour
{

    public GameObject Puddle;

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
        if (collision.gameObject.layer == 14)
        {
            explode(collision.transform.position);
        }
        else if (collision.gameObject.layer >= 9 && collision.gameObject.layer <= 12)
        {
            collision.gameObject.GetComponent<PlayerObstacleManager>().Fall();
        }
    }

    private void explode(Vector3 pos)
    {
        Instantiate(Puddle, pos, Quaternion.identity);
        Destroy(gameObject);
    }
}
