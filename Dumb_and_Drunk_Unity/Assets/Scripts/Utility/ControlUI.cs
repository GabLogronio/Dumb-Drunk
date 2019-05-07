using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlUI : MonoBehaviour {

    [SerializeField]
    Vector3 RelativePosition;

    [SerializeField]
    GameObject Limb;

    [SerializeField]
    Image ButtonImage;
	
	// Update is called once per frame
	void Update () {

        transform.position = Limb.transform.position + RelativePosition;
        Vector3 ImagePosition = Camera.main.WorldToScreenPoint(this.transform.position);
        ButtonImage.transform.position = ImagePosition;

    }

    public void SetImage(Image ImageToSet)
    {
        ButtonImage = ImageToSet;
    }

}
