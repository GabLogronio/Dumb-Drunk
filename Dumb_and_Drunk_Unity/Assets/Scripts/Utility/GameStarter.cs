using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStarter : MonoBehaviour {

    bool P1Ready = false, P2Ready = false, P1Chosen = false, P2Chosen = false, Step0 = true,  Step1 = false, Step2 = false, Step3 = false, Step4 = false;

    [SerializeField]
    Transform InitialTransform, FirstMiddleTransform, SecondMiddleTransform, FinalTransform;

    [SerializeField]
    float MovementSpeed, RotationSpeed;

    [SerializeField]
    GameObject P1Controller, P2Controller, P1ReadyText, P2ReadyText, P1GUIStarter, P2GUIStarter, P1GUI, P2GUI, P1BBar, P2BBar, DifficultyExplanationText, P1Selector, P2Selector, Camera;

    // Use this for initialization
    void Start () {

        P1Controller.GetComponent<PlayerInputController>().BlockControls(true);
        P2Controller.GetComponent<PlayerInputController>().BlockControls(true);
        /*
        P1Controller.GetComponent<PlayerBarsManager>().BlockBar(true, true);
        P1Controller.GetComponent<PlayerBarsManager>().BlockBar(false, true);

        P2Controller.GetComponent<PlayerBarsManager>().BlockBar(true, true);
        P2Controller.GetComponent<PlayerBarsManager>().BlockBar(false, true);
        */
        Camera.transform.position = InitialTransform.position;
        Camera.transform.rotation = InitialTransform.rotation;


    }

    // Update is called once per frame
    void Update () {

        if (Step4)
        {
            if (Vector3.Distance(FinalTransform.position, Camera.transform.position) < 0.1f)
            {
                Step4 = false;
                P1Controller.GetComponent<PlayerInputController>().BlockControls(false);
                P2Controller.GetComponent<PlayerInputController>().BlockControls(false);
                /*
                P1Controller.GetComponent<PlayerBarsManager>().BlockBar(true, false);
                P1Controller.GetComponent<PlayerBarsManager>().BlockBar(false, false);

                P2Controller.GetComponent<PlayerBarsManager>().BlockBar(true, false);
                P2Controller.GetComponent<PlayerBarsManager>().BlockBar(false, false);
                */
                P1GUI.SetActive(true);
                P2GUI.SetActive(true);

                P1BBar.SetActive(true);
                P2BBar.SetActive(true);

                GameSettings.SettingsInstance.Setup();
                GameManager.GameManagerInstance.StartTimer();

                Camera.GetComponent<CameraFollow>().Activate();

            }
            else
            {
                Camera.transform.position = Vector3.MoveTowards(Camera.transform.position, FinalTransform.position, Time.deltaTime * MovementSpeed * 3.5f);
            }

        }

        if (Step3)
        {
            if (Vector3.Distance(SecondMiddleTransform.position, Camera.transform.position) < 0.1f)
            {
                Step3 = false;
                Step4 = true;
            }
            else
            {
                Camera.transform.position = Vector3.MoveTowards(Camera.transform.position, SecondMiddleTransform.position, Time.deltaTime * MovementSpeed);
                Camera.transform.rotation = Quaternion.Slerp(Camera.transform.rotation, SecondMiddleTransform.rotation, Time.deltaTime * RotationSpeed);

            }

        }

        if (Step2)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.J))
            {
                P1Chosen = false;

                GameSettings.SettingsInstance.Player1Difficulty = GameSettings.DifficultyLevel.Easy;
                P1Selector.GetComponent<RectTransform>().anchoredPosition = new Vector3(P1Selector.GetComponent<RectTransform>().anchoredPosition.x, -140f, 0);
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.K))
            {
                P1Chosen = false;

                GameSettings.SettingsInstance.Player1Difficulty = GameSettings.DifficultyLevel.Normal;
                P1Selector.GetComponent<RectTransform>().anchoredPosition = new Vector3(P1Selector.GetComponent<RectTransform>().anchoredPosition.x, 30f, 0);


            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.L))
            {
                P1Chosen = false;

                GameSettings.SettingsInstance.Player1Difficulty = GameSettings.DifficultyLevel.Hard;
                P1Selector.GetComponent<RectTransform>().anchoredPosition = new Vector3(P1Selector.GetComponent<RectTransform>().anchoredPosition.x, 190f, 0);

            }

            if (Input.GetKeyDown(KeyCode.Joystick2Button0) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                P2Chosen = false;

                GameSettings.SettingsInstance.Player2Difficulty = GameSettings.DifficultyLevel.Easy;
                P2Selector.GetComponent<RectTransform>().anchoredPosition = new Vector3(P2Selector.GetComponent<RectTransform>().anchoredPosition.x, -140f, 0);


            }
            if (Input.GetKeyDown(KeyCode.Joystick2Button1) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                P2Chosen = false;

                GameSettings.SettingsInstance.Player2Difficulty = GameSettings.DifficultyLevel.Normal;
                P2Selector.GetComponent<RectTransform>().anchoredPosition = new Vector3(P2Selector.GetComponent<RectTransform>().anchoredPosition.x, 30f, 0);


            }
            if (Input.GetKeyDown(KeyCode.Joystick2Button3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                P2Chosen = false;

                GameSettings.SettingsInstance.Player2Difficulty = GameSettings.DifficultyLevel.Hard;
                P2Selector.GetComponent<RectTransform>().anchoredPosition = new Vector3(P2Selector.GetComponent<RectTransform>().anchoredPosition.x, 190f, 0);


            }

            if (Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.E))
            {
                P1Chosen = true;
                P1Selector.GetComponent<RectTransform>().anchoredPosition = new Vector3(P1Selector.GetComponent<RectTransform>().anchoredPosition.x, -720f, 0);

            }

            if (Input.GetKeyDown(KeyCode.Joystick2Button7) || Input.GetKeyDown(KeyCode.Keypad5))
            {
                P2Chosen = true;
                P2Selector.GetComponent<RectTransform>().anchoredPosition = new Vector3(P2Selector.GetComponent<RectTransform>().anchoredPosition.x, -720f, 0);

            }

            if (P1Chosen && P2Chosen)
            {
                Step2 = false;
                Step3 = true;

                DifficultyExplanationText.SetActive(false);
                P1GUIStarter.SetActive(false);
                P2GUIStarter.SetActive(false);

                P1Selector.SetActive(false);
                P2Selector.SetActive(false);
            }
        }

        if (Step1)
        {
            if (Vector3.Distance(FirstMiddleTransform.position, Camera.transform.position) < 0.1f)
            {
                Invoke("EndStep1", 0.25f);
           
            }
            else
            {
                Camera.transform.position = Vector3.MoveTowards(Camera.transform.position, FirstMiddleTransform.position, Time.deltaTime * MovementSpeed * 5f);

            }
        }

        if (Step0)
        { 
            if (Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.E))
            {
                P1Ready = !P1Ready;

                if (P1Ready) P1ReadyText.SetActive(true);
                else P1ReadyText.SetActive(false);

            }

            if (Input.GetKeyDown(KeyCode.Joystick2Button7) || Input.GetKeyDown(KeyCode.Keypad5))
            {
                P2Ready = !P2Ready;

                if (P2Ready) P2ReadyText.SetActive(true);
                else P2ReadyText.SetActive(false);
            }

            if (P1Ready && P2Ready)
            {
                Step0 = false;
                Step1 = true;

                P1ReadyText.SetActive(false);
                P2ReadyText.SetActive(false);

            }

        }

    }

    void EndStep1()
    {
        Step1 = false;
        Step2 = true;

        DifficultyExplanationText.SetActive(true);
        P1GUIStarter.SetActive(true);
        P2GUIStarter.SetActive(true);

        P1Selector.SetActive(true);

        P2Selector.SetActive(true);
    }

}
