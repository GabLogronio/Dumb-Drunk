using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PlayerObstacleManager : MonoBehaviour {

    [SerializeField]
    PlayerBalanceManager BalanceManager;

    [SerializeField]
    Rigidbody HipsRigidbody;

    Rigidbody OwnRigidbody;

    // Use this for initialization
    void Start () {

        OwnRigidbody = GetComponent<Rigidbody>();

    }

    public void Taxi(Vector3 TaxiPosition)
    {
        if (!BalanceManager.HasFallen())
        {
            ResetRigidbodiesVelocity();
            Vector3 PushDirection = BalanceManager.transform.position - TaxiPosition;
            PushDirection = PushDirection.normalized;
            PushDirection.y = 1f;
            BalanceManager.Fall(PushDirection * 200f);

        }
    }

    public void Granny(Vector3 GrannyPosition)
    {
        if (!BalanceManager.HasFallen())
        {
            ResetRigidbodiesVelocity();
            Vector3 PushDirection = BalanceManager.transform.position - GrannyPosition;
            PushDirection = PushDirection.normalized;
            PushDirection.y = 0f;
            BalanceManager.Fall(PushDirection * 150f);

        }

    }

    public void RotatingSign(Vector3 SignPosition)
    {
        if (!BalanceManager.HasFallen())
        {
            ResetRigidbodiesVelocity();
            Vector3 PushDirection = BalanceManager.transform.position - SignPosition;
            PushDirection = PushDirection.normalized;
            PushDirection.y = 0.5f;
            BalanceManager.Fall(PushDirection * 300f);

        }

    }

    public void MovingCart(Vector3 MovingCartPosition)
    {
        if (!BalanceManager.HasFallen())
        {
            ResetRigidbodiesVelocity();
            Vector3 PushDirection = BalanceManager.transform.position - MovingCartPosition;
            PushDirection.y = 0.5f;
            PushDirection = PushDirection.normalized;
            BalanceManager.Fall(PushDirection * 300f);

        }
    }

    public void BlockControls(bool ToSet)
    {
        BalanceManager.gameObject.GetComponent<PlayerInputController>().BlockControls(ToSet);
    }

    public void Fall()
    {
        BalanceManager.Fall();
    }

    public void OtherPlayerInteraction(Vector3 direction)
    {
        //if (direction.x > 0f) PlayerController.GetComponent<PlayerBarsManager>().AddBalance(false);
        //else PlayerController.GetComponent<PlayerBarsManager>().AddBalance(true);

    }

    void ResetRigidbodiesVelocity()
    {
        foreach (Transform child in transform.parent)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null) rb.velocity = Vector3.zero;

        }
    }

    public void BlockBalance(float Timer)
    {
        BalanceManager.BlockBar(true, Timer);
    }

    public PlayerBalanceManager GetPlayerController()
    {
        return BalanceManager;
    }
}
