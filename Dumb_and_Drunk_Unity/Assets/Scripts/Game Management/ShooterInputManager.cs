using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShooterInputManager : InputManager
{

    float RightTimer = 0f, LeftTimer = 0f, UpTimer = 0f, DownTimer = 0f, TimerChanger = 2.5f, InputX = 0f, InputY = 0f, InputSpeed = 20f;

    float MinChangeTime = 3f, MaxChangeTime = 10f,
      DeltaX = 0f, DeltaY = 0f, X = 0f, Y = 0f, CurrentChangeTime = 0f, CurrentSpeed = 0f;

    private float charge = 0;
    private bool pressed = false;
    public GameObject bottle;
    public GameObject pointer;
    private RectTransform pointerRT;

    private float lastChange = 0;
    public float multiplicator = 1000;

    public override void PressedButton(string ButtonName, bool Down)
    {
        if (Down)
        {
            DebugText.instance.Set("Charging shot");
            pressed = true;
        }
        else
        {
            DebugText.instance.Set("Released shot");
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

    void Start()
    {
        pointerRT = pointer.GetComponent<RectTransform>();
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

        if (Input.GetKeyDown(KeyCode.Space)) PressedButton("", true);
        if (Input.GetKeyUp(KeyCode.Space)) PressedButton("", false);

        if (pressed) charge += Time.deltaTime;

        if (pointerRT.localPosition.x > 960 && X > 0)
        {
            Debug.Log("Destra");
            DeltaX = - Mathf.Abs(DeltaX);
            X = -Mathf.Abs(X);
        }
        if (pointerRT.localPosition.x < -960 && X < 0)
        {
            Debug.Log("Sinistra");
            DeltaX = Mathf.Abs(DeltaX);
            X = Mathf.Abs(X);
        }
        if (pointerRT.localPosition.y > 540 && Y > 0)
        {
            Debug.Log("Su");
            DeltaY = -Mathf.Abs(DeltaY);
            Y = -Mathf.Abs(Y);
        }
        if (pointerRT.localPosition.y < -540 && Y < 0)
        {
            Debug.Log("Giù");
            DeltaY = Mathf.Abs(DeltaY);
            Y = Mathf.Abs(Y);
        }


        X += DeltaX * Time.deltaTime;
        Y += DeltaY * Time.deltaTime;


        //InputX = RightTimer - LeftTimer;
        //InputY = UpTimer - DownTimer;

            //pointerRT.localPosition = new Vector3(pointerRT.localPosition.x + X * CurrentSpeed * Time.deltaTime, pointerRT.localPosition.y + Y * CurrentSpeed * Time.deltaTime, 1.0f);
        pointerRT.Translate(new Vector3((X * CurrentSpeed) + (InputX * InputSpeed) * Time.deltaTime, (Y * CurrentSpeed) + (InputY * InputSpeed) * Time.deltaTime, 0f));
    }

    public override void Fallen(bool ToSet)
    {

    }

    private void Shoot()
    {
        GameObject newBottle = Instantiate(bottle, Camera.main.transform.position, Quaternion.identity);
        newBottle.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * Mathf.Log(chargeToPower(), 10) * multiplicator);
        newBottle.GetComponent<Rigidbody>().AddTorque(Vector3.right * 100f * multiplicator);
        charge = 0;
    }

    void RandomizeDirection()
    {
        float NewX = Random.Range(-1f, 1f), NewY = Random.Range(-1f, 1f);
        CurrentChangeTime = Random.Range(MinChangeTime, MaxChangeTime);
        CurrentSpeed = new Vector3(X, Y, 0f).magnitude / 2;

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
