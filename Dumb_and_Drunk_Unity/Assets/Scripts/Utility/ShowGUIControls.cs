using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowGUIControls : MonoBehaviour {

    [SerializeField]
    GameObject GUIControls;

    [SerializeField]
    KeyCode Control;

	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(Control))
        {
            GUIControls.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(Control))
        {
            GUIControls.SetActive(false);
        }
    }
}
