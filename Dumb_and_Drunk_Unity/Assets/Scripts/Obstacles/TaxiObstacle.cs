using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxiObstacle : StationaryObstacle
{
    [SerializeField]
    private float AlarmRange;

    [SerializeField]
    Collider[] NearbyPlayer1, NearbyPlayer2;

    [SerializeField]
    AlarmLights Lights;

    public LayerMask Player1Layer, Player2Layer;

    // If a player touches it, the alarm sound goes off and pushes slighlty the players in range away, to simulate the scare
    protected override void Activate()
    {
        if (!AlreadyActivated)
        {
            NearbyPlayer1 = new Collider[1];
            Physics.OverlapSphereNonAlloc(transform.position, AlarmRange, NearbyPlayer1, Player1Layer);
            if (NearbyPlayer1[0] != null)
                NearbyPlayer1[0].gameObject.GetComponent<PlayerObstacleManager>().Taxi(transform.position);

            NearbyPlayer2 = new Collider[1];
            Physics.OverlapSphereNonAlloc(transform.position, AlarmRange, NearbyPlayer2, Player2Layer);
            if (NearbyPlayer2[0] != null) NearbyPlayer2[0].gameObject.GetComponent<PlayerObstacleManager>().Taxi(transform.position);

            AlreadyActivated = true;

            Lights.gameObject.SetActive(true);
            Lights.Activate();
        }
    }
}
