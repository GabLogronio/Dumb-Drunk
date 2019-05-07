using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCenter : MonoBehaviour {

    [SerializeField]
    GameObject RightFoot, LeftFoot, Puke;
    GameObject PukeInstantiated;

    float BodyLength = 0.55f, LegsLength = 0.85f;

    bool ThrowingUp = false, Fallen = false;

    PlayerBarsManager BarsManager;

    private void Start()
    {
        BarsManager = GetComponent<PlayerBarsManager>();
    }

    public void ThrowUp()
    {
        PukeInstantiated = Instantiate(Puke, new Vector3(transform.position.x, 0f, transform.position.z) + transform.forward * 0.55f, transform.parent.transform.rotation);
        PukeInstantiated.GetComponent<PuddleObstacle>().Activate();
        ThrowingUp = true;

        RightFoot.GetComponent<FootController>().FixFoot(true);
        LeftFoot.GetComponent<FootController>().FixFoot(true);
    }

    public void StopThrowingUp()
    {
        PukeInstantiated.GetComponent<PuddleObstacle>().StopSpread();
        ThrowingUp = false;

        RightFoot.GetComponent<FootController>().FixFoot(false);
        LeftFoot.GetComponent<FootController>().FixFoot(false);
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
        if(!Fallen)

        if (ThrowingUp)
        {
            float Height = Mathf.Sqrt(LegsLength * LegsLength - Mathf.Pow((Vector3.Distance(RightFoot.transform.position, LeftFoot.transform.position) / 2f), 2));
            Vector3 forward = new Vector3((RightFoot.transform.forward.x + LeftFoot.transform.forward.x) / 2, 0, (RightFoot.transform.forward.z + LeftFoot.transform.forward.z) / 2);
            transform.forward = forward;
                if (!float.IsNaN(Height) && transform.position.y > RightFoot.transform.position.y && transform.position.y > LeftFoot.transform.position.y)
                {
                    Vector3 Desired = new Vector3((RightFoot.transform.position.x + LeftFoot.transform.position.x) / 2,
                                             (RightFoot.transform.position.y + LeftFoot.transform.position.y) / 2 + Height,
                                             (RightFoot.transform.position.z + LeftFoot.transform.position.z) / 2)
                                             + transform.forward * 0.3f;
                    if (!float.IsNaN(Desired.x) && !float.IsNaN(Desired.y) && !float.IsNaN(Desired.z))
                        transform.position = Desired;
                }
        }
        else
        {
            float Height = Mathf.Sqrt(LegsLength * LegsLength - Mathf.Pow((Vector3.Distance(RightFoot.transform.position, LeftFoot.transform.position) / 2f), 2)) + BodyLength;
            //float BalanceValue = GetComponent<PlayerBarsManager>().GetBalanceValue();
            Vector3 forward = new Vector3((RightFoot.transform.forward.x + LeftFoot.transform.forward.x) / 2, 0, (RightFoot.transform.forward.z + LeftFoot.transform.forward.z) / 2);
            transform.forward = forward;
                if (!float.IsNaN(Height) && transform.position.y > RightFoot.transform.position.y && transform.position.y > LeftFoot.transform.position.y)
                {
                    Vector3 Desired = new Vector3((RightFoot.transform.position.x + LeftFoot.transform.position.x) / 2,
                                 (RightFoot.transform.position.y + LeftFoot.transform.position.y) / 2 + Height,
                                 (RightFoot.transform.position.z + LeftFoot.transform.position.z) / 2)
                                 + transform.forward * 0.05f;
                    if (!float.IsNaN(Desired.x) && !float.IsNaN(Desired.y) && !float.IsNaN(Desired.z))
                        transform.position = Desired;
                    // + transform.right * Vector3.Distance(RightFoot.transform.position, LeftFoot.transform.position) * BalanceValue / 200;
                }

            }

        
    }

}
