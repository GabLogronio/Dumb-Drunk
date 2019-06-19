using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastWallBehaviour : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer >= 9 && collision.gameObject.layer <= 12)
        {
            MatchManager.getInstance().scene2End(collision.gameObject.layer);
        }
    }
}
