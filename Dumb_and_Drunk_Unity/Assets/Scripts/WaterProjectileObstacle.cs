using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterProjectileObstacle : MonoBehaviour
{
    [SerializeField]
    GameObject PuddlePrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
            explode(transform.position);
        }
    }

    private void explode(Vector3 pos)
    {
        AkSoundEngine.PostEvent("ClickGeneral", gameObject);
        Instantiate(PuddlePrefab, new Vector3(pos.x, 0.5f, pos.z), Quaternion.identity);
        Destroy(gameObject);
    }
}
