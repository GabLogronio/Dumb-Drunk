using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField]
    float RotationSpeed = 5f;

    public int pos;

    private void Update()
    {
        transform.Rotate(Vector3.up * RotationSpeed);
        if (MatchManager.getInstance().getTimer() <= 0.5f)
        {
            CancelInvoke();
            DestroyKey();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer >= 9 && collision.gameObject.layer <= 12) // Player Layer
        {
            MatchManager.getInstance().KeyCollection(collision.gameObject.layer);
            Spawer.getInstance().KeyCollected(pos);
            CancelInvoke();
            DestroyKey();
        }
    }

    void DestroyKey()
    {
        Destroy(gameObject);
    }
}
