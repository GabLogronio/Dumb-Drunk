using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {

    [SerializeField]
    Transform PlayerController;

    [SerializeField]
    float height;

	// Update is called once per frame
	void Update () {

        transform.position = new Vector3(PlayerController.position.x, height, PlayerController.position.z);
	}
}
