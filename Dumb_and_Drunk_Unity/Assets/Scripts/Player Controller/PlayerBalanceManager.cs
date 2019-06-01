using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBalanceManager : MonoBehaviour {

    [SerializeField]
    bool Mode1 = true;
    bool Blocked = false;

    float MinChangeTime = 0f, MaxChangeTime = 1.5f, MinAngularAcc = 0.001f, MaxAngularAcc = 0.01f,
          X = 0f, Y = 0f, ChangeTime = 0f, CurrentSpeed = 0f, CurrentAngular = 0f;

    [SerializeField]
    GameObject RightFoot, LeftFoot, Hips;

    [SerializeField]
    PlayerInputManager InputController;

    [SerializeField]
    float BodyLength = 0.55f, LegsLength = 0.85f, Forward = 0.1f;
    float XRot = 0f, ZRot = 0f;

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
        XRot = 0f;
        ZRot = 0f;

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
            float Height = Mathf.Sqrt(LegsLength * LegsLength - Mathf.Pow((Vector3.Distance(RightFoot.transform.position, LeftFoot.transform.position) / 2f), 2)) + BodyLength;

            if (new Vector2(transform.localPosition.x, transform.localPosition.z).magnitude < 0.375f)
            {
                Vector3 BalanceModifier = Vector3.right * XRot + Vector3.forward * ZRot;

                if (!Blocked && !float.IsNaN(Height) && !float.IsNaN(BalanceModifier.x) && !float.IsNaN(BalanceModifier.y) && !float.IsNaN(BalanceModifier.z) && transform.position.y > RightFoot.transform.position.y && transform.position.y > LeftFoot.transform.position.y)
                {
                    transform.position = new Vector3(transform.position.x, Height, transform.position.z);
                    transform.Translate(transform.forward * Time.deltaTime * CurrentSpeed);
                    transform.Translate(BalanceModifier * Time.deltaTime, Space.World);
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(X, 0f, Y)), CurrentAngular);
                    // + transform.right * Vector3.Distance(RightFoot.transform.position, LeftFoot.transform.position) * BalanceValue / 200;
                }
            }
            else
            {
                //Fall();
            }
        }
    }

    public void MoveBodyCenter(float x, float z)
    {
        if (Mode1)
        {
            XRot = x / 7.5f;
            ZRot = z / 7.5f;
        }
        else
        {
            XRot += x / 20f;
            ZRot += z / 20f;
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

