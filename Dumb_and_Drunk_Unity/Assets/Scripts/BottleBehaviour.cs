using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleBehaviour : MonoBehaviour
{

    public GameObject Puddle;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 14)
        {
            AkSoundEngine.PostEvent("Bottle_Break", gameObject);
            explode(transform.position);

        }
        else if (collision.gameObject.layer >= 9 && collision.gameObject.layer <= 12)
        {
            collision.gameObject.GetComponent<PlayerObstacleManager>().Fall();
            AkSoundEngine.PostEvent("Bottle_Break", gameObject);
            Destroy(gameObject);
        }
    }

    private void explode(Vector3 pos)
    {
        Instantiate(Puddle, new Vector3 (pos.x, 0.5f, pos.z), Quaternion.identity);
        Destroy(gameObject);
    }
}
