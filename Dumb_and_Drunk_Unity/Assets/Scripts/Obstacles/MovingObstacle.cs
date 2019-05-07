using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class MovingObstacle : MonoBehaviour {

    [SerializeField]
    protected float PushForce;

    [SerializeField]
    protected float MovementSpeed;

    protected GameObject target = null;

    protected void Update()
    {
        // Move forward, how? 
    }

    // If a players touches the object, a function is called. Can be overwritten
    protected virtual void OnCollisionEnter(Collision collision)
    {
        target = collision.gameObject;

        if (target.layer == 9 || target.layer == 10) // PLAYER LAYER
        {
            Activate();
        }
    }

    // Each obstacle reacts in its own way- MUST be overwritten
    protected abstract void Activate();


}
