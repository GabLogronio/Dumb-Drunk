using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {

    int TotalKeys = 3, RemainingKeys = 3;

    [SerializeField]
    GameObject LeftDoor, RightDoor;

    float Speed = 1.5f;
    [SerializeField]
    bool Open, Player1Arrived = false, Player2Arrived = false;

    void Start()
    {
        RemainingKeys = TotalKeys;
    }

    void Update()
    {
        if (Open && Player1Arrived && Player2Arrived)
        {
            if (LeftDoor.transform.localEulerAngles.y > 270f || LeftDoor.transform.localEulerAngles.y== 0f) LeftDoor.transform.Rotate( - Vector3.up * Speed);

            if (RightDoor.transform.localEulerAngles.y < 90f) RightDoor.transform.Rotate(Vector3.up * Speed);

        }
    }

    public void CollectedKey()
    {
        RemainingKeys--;
        if (RemainingKeys == 0)
            OpenGate();
    }

    void OpenGate()
    {
        //SOUND GATE

        Open = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            //other.gameObject.GetComponent<PlayerObstacleManager>().BlockControls(true);
            Player1Arrived = true;
        }
        if (other.gameObject.layer == 10)
        {
            //other.gameObject.GetComponent<PlayerObstacleManager>().BlockControls(true);
            Player2Arrived = true;
        }
        if (Player1Arrived && Player2Arrived && Open) AkSoundEngine.PostEvent("Key", gameObject);

    }

}
