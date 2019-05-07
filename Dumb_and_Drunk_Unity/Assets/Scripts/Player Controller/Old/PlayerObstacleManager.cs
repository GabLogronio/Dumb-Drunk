using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PlayerObstacleManager : MonoBehaviour {

    [SerializeField]
    GameObject PlayerController;

    [SerializeField]
    Rigidbody HipsRigidbody;

    Rigidbody OwnRigidbody;

    // Use this for initialization
    void Start () {

        OwnRigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void Taxi(Vector3 TaxiPosition)
    {
        if (!PlayerController.GetComponent<PlayerSpecialStatusManager>().Fallen())
        {
            PlayerController.GetComponent<PlayerSpecialStatusManager>().Fall();
            Vector3 PushDirection = PlayerController.transform.position - TaxiPosition;
            PushDirection = PushDirection.normalized;
            PushDirection.y = 1f;
            HipsRigidbody.velocity = (PushDirection * 100f);

        }

    }

    public void Granny(Vector3 GrannyPosition)
    {
        if (!PlayerController.GetComponent<PlayerSpecialStatusManager>().Fallen())
        {
            PlayerController.GetComponent<PlayerSpecialStatusManager>().Fall();
            Vector3 PushDirection = PlayerController.transform.position - GrannyPosition;
            PushDirection.y = 0f;
            PushDirection = PushDirection.normalized;
            HipsRigidbody.velocity = (PushDirection * 10f);
        }

    }

    public void Puddle(bool ToActivate)
    {
        if (ToActivate && PlayerController.GetComponent<PlayerBarsManager>() != null) PlayerController.GetComponent<PlayerBarsManager>().BalanceBarSpeedModifier(ToActivate);
    }

    public void RotatingSign(Vector3 SignPosition)
    {
        if (!PlayerController.GetComponent<PlayerSpecialStatusManager>().Fallen())
        {
            PlayerController.GetComponent<PlayerSpecialStatusManager>().Fall();
            Vector3 PushDirection = PlayerController.transform.position - SignPosition;
            PushDirection = PushDirection.normalized;
            PushDirection.y = 0.5f;
            HipsRigidbody.velocity = (PushDirection * 150f);
        }

    }

    public GameObject GetPlayerController()
    {
        return gameObject;
    }

    public void EmptyNauseaBar()
    {
        PlayerController.GetComponent<PlayerBarsManager>().EmptyNauseaBar();
    }

    public void BlockControls(bool ToSet)
    {
        PlayerController.GetComponent<PlayerInputController>().BlockControls(ToSet);
    }

    public void Fall()
    {
        // PlayerController.GetComponent<BarsTutorial>().SetBalance(101f);
    }

    public void OtherPlayerInteraction(Vector3 direction)
    {

        if (direction.x > 0f) PlayerController.GetComponent<PlayerBarsManager>().AddBalance(false);
        else PlayerController.GetComponent<PlayerBarsManager>().AddBalance(true);

    }

    public void MovingCart(Vector3 MovingCartPosition)
    {
        if (!PlayerController.GetComponent<PlayerSpecialStatusManager>().Fallen())
        {
            PlayerController.GetComponent<PlayerSpecialStatusManager>().Fall();
            Vector3 PushDirection = PlayerController.transform.position - MovingCartPosition;
            PushDirection.y = 0f;
            PushDirection.z = 0f;
            PushDirection = PushDirection.normalized;
            HipsRigidbody.velocity = (PushDirection * 75f);

        }

    }

}
