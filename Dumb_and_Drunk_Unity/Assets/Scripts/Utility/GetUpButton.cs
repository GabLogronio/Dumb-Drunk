using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetUpButton : MonoBehaviour {

    bool InContact = false;

    //[SerializeField]
    //PlayerSpecialStatusManager StatusManager;

    [SerializeField]
    KeyCode ButtonKeyCode, AlternateKeyCode;

    GameObject Other;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if ((Input.GetKeyDown(ButtonKeyCode) || Input.GetKeyDown(AlternateKeyCode)))
        {
            transform.GetChild(0).gameObject.SetActive(true);

            if (InContact)
            {
                //StatusManager.AddGetUpPoint();
                Destroy(Other);

            }

        }

        if ((Input.GetKeyUp(ButtonKeyCode) || Input.GetKeyUp(AlternateKeyCode)))
        {
            transform.GetChild(0).gameObject.SetActive(false);

        }



    }


    private void OnTriggerEnter2D(Collider2D coll)
    {
        Other = coll.gameObject;
        InContact = true;
        coll.gameObject.GetComponent<ButtonMoveUp>().FadeOut();
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        Other = null;
        InContact = false;
    }

}
