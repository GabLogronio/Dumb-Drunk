using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {

    [SerializeField]
    Transform RightFoot, LeftFoot;

    [SerializeField]
    float height;

	// Update is called once per frame
	void Update () {

        transform.position = new Vector3((RightFoot.position.x + LeftFoot.position.x)/2, height, (RightFoot.position.z + LeftFoot.position.z) / 2);
        //transform.position = new Vector3(PlayerController.position.x, height, PlayerController.position.z);

	}
}
