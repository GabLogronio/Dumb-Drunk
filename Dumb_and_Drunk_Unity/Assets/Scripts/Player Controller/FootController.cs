using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class FootController : LimbController
{
    [Header("Movement parameters")]
    [SerializeField]
    float FootHeight, StepForce, RotationForce;

    [Header("RightFoot = true, LeftFoot = false")]
    [SerializeField]
    bool RightFoot;
    bool MovingBack = false;

    [SerializeField]
    PlayerInputManager PlayerController;

    [SerializeField]
    GameObject InstantiableJoint;

    Rigidbody rb;
    GameObject Joint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Active = true;
    }

    // ----------------------------- UPDATES -----------------------------

    private void Update()
    {
        if (Moving) KeepMoving();

    }

    public override bool UpdateDirection(Vector2 ToSet, bool direction)
    {
        MovingBack = direction;
        return base.UpdateDirection(ToSet, direction);

    }

    void KeepMoving()
    {
        rb.velocity = Vector3.zero;

        CurrentDirection = CurrentDirection.normalized * 0.5f;
        Vector3 FinalDirection = PlayerController.gameObject.transform.position - transform.position + new Vector3(CurrentDirection.x, 0f, CurrentDirection.y);
        if (RightFoot) FinalDirection += PlayerController.gameObject.transform.right * 0.15f;
        else FinalDirection -= PlayerController.gameObject.transform.right * 0.15f;

        if (!MovingBack)
        {
            if (FootHeight - transform.position.y >= 0f) FinalDirection.y = FootHeight - transform.position.y;
            else FinalDirection.y = transform.position.y - FootHeight;
        }
        else
        {
            if (FootHeight / 2f - transform.position.y >= 0f) FinalDirection.y = FootHeight / 2f - transform.position.y;
            else FinalDirection.y = transform.position.y - FootHeight / 2f;

        }

        rb.AddForce(FinalDirection * StepForce);
    }

    // ----------------------------- COLLIDERS -----------------------------

    private void OnCollisionEnter(Collision coll)
    {

        if (coll.gameObject.layer == 14 && Active && Joint == null) // Environment layer
        {
            PlayerController.SetFoot(RightFoot, true);

            ContactPoint contact = coll.contacts[0];
            Joint = Instantiate(InstantiableJoint, new Vector3(contact.point.x, contact.point.y, contact.point.z), coll.gameObject.transform.rotation);
            Joint.GetComponent<HingeJoint>().connectedBody = rb;
            Joint.transform.parent = transform;

        }
        
    }

    private void OnCollisionExit(Collision coll)
    {
        if (Joint == null && coll.gameObject.layer == 11)
        {
            PlayerController.SetFoot(RightFoot, false);
        }
    }

    // ----------------------------- CONTROLS -----------------------------

    public override bool Move()
    {
        if (Joint != null) Destroy(Joint);
        return base.Move();
    }
}
