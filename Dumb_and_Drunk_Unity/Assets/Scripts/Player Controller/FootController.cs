using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class FootController : LimbController
{
    float FootHeight = 1f, StepForce = 1000f;

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

    public override bool Release()
    {
        base.Release();
        rb.AddForce(Vector3.down * StepForce / 10f, ForceMode.Impulse);
        return true;
    }

    void KeepMoving()
    {
        rb.velocity = Vector3.zero;

        Vector3 FinalDirection = PlayerBalance.GetInitialPosition() - transform.position + CurrentDirection.normalized * 0.65f;
        if (RightFoot) FinalDirection += PlayerBalance.GetHips().right * 0.2f;
        else FinalDirection -= PlayerBalance.GetHips().right * 0.2f;

        FinalDirection.y = (FootHeight - transform.position.y) * 2f;

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
            ContactPoint contact = coll.contacts[0];
            Joint = Instantiate(InstantiableJoint, new Vector3(contact.point.x, contact.point.y, contact.point.z), coll.gameObject.transform.rotation);
            Joint.GetComponent<HingeJoint>().connectedBody = rb;
            Joint.transform.parent = transform;

        }
        
    }

    private void OnCollisionExit(Collision coll)
    {
        if (Joint == null && coll.gameObject.layer == 14)
        {
            PlayerController.SetFoot(RightFoot, false);
        }
    }

    // ----------------------------- CONTROLS -----------------------------

    public override bool Move()
    {
        if (Joint != null) Destroy(Joint);
        rb.AddForce(Vector3.up * 50f, ForceMode.Impulse);
        return base.Move();
    }
}
