using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaguetteBoxObstacle : StationaryObstacle
{
    protected override void Activate()
    {

        if (!AlreadyActivated)
        {
            // Breaking box sound

            // Have to see how to make the baguettes

            AlreadyActivated = true;
        }
    }
}
