using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiningPeopleObstacle : StationaryObstacle
{

    // If a player gets too close,, he gets pushed away
    protected override void Activate()
    {
        Vector3 PushDirection = target.transform.position - transform.position;
        target.GetComponent<Rigidbody>().AddForce(new Vector3(PushDirection.x, 0, PushDirection.z).normalized * PushForce, ForceMode.Impulse);
    }

}
