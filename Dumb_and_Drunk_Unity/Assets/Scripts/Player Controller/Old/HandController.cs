using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class HandController : LimbController
{

    public override bool Detach()
    {
        return true;
    }

    public override bool Move()
    {
        return true;
    }

    public override bool Set(bool ToSet)
    {
        return ToSet;
    }
    /*
    [SerializeField]
    float MoveForce, ActivateDuration = 0.5f, Cooldown = 0.75f;

    [SerializeField]
    GameObject PlayerController;

    Rigidbody rb;
    FixedJoint Joint;

    GameObject OtherPlayer = null;
    bool ActivatedByPlayer = false, OnCooldown = false, Detachable = false, AttachedToAnotherPlayer = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 direction)
    {
        if (AttachedToAnotherPlayer && Detachable)
        {
            Detach();
            OtherPlayer.GetComponent<PlayerObstacleManager>().OtherPlayerInteraction(direction);

        }

        ActivatedByPlayer = true;
        rb.velocity = Vector3.zero;
        rb.AddForce(direction * MoveForce);
        rb.AddForce(transform.up * (PlayerController.transform.position.y - transform.position.y) * MoveForce);
        CancelInvoke();
        Invoke("Deactivate", ActivateDuration);

    }



    /*public void Detach()
    {
        if(Joint != null )Destroy(Joint);
        if (PlayerController.GetComponent<PlayerBarsManager>() != null)
            PlayerController.GetComponent<PlayerBarsManager>().BlockBar(true, false);
        AttachedToAnotherPlayer = false;
        CancelInvoke();
        Invoke("FinishCooldown", Cooldown);        
        //PlayerController.GetComponent<PlayerBarsManager>().UnlockBars();

    }

    private void OnJointBreak(float breakForce)
    {
        Detach();

    }

    void OnCollisionEnter(Collision coll)
    {
        if ((coll.gameObject.layer == 9 || coll.gameObject.layer == 10 || coll.gameObject.layer == 11 || coll.gameObject.layer == 13) && ActivatedByPlayer && !OnCooldown && coll.gameObject.layer != gameObject.layer)
        {
            if (coll.gameObject.layer == 9 || coll.gameObject.layer == 10)
            {
                Detachable = false;
                AttachedToAnotherPlayer = true;
                OtherPlayer = coll.gameObject;

            }

            if (Joint == null)
            {
                Joint = gameObject.AddComponent<FixedJoint>();
                Joint.breakForce = 450f;
                Joint.breakTorque = 450f;
                Rigidbody OtherRb = coll.gameObject.GetComponent<Rigidbody>();
                if (OtherRb != null) Joint.connectedBody = OtherRb;
                if (PlayerController.GetComponent<PlayerBarsManager>() != null)
                    PlayerController.GetComponent<PlayerBarsManager>().BlockBar(true, true);

            }
        }
    }

    public void SetDetachable(bool ToSet)
    {
        Detachable = ToSet;
    }

    void Deactivate()
    {
        ActivatedByPlayer = false;

    }

    void FinishCooldown()
    {
        OnCooldown = false;
    }*/
}
