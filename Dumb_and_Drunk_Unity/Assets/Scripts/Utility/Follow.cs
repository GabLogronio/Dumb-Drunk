using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {

    [SerializeField]
    Transform PlayerController, Camera;

    [SerializeField]
    float Height;

    [SerializeField]
    bool LookCamera;

	// Update is called once per frame
	void Update () {

        transform.position = new Vector3(PlayerController.position.x, Height, PlayerController.position.z);
        if (LookCamera) transform.LookAt(Camera);
	}
}
