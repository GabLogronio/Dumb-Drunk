using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBottle : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("COLLISION");
        if (other.gameObject.layer == 9 || other.gameObject.layer == 10)
        {
            other.gameObject.GetComponent<PlayerObstacleManager>().BlockBalance(10f);
            gameObject.SetActive(false);
        }
    }

}
