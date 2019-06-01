using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class StationaryObstacle : MonoBehaviour {

    [SerializeField]
    protected float PushForce;

    protected bool AlreadyActivated = false;

    protected GameObject target = null;

    // If a players touches the object, a function is called. Can be overwritten
    protected virtual void OnCollisionEnter(Collision collision)
    {

        target = collision.gameObject;

        if (collision.gameObject.layer >= 9 && collision.gameObject.layer <= 12)
        {
            Activate();
        }
    }

    // Each obstacle reacts in its own way- MUST be overwritten
    protected abstract void Activate();
    
}
