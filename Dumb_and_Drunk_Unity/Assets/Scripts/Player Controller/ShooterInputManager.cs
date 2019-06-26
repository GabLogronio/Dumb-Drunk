using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShooterInputManager : InputManager
{
    // PlayerControl
    float RightTimer = 0f, LeftTimer = 0f, UpTimer = 0f, DownTimer = 0f, TimerChanger = 2.5f, InputX = 0f, InputY = 0f, InputSpeed = 50f, cooldown = 1f;

    // RandomWanderer
    float MinChangeTime = 3f, MaxChangeTime = 10f,
      DeltaX = 0f, DeltaY = 0f, X = 0f, Y = 0f, CurrentChangeTime = 0f, CurrentSpeed = 0f;

    private float charge = 0, MaxCharge = 3f;
    private bool pressed = false, inCooldown = false;
    [SerializeField]
    GameObject BottlePrefab;
    [SerializeField]
    RectTransform pointerRT;


    float lastChange = 0;
    float multiplicator = 30f;

    public override void PressedButton(string ButtonName, bool Down)
    {
        if (Down)
        {
            pressed = true;
        }
        else
        {
            pressed = false;
            Shoot();
        }
    }

    public override void SetAnalogAxis(float ToSetHor, float ToSetVer)
    {
        //mind your business
    }

    public override void SetGyroscope(char Right, char Left, char Down, char Up)
    {
        if (Right == 'T') RightTimer = TimerChanger;
        if (Left == 'T') LeftTimer = TimerChanger;
        if (Down == 'T') DownTimer = TimerChanger;
        if (Up == 'T') UpTimer = TimerChanger;

    }

    void FinishCooldown()
    {
        inCooldown = false;
    }

    void Start()
    {
        RandomizeDirection();
    }

    private float chargeToPower()
    {
        if (charge > 1.8f) charge = 1.8f;
        return (charge / 1.8f * 9) + 1;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimers();
        CheckBounds();

        if (pressed) charge += Time.deltaTime;

        X += DeltaX * Time.deltaTime;
        Y += DeltaY * Time.deltaTime;


        InputX = RightTimer - LeftTimer;
        InputY = UpTimer - DownTimer;

        pointerRT.Translate(new Vector3((X * CurrentSpeed) + (InputX * InputSpeed) * Time.deltaTime, (Y * CurrentSpeed) + (InputY * InputSpeed) * Time.deltaTime, 0f));
    }

    void CheckBounds()
    {
        if (pointerRT.localPosition.x > 960)
        {
            if (X > 0)
            {
                DeltaX = -Mathf.Abs(DeltaX);
                X = -Mathf.Abs(X);
            }

            if (RightTimer > 0f)
            {
                RightTimer = 0f;
                LeftTimer = TimerChanger;
            }

        }
        if (pointerRT.localPosition.x < -960)
        {
            if (X < 0)
            {
                DeltaX = Mathf.Abs(DeltaX);
                X = Mathf.Abs(X);
            }

            if (LeftTimer > 0f)
            {
                LeftTimer = 0f;
                RightTimer = TimerChanger;
            }

        }
        if (pointerRT.localPosition.y > 540)
        {
            if (Y > 0)
            {
                DeltaY = -Mathf.Abs(DeltaY);
                Y = -Mathf.Abs(Y);
            }

            if (UpTimer > 0f)
            {
                UpTimer = 0f;
                DownTimer = TimerChanger;
            }

        }
        if (pointerRT.localPosition.y < -540)
        {
            if (Y < 0)
            {
                DeltaY = Mathf.Abs(DeltaY);
                Y = Mathf.Abs(Y);
            }

            if (DownTimer > 0f)
            {
                DownTimer = 0f;
                UpTimer = TimerChanger;
            }
        }
    }

    public override void Fallen(bool ToSet)
    {

    }

    private void Shoot()
    {
        if (!inCooldown)
        {
            GameObject newBottle = Instantiate(BottlePrefab, Camera.main.transform.position, Quaternion.identity);
            newBottle.GetComponent<Rigidbody>().velocity = ((Camera.main.ScreenToWorldPoint(new Vector3(pointerRT.position.x, pointerRT.position.y, Camera.main.farClipPlane)) - Camera.main.transform.position).normalized * Mathf.Log(chargeToPower(), 10) * multiplicator);
            newBottle.GetComponent<Rigidbody>().AddTorque(Vector3.right * 100f * multiplicator);
            AkSoundEngine.PostEvent("Bottle_Woosh", gameObject);
            inCooldown = true;
            Invoke("FinishCooldown", cooldown);
        }

        charge = 0;
    }

    void RandomizeDirection()
    {
        float NewX = Random.Range(-1f, 1f), NewY = Random.Range(-1f, 1f);
        CurrentChangeTime = Random.Range(MinChangeTime, MaxChangeTime);
        CurrentSpeed = new Vector3(NewX, NewY, 0f).magnitude;

        DeltaX = (NewX - X) / CurrentChangeTime;
        DeltaY = (NewY - Y) / CurrentChangeTime;

        Invoke("RandomizeDirection", CurrentChangeTime);
    }

    void UpdateTimers()
    {
        if (RightTimer > 0f) RightTimer -= Time.deltaTime;
        if (LeftTimer > 0f) LeftTimer -= Time.deltaTime;
        if (UpTimer > 0f) UpTimer -= Time.deltaTime;
        if (DownTimer > 0f) DownTimer -= Time.deltaTime;

    }
}
