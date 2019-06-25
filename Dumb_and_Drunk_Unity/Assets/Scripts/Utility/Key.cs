using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField]
    float RotationSpeed = 5f;

    public int pos;

    bool Activated = false;

    private void Update()
    {
        transform.Rotate(Vector3.up * RotationSpeed);
        if (MatchManager.getInstance().getTimer() <= 0.5f)
        {
            DestroyKey();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!Activated && collision.gameObject.layer >= 9 && collision.gameObject.layer <= 12) // Player Layer
        {
            Activated = true;
            AkSoundEngine.PostEvent("Key", gameObject);
            MatchManager.getInstance().KeyCollection(collision.gameObject.layer);
            Spawer.getInstance().KeyCollected(pos);
            DestroyKey();
        }
    }

    void DestroyKey()
    {
        Destroy(gameObject);
    }
}
