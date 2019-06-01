using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceDetector : MonoBehaviour
{
    [SerializeField]
    bool Hor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 15)
            transform.GetComponentInParent<BouncingFace>().ChangeDirection(Hor);
    }

}
