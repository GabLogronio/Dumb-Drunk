using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform Player1, Player2;
    public float distance;

    bool Active = false;

    private void Update()
    {
        if (Active)
        {
            float FurtherBackPlayer = Mathf.Min(Player1.transform.position.z, Player2.transform.position.z);

            if (FurtherBackPlayer - distance > transform.position.z) transform.position = new Vector3(transform.position.x, transform.position.y, FurtherBackPlayer - distance);
        }

    }

    public void Activate()
    {
        BoxCollider BoxColl = GetComponent<BoxCollider>();
        BoxColl.center = new Vector3(BoxColl.center.x, -3.486505f, BoxColl.center.z);

        Active = true;
        distance = (Player1.transform.position.z + Player2.transform.position.z) / 2 - transform.position.z;
    }

}
