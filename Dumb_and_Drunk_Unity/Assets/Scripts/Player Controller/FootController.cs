using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class FootController : LimbController
{
    float FootHeight = 0f, BaseFootHeight = 0.4f, StepForce = 750;

    [Header("RightFoot = true, LeftFoot = false")]
    [SerializeField]
    bool RightFoot;
    bool MovingBack = false;

    [SerializeField]
    PlayerInputManager PlayerController;

    [SerializeField]
    GameObject InstantiableJoint;

    [SerializeField]
    PlayerBalanceManager PlayerBalance;

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

    public override bool UpdateDirection(Vector3 ToSet, bool direction)
    {
        MovingBack = direction;
        return base.UpdateDirection(ToSet, direction);

    }

    void KeepMoving()
    {
        rb.velocity = Vector3.zero;

        CurrentDirection = CurrentDirection.normalized * 0.65f;
        Vector3 FinalDirection = PlayerBalance.GetInitialPosition() - transform.position + CurrentDirection;

        if (FootHeight - transform.position.y >= 0f) FinalDirection.y = FootHeight - transform.position.y;
        else FinalDirection.y = transform.position.y - FootHeight;

        rb.AddForce(FinalDirection * StepForce);

        /*
        if (!MovingBack)
        {
            if (FootHeight - transform.position.y >= 0f) FinalDirection.y = FootHeight - transform.position.y;
            else FinalDirection.y = transform.position.y - FootHeight;
        }
        else
        {
            if (FootHeight / 1f - transform.position.y >= 0f) FinalDirection.y = FootHeight / 1f - transform.position.y;
            else FinalDirection.y = transform.position.y - FootHeight / 1f;

        }
        */
    }

    public override void Detach()
    {
        if (Joint != null) Destroy(Joint);
    }

    // ----------------------------- COLLIDERS -----------------------------

    private void OnCollisionEnter(Collision coll)
    {

        if (coll.gameObject.layer == 14 && Active && Joint == null) // Environment layer
        {
            PlayerController.SetFoot(RightFoot, true);

            FootHeight = transform.position.y + BaseFootHeight;

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
        rb.AddForce(Vector3.up * FootHeight);
        return base.Move();
    }
}
