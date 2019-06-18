using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBalanceManager : MonoBehaviour {

    bool Blocked = true;

    // Random Wander parameters
    float MinChangeTime = 2f, MaxChangeTime = 5f,
      DeltaX = 0f, DeltaY = 0f, X = 0f, Y = 0f, CurrentChangeTime = 0f;

    float RightTimer = 0f, LeftTimer = 0f, UpTimer = 0f, DownTimer = 0f, TimerChanger = 1.5f, InputX = 0f, InputY = 0f, InputSpeed = 0.35f;

    [SerializeField]
    GameObject RightFoot, LeftFoot, Hips, BalanceGUI, KeyPrefab;

    [SerializeField]
    RectTransform PlayerImage;

    [SerializeField]
    PlayerInputManager InputController;

    bool Moving = true;

    float BodyLength = 0.6f, LegsLength = 1.66f;

    Vector3 PositionModifier = Vector3.zero, InitialPosition = Vector3.zero, ImageInitialPosition = Vector3.zero;

    bool Fallen = false;

    private void Start()
    {
        RandomizeDirection();
        ResetBar();
        ImageInitialPosition = PlayerImage.localPosition;

    }

    public void Fall()
    {
        Fallen = true;

        BlockBar(true);

        InputController.Detach();
        InputController.BlockControls(true);
        InputController.SetCanAttach(false);

        BalanceGUI.SetActive(false);

        Hips.GetComponent<SpringJoint>().spring = 0f;
        Hips.GetComponent<Rigidbody>().velocity = Vector3.zero;

        NetworkServerManager.getInstance().ServerStringMessageSender(InputController, "Fallen");
        ShootKeys(MatchManager.getInstance().keyCollected[gameObject.layer - 9]);
        MatchManager.getInstance().keyCollected[gameObject.layer - 9] = 0;
    }

    void ShootKeys(int NumberOfKeys)
    {
        float X = Random.Range(0f, 1f), Y = Random.Range(0f, 1f);
        if (X > 0f) X += 1f; else X -= 1f;
        if (Y > 0f) Y += 1f; else Y -= 1f;
        Vector3 ShootDirection = new Vector3(X, 2f, Y);

        GameObject newKey = Instantiate(KeyPrefab, new Vector3 (transform.position.x + X, 0f, transform.position.z + Y), Quaternion.identity);

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
        BlockBar(true, 5f);

        BalanceGUI.SetActive(true);

        Hips.GetComponent<SpringJoint>().spring = 1000f;
        //Hips.GetComponent<Rigidbody>().AddForce(Vector3.up * 500f);

        //transform.localPosition = new Vector3(0f, LegsLength + BodyLength, 0f);
        ResetBar();


        NetworkServerManager.getInstance().ServerStringMessageSender(InputController, "GotUp");

    }

    void ResetBar()
    {
        float Height = Mathf.Sqrt(LegsLength * LegsLength - Mathf.Pow((Vector3.Distance(RightFoot.transform.position, LeftFoot.transform.position) / 2f), 2)) + BodyLength;
        InitialPosition = new Vector3((RightFoot.transform.position.x + LeftFoot.transform.position.x) / 2, Height, (RightFoot.transform.position.z + LeftFoot.transform.position.z) / 2);
        transform.position = InitialPosition;

        PositionModifier = Vector3.zero;
        RightTimer = 0f; LeftTimer = 0f; UpTimer = 0f; DownTimer = 0f;
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
        PlayerImage.localPosition = ImageInitialPosition;

        if (!Fallen)
        {
            UpdateTimers();

            // Initial position based on the feet
            float Height = Mathf.Sqrt(LegsLength * LegsLength - Mathf.Pow((Vector3.Distance(RightFoot.transform.position, LeftFoot.transform.position) / 2f), 2)) + BodyLength;
            InitialPosition = new Vector3((RightFoot.transform.position.x + LeftFoot.transform.position.x) / 2, Height, (RightFoot.transform.position.z + LeftFoot.transform.position.z) / 2);

            if (!float.IsNaN(Height) && !float.IsNaN(PositionModifier.x) && !float.IsNaN(PositionModifier.y) && !float.IsNaN(PositionModifier.z) && transform.position.y > RightFoot.transform.position.y && transform.position.y > LeftFoot.transform.position.y)
            {
                if(!Blocked)
                {
                    if (Moving)
                    {
                        X += DeltaX * Time.deltaTime;
                        Y += DeltaY * Time.deltaTime;

                        InputX = (RightTimer - LeftTimer) / TimerChanger * InputSpeed * Time.deltaTime;
                        InputY = (UpTimer - DownTimer) / TimerChanger * InputSpeed * Time.deltaTime;

                        //DebugText.instance.Set("RT: " + RightTimer + ", LT: " + LeftTimer + ", UT: " + UpTimer + ", DT: " + DownTimer + " - ");
                        //DebugText.instance.Set("Input X: " + InputX + ", Input Y: " + InputY + ", UT: " + " - ");


                    }

                    PositionModifier += new Vector3(X, 0f, Y);
                    PositionModifier += Vector3.right * InputX + Vector3.forward * InputY;

                    transform.position = InitialPosition + PositionModifier;
                    PlayerImage.localPosition = ImageInitialPosition + new Vector3(X, Y, 0f) / 0.75f * 80f;

                    if (PositionModifier.magnitude > 0.75f) Fall();

                }
                else
                {
                    transform.position = InitialPosition;

                }
            }
        }
    }

    public void SetMoving(bool ToSet)
    {
        ResetBar();
        Moving = ToSet;
    }

    void UpdateTimers()
    {
        if (RightTimer > 0f) RightTimer -= Time.deltaTime;
        if (LeftTimer > 0f) LeftTimer -= Time.deltaTime;
        if (UpTimer > 0f) UpTimer -= Time.deltaTime;
        if (DownTimer > 0f) DownTimer -= Time.deltaTime;

    }

    public void MoveBodyCenter(char Right, char Left, char Up, char Down)
    {
        if (Right == 'T') RightTimer = TimerChanger;
        if (Left == 'T') LeftTimer = TimerChanger;
        if (Down == 'T') DownTimer = TimerChanger;
        if (Up == 'T') UpTimer = TimerChanger;

    }

    void RandomizeDirection()
    {
        float NewX = Random.Range(-0.001f, 0.001f), NewY = Random.Range(-0.001f, 0.001f);
        CurrentChangeTime = Random.Range(MinChangeTime, MaxChangeTime);

        DeltaX = (NewX - X) / CurrentChangeTime;
        DeltaY = (NewY - Y) / CurrentChangeTime;

        Invoke("RandomizeDirection", CurrentChangeTime);
    }

    public Vector3 GetInitialPosition()
    {
        return InitialPosition;
    }

}

