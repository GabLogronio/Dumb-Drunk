using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCenter : MonoBehaviour {

    [SerializeField]
    bool Mode1 = true;

    [SerializeField]
    float MinChangeTime = 0f, MaxChangeTime = 1.5f, MinAngularAcc = 0.001f, MaxAngularAcc = 0.01f;

    float X = 0f, Y = 0f, ChangeTime = 0f, CurrentSpeed = 0f, CurrentAngular = 0f;

    [SerializeField]
    GameObject RightFoot, LeftFoot;


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
    }

    public void RecoverFromFall()
    {
        Fallen = false;
    }

    void Update()
    {
        float Height = Mathf.Sqrt(LegsLength * LegsLength - Mathf.Pow((Vector3.Distance(RightFoot.transform.position, LeftFoot.transform.position) / 2f), 2)) + BodyLength;

        if ( new Vector2 (transform.localPosition.x, transform.localPosition.z).magnitude < 0.375f)
        {
            Vector3 BalanceModifier = Vector3.right * XRot + Vector3.forward * ZRot;

            if (!float.IsNaN(Height) && !float.IsNaN(BalanceModifier.x) && !float.IsNaN(BalanceModifier.y) && !float.IsNaN(BalanceModifier.z) && transform.position.y > RightFoot.transform.position.y && transform.position.y > LeftFoot.transform.position.y)
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
            Debug.Log("Fallen");
            transform.localPosition = new Vector3(0f, Height, 0f);
            XRot = 0f;
            ZRot = 0f;
            // FALL
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

/*      OLD UPDATE FUNCTION
 *     void Update()
    {
        if(!Fallen)
        {

            float Height = Mathf.Sqrt(LegsLength * LegsLength - Mathf.Pow((Vector3.Distance(RightFoot.transform.position, LeftFoot.transform.position) / 2f), 2)) + BodyLength;

            Vector3 forward = new Vector3((RightFoot.transform.forward.x + LeftFoot.transform.forward.x) / 2, 0, (RightFoot.transform.forward.z + LeftFoot.transform.forward.z) / 2);
            transform.forward = forward;

            if (!float.IsNaN(Height) && transform.position.y > RightFoot.transform.position.y && transform.position.y > LeftFoot.transform.position.y)
            {
                Vector3 Center = new Vector3((RightFoot.transform.position.x + LeftFoot.transform.position.x) / 2,
                             (RightFoot.transform.position.y + LeftFoot.transform.position.y) / 2 + Height,
                             (RightFoot.transform.position.z + LeftFoot.transform.position.z) / 2);
                Vector3 BalanceModifier = Vector3.right * XRot + Vector3.forward * ZRot;
                if (!float.IsNaN(Center.x) && !float.IsNaN(Center.y) && !float.IsNaN(Center.z) && !float.IsNaN(BalanceModifier.x) && !float.IsNaN(BalanceModifier.y) && !float.IsNaN(BalanceModifier.z))
                    transform.position = Center + BalanceModifier;
                // + transform.right * Vector3.Distance(RightFoot.transform.position, LeftFoot.transform.position) * BalanceValue / 200;
            }

        }     
    }
 */
