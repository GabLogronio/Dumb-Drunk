using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpecialStatusManager : MonoBehaviour {

    PlayerBarsManager BarsManager;

    PlayerInputController InputController;

    [SerializeField]
    Text CounterText;

    [SerializeField]
    FadingImage[] TextImages;

    [SerializeField]
    GameObject Hips;

    [SerializeField]
    PlayerGUIControlsManager ControlsManager;

    [SerializeField]
    float HelpRange = 2.5f;

    [SerializeField]
    LayerMask OtherPlayerLayer;

    int PointsCounter, RequiredPoints = 5;

    KeyCode[] PlayerControls;

    bool AlternateFootToRecover = true, ThrowingUp = false, HasFallen = false;

    void Start ()
    {
        BarsManager = GetComponent<PlayerBarsManager>();
        InputController = GetComponent<PlayerInputController>();
        PointsCounter = RequiredPoints;

    }

    void Update ()
    {

	}

    public void Fall()
    {
        //SOUND FALL
        AkSoundEngine.PostEvent("YouFell", gameObject);

        HasFallen = true;

        GetComponent<BodyCenter>().Fall();
        BarsManager.EmptyBalanceBar();

        BarsManager.BlockBar(true, true);
        BarsManager.BlockBar(false, true);

        ActivateImage(0);

        PlayerControls = InputController.RerollControls();
        InputController.DetachFeet();
        InputController.DetachHands();
        InputController.BlockControls(true);
        InputController.SetFeetCanAttach(false);

        Hips.GetComponent<SpringJoint>().spring = 0f;
        Hips.GetComponent<Rigidbody>().velocity = Vector3.zero;

        CounterText.text = "" + PointsCounter;

        ControlsManager.StartSpawning();

    }

    void RecoverFromFall()
    {
        HasFallen = false;

        Invoke("ResetBars", 2f);

        GetComponent<BodyCenter>().RecoverFromFall();

        //SOUND GOT UP
        AkSoundEngine.PostEvent("GotUp", gameObject);

        ActivateImage(1);

        InputController.SetFeetCanAttach(true);
        InputController.BlockControls(false);

        Hips.GetComponent<SpringJoint>().spring = 1000f;
        Hips.GetComponent<Rigidbody>().AddForce(Vector3.up * 500f);


    }

    public void StartThrowingUp()
    {
        if (BarsManager.SetThrowingUp(true))
        {
            //SOUND THROW UP
            AkSoundEngine.PostEvent("ThrowingUp", gameObject);

            BarsManager.BlockBar(true, true);
            InputController.BlockControls(true);
            GetComponent<BodyCenter>().ThrowUp();

            ActivateImage(2);
        }

    }

    void ThrowUp()
    {

    }

    public void RecoverFromThrowUp()
    {
        //SOUND Recover
        AkSoundEngine.PostEvent("Recover", gameObject);

        BarsManager.BlockBar(true, false);
        InputController.BlockControls(false);
        GetComponent<BodyCenter>().StopThrowingUp();

        BarsManager.SetThrowingUp(false);

    }

    void ActivateImage(int i)
    {
        if (i < TextImages.Length)
        {
            TextImages[i].Activate();
            Vector3 position = Camera.main.WorldToScreenPoint(this.transform.position + (Vector3.up * 2f));
            TextImages[i].transform.position = position;
        }
    }

    public void AddGetUpPoint()
    {

        PointsCounter--;

        CounterText.text = "" + PointsCounter;

        //SOUND SET RTPC TIME
        AkSoundEngine.SetRTPCValue("GettinUp", PointsCounter, gameObject);

        if (PointsCounter <= 0)
        {
            ControlsManager.StopSpawning();
            PointsCounter = RequiredPoints;
            RecoverFromFall();
            CounterText.text = "";
        } 
    }

    void ResetBars()
    {
        BarsManager.BlockBar(true, false);
        BarsManager.BlockBar(false, false);
    }

    public bool Fallen()
    {
        return HasFallen;
    }
}