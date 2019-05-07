using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHydrantObstacle : StationaryObstacle
{
    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);

    }

    // If a player hits it, it spills water OR 
    protected override void Activate()
    {

        if (!AlreadyActivated)
        {
            SplashWater();

            AlreadyActivated = true;
        }
    }

    // Gets the first child, the puddle, activates it and releases it from the parent
    private void SplashWater()
    {
        Debug.Log("FireHydrant");
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).transform.parent = null;
    }

}