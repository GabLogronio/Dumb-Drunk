using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class HandController : LimbController
{
    [SerializeField]
    float MoveForce, ActivateDuration = 0.5f;

    [SerializeField]
    GameObject BodyCenter;

    bool Detachable = false;

    FixedJoint Joint;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Active = false;
    }

    // ----------------------------- UPDATES -----------------------------

    private void Update()
    {
        if (Moving) KeepMoving();
        else rb.velocity = Vector3.zero;
    }

    public void KeepMoving()
    {
        Active = true;
        CancelInvoke();
        Invoke("Deactivate", ActivateDuration);

        rb.velocity = Vector3.zero;
        rb.AddForce(new Vector3(CurrentDirection.x, 0f, CurrentDirection.y) * MoveForce);
        rb.AddForce(transform.up * (BodyCenter.transform.position.y - transform.position.y) * MoveForce);

    }

    void Deactivate()
    {
        Active = false;
    }

    public override bool Move()
    {
        if (Joint != null) Destroy(Joint);
        return base.Move();
    }

    public override void Detach()
    {
        if (Joint != null) Destroy(Joint);
    }

    // ----------------------------- COLLIDERS -----------------------------

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.layer == 14 && Active)
        {
            if (Joint == null)
            {
                Joint = gameObject.AddComponent<FixedJoint>();
                Joint.breakForce = 450f;
                Joint.breakTorque = 450f;
                Rigidbody OtherRb = coll.gameObject.GetComponent<Rigidbody>();
                if (OtherRb != null) Joint.connectedBody = OtherRb;

            }
        }
    }

}
