using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPotObstacle : StationaryObstacle
{
    // If it touches a player it pushes him back
    protected override void Activate()
    {
        target.GetComponent<Rigidbody>().AddForce(-target.transform.forward * PushForce, ForceMode.Impulse);
    }

    // Overrides. If it touches the ground spills the water. Whatever it hits, it cracks and disappears
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (target.layer == 10) // ENVIRONMENT LAYER
        {
            SplashWater();
        }

        // Play crack sound

        Invoke("Smash", 0.5f);
    }

    // If it touches the ground it spills some water
    private void SplashWater()
    {
        // Gets the first child, the puddle, activates it and releases it from the parent
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).transform.parent = null;
    }

    // Diseappears
    private void Smash()
    {
        gameObject.SetActive(false);
    }
}
