using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBalanceManager : MonoBehaviour {

    bool Blocked = false;

    // Random Wander parameters
    float MinChangeTime = 0f, MaxChangeTime = 1.5f, MinAngularAcc = 0.001f, MaxAngularAcc = 0.01f,
          X = 0f, Y = 0f, ChangeTime = 0f, CurrentSpeed = 0f, CurrentAngular = 0f;

    float RightTimer = 0f, LeftTimer = 0f, UpTimer = 0f, DownTimer = 0f, TimerChanger = 2.5f;

    [SerializeField]
    GameObject RightFoot, LeftFoot, Hips;

    [SerializeField]
    PlayerInputManager InputController;

    [SerializeField]
    float BodyLength = 0.55f, LegsLength = 0.85f, Forward = 0.1f, Speed = 1f;

    Vector3 BalanceModifier = Vector3.zero;

    bool Fallen = false;

    private void Start()
    {
        RandomizeDirection();

    }

    public void Fall()
    {
        Fallen = true;

        BlockBar(true);

        InputController.Detach();
        InputController.BlockControls(true);
        InputController.SetCanAttach(false);

        Hips.GetComponent<SpringJoint>().spring = 0f;
        Hips.GetComponent<Rigidbody>().velocity = Vector3.zero;

        NetworkServerManager.getInstance().ServerStringMessageSender(InputController, "Fallen");
    }

    public void Fall(Vector3 direction)
    {
        Fall();
        Hips.GetComponent<Rigidbody>().velocity = direction;
    }

    public void RecoverFromFall()
    {
        Fallen = false;

        //SOUND GOT UP
        //AkSoundEngine.PostEvent("GotUp", gameObject);

        InputController.SetCanAttach(true);
        InputController.BlockControls(false);

        Hips.GetComponent<SpringJoint>().spring = 1000f;
        Hips.GetComponent<Rigidbody>().AddForce(Vector3.up * 500f);

        transform.localPosition = new Vector3(0f, LegsLength + BodyLength, 0f);
        RightTimer = 0f; LeftTimer = 0f; UpTimer = 0f; DownTimer = 0f;


        NetworkServerManager.getInstance().ServerStringMessageSender(InputController, "GotUp");

    }

    public bool HasFallen()
    {
        return Fallen;
    }

    public void BlockBar(bool ToSet)
    {
        Blocked = ToSet;
    }

    public void BlockBar(bool ToSet, float Timer)
    {
        Blocked = ToSet;
        Invoke("UnlockBar", Timer);
    }

    void UnlockBar()
    {
        Blocked = false;
    }

    void Update()
    {
        if (!Fallen)
        {
            UpdateTimers();

            float Height = Mathf.Sqrt(LegsLength * LegsLength - Mathf.Pow((Vector3.Distance(RightFoot.transform.position, LeftFoot.transform.position) / 2f), 2)) + BodyLength;

            if (new Vector2(transform.localPosition.x, transform.localPosition.z).magnitude < 0.375f) // NOT FALLEN
            {
                Vector3 BalanceModifier = Vector3.right * (RightTimer - LeftTimer) + Vector3.forward * (UpTimer - DownTimer);

                if (!Blocked && !float.IsNaN(Height) && !float.IsNaN(BalanceModifier.x) && !float.IsNaN(BalanceModifier.y) && !float.IsNaN(BalanceModifier.z) && transform.position.y > RightFoot.transform.position.y && transform.position.y > LeftFoot.transform.position.y)
                {
                    transform.position = new Vector3(transform.position.x, Height, transform.position.z);
                    //transform.Translate(transform.forward * Time.deltaTime * CurrentSpeed);
                    transform.Translate(BalanceModifier * Time.deltaTime, Space.World);
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(X, 0f, Y)), CurrentAngular);
                    // + transform.right * Vector3.Distance(RightFoot.transform.position, LeftFoot.transform.position) * BalanceValue / 200;
                }
            }
            else
            {
                Fall();
            }
        }
    }

    void UpdateTimers()
    {
        if (RightTimer > 0f) RightTimer -= Time.deltaTime;
        if (LeftTimer > 0f) LeftTimer -= Time.deltaTime;
        if (UpTimer > 0f) UpTimer -= Time.deltaTime;
        if (DownTimer > 0f) DownTimer -= Time.deltaTime;

    }

    public void MoveBodyCenter(char Direction)
    {
        switch (Direction)
        {
            case 'R':
                RightTimer = TimerChanger;
                break;
            case 'L':
                LeftTimer = TimerChanger;
                break;
            case 'U':
                UpTimer = TimerChanger;
                break;
            case 'D':
                DownTimer = TimerChanger;
                break;

        }
    }



    void RandomizeDirection()
    {
        X = Random.Range(-0.1f, 0.1f);
        Y = Random.Range(-0.1f, 0.1f);
        ChangeTime = Random.Range(MinChangeTime, MaxChangeTime);
        CurrentSpeed = new Vector3(X, 0f, Y).magnitude;
        CurrentAngular = Random.Range(MinAngularAcc, MaxAngularAcc);

        Invoke("RandomizeDirection", ChangeTime);
    }

}

