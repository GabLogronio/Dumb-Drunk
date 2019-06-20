using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBalanceManager : MonoBehaviour {

    // Random Wander parameters
    float MinChangeTime = 2f, MaxChangeTime = 5f, Delta = 0f, CurrentRandom = 0f;
    // Input parameters
    float RightTimer = 0f, LeftTimer = 0f, TimerChanger = 1.5f, CurrentInput = 0f, InputSpeed = 0.35f;

    float CurrentBalance = 0f;

    [SerializeField]
    GameObject RightFoot, LeftFoot, Hips, BalanceGUI, KeyPrefab;

    [SerializeField]
    RectTransform PlayerImage;

    [SerializeField]
    PlayerInputManager InputController;

    bool RandomMoving = true, BlockedBar = false;

    float BodyLength = 0.6f, LegsLength = 1.66f, SpringForce = 1000f;

    Vector3 InitialPosition = Vector3.zero, ImageInitialPosition = Vector3.zero;

    bool Fallen = false;

    private void Start()
    {
        RandomizeDirection();
        ResetBar();
        ImageInitialPosition = new Vector3(0, 40f, 0);

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

    public void Fall(Vector3 direction)
    {
        Fall();
        Hips.GetComponent<Rigidbody>().velocity = direction;

    }

    void ShootKeys(int NumberOfKeys)
    {
        for (int i = 0; i < NumberOfKeys; i++)
        {
            float X = Random.Range(0f, 1f), Y = Random.Range(0f, 1f);
            if (X > 0f) X += 1f; else X -= 1f;
            if (Y > 0f) Y += 1f; else Y -= 1f;
            Vector3 ShootDirection = new Vector3(X, 2f, Y);

            GameObject newKey = Instantiate(KeyPrefab, new Vector3(transform.position.x + X, 2f, transform.position.z + Y), Quaternion.identity);

        }
    }

    public void RecoverFromFall()
    {
        //SOUND GOT UP
        //AkSoundEngine.PostEvent("GotUp", gameObject);

        InputController.SetCanAttach(true);
        InputController.BlockControls(false);
        ResetBar();
        BlockBar(true, 5f);

        BalanceGUI.SetActive(true);

        Hips.GetComponent<SpringJoint>().spring = 1000f;
        Hips.GetComponent<Rigidbody>().AddForce(Vector3.up * 100f);

        NetworkServerManager.getInstance().ServerStringMessageSender(InputController, "GotUp");

    }

    public void ResetForScene2()
    {
        ResetBar();
        Hips.GetComponent<SpringJoint>().spring = 1000f;
        Hips.GetComponent<Rigidbody>().AddForce(Vector3.up * 100f);
        BlockBar(true);

    }

    void ResetBar()
    {
        float Height = Mathf.Sqrt(LegsLength * LegsLength - Mathf.Pow((Vector3.Distance(RightFoot.transform.position, LeftFoot.transform.position) / 2f), 2)) + BodyLength;
        if(float.IsNaN(Height)) Height = 2.25f;
        InitialPosition = new Vector3((RightFoot.transform.position.x + LeftFoot.transform.position.x) / 2, Height, (RightFoot.transform.position.z + LeftFoot.transform.position.z) / 2);
        transform.position = InitialPosition;
        PlayerImage.localPosition = ImageInitialPosition;

        Fallen = false;
        CurrentRandom = 0f;
        CurrentBalance = 0f;
        CurrentInput = 0f;
        RightTimer = 0f; LeftTimer = 0f;
    }

    public bool HasFallen()
    {
        return Fallen;
    }

    public void BlockBar(bool ToSet)
    {
        BlockedBar = ToSet;
    }

    public void BlockBar(bool ToSet, float Timer)
    {
        BlockedBar = ToSet;
        Invoke("UnlockBar", Timer);
    }

    void UnlockBar()
    {
        BlockedBar = false;
    }

    void Update()
    {
        if (!Fallen)
        {
            UpdateTimers();
            // Initial position based on the feet
            float Height = Mathf.Sqrt(LegsLength * LegsLength - Mathf.Pow((Vector3.Distance(RightFoot.transform.position, LeftFoot.transform.position) / 2f), 2)) + BodyLength;
            InitialPosition = new Vector3((RightFoot.transform.position.x + LeftFoot.transform.position.x) / 2, Height, (RightFoot.transform.position.z + LeftFoot.transform.position.z) / 2);

            if (!float.IsNaN(Height) && !float.IsNaN(CurrentBalance) && transform.position.y > RightFoot.transform.position.y && transform.position.y > LeftFoot.transform.position.y)
            {
                if(BlockedBar)
                {
                    transform.position = InitialPosition;
                    PlayerImage.localPosition = ImageInitialPosition;
                }
                else
                {
                    if (RandomMoving) CurrentRandom += Delta * Time.deltaTime;
                    else CurrentRandom = 0f;

                    CurrentInput = (RightTimer - LeftTimer) / TimerChanger * InputSpeed * Time.deltaTime;

                    CurrentBalance += (CurrentRandom + CurrentInput);

                    transform.position = InitialPosition + transform.right * CurrentBalance;

                    PlayerImage.localPosition = ImageInitialPosition + new Vector3( CurrentBalance / 0.75f * 80f, 0f, 0f);

                    // DebugText.instance.Set("Balance: " + CurrentBalance);

                    if (CurrentBalance > 0.75f || CurrentBalance < -0.75) Fall();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Z)) Fall();
        if (Input.GetKeyDown(KeyCode.X)) RecoverFromFall();

    }

    public void SetRandomMoving(bool ToSet)
    {
        ResetBar();
        RandomMoving = ToSet;
    }

    void UpdateTimers()
    {
        if (RightTimer > 0f) RightTimer -= Time.deltaTime;
        if (LeftTimer > 0f) LeftTimer -= Time.deltaTime;

    }

    public void MoveBodyCenter(char Right, char Left, char Up, char Down)
    {
        if (Right == 'T') RightTimer = TimerChanger;
        if (Left == 'T') LeftTimer = TimerChanger;

    }

    void RandomizeDirection()
    {
        float NewRandom = Random.Range(-0.01f, 0.01f);
        float CurrentChangeTime = Random.Range(MinChangeTime, MaxChangeTime);

        Delta = (NewRandom - CurrentRandom) / CurrentChangeTime;

        Invoke("RandomizeDirection", CurrentChangeTime);
    }

    public Vector3 GetInitialPosition()
    {
        return InitialPosition;
    }

}

