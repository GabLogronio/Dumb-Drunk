using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShooterInputManager : InputManager
{
    private float charge = 0;
    private bool pressed = false;
    public GameObject bottle;
    public GameObject pointer;
    private RectTransform pointerRT;
    private bool ToRight = false, ToUp = false;
    private float AimX = 0, AimY = 0;
    private float step = 400;
    private float lastChange = 0;
    private bool ToRightRandom = true, ToUpRandom = true;
    public float multiplicator = 1000;

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

    public override void SetGyroscope(float ToSetXRot, float ToSetZRot)
    {
        if (ToSetZRot > 0) ToRight = true;
        else ToRight = false;

        if (ToSetXRot > 0) ToUp = false;
        else ToUp = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //pointerRT = pointer.GetComponent<RectTransform>();
    }

    private float chargeToPower()
    {
        if (charge > 1.8f) charge = 1.8f;
        return (charge / 1.8f * 9) + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) PressedButton("", true);
        if (Input.GetKeyUp(KeyCode.Space)) PressedButton("", false);
        if (pressed) charge += Time.deltaTime;
        if (lastChange + Time.deltaTime > 1)
        {
            lastChange = 0;
            ToRightRandom = (Random.value > 0.5f);
            ToUpRandom = (Random.value > 0.5f);
        }
        AimX += ((ToRight ? step : -step) + (ToRightRandom ? step / 2 : -step / 2))*Time.deltaTime;
        AimY += ((ToUp ? step : -step) + (ToUpRandom ? step / 2 : -step / 2))*Time.deltaTime;
        //pointerRT.localPosition = new Vector3(AimX, AimY, 1.0f);
    }

    private void Shoot()
    {
        GameObject newBottle = Instantiate(bottle, Camera.main.transform.position, Quaternion.identity);
        newBottle.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * Mathf.Log(chargeToPower(), 10) * multiplicator);
        newBottle.GetComponent<Rigidbody>().AddTorque(Vector3.right * multiplicator);
        charge = 0;
    }
}
