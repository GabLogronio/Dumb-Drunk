using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsManager : MonoBehaviour {

    [System.Serializable]
    public struct ButtonToSpawn
    {
        public Image ButtonImage;
        public GameObject Spawnable;

    }

    [Header("KeyCode connected to Image")]
    [SerializeField]
    ButtonToSpawn[] Controls = new ButtonToSpawn[]
    {
        new ButtonToSpawn { ButtonImage = null, Spawnable = null },
        new ButtonToSpawn { ButtonImage = null, Spawnable = null },
        new ButtonToSpawn { ButtonImage = null, Spawnable = null },
        new ButtonToSpawn { ButtonImage = null, Spawnable = null },

    };

    Image[] ControlsImage;

    [SerializeField]
    Vector3[] Positions;

    [SerializeField]
    GameObject SpawnableButton;

    [SerializeField]
    float TimeBetweenSpawn = 1f;
    float RightLeg = -400f, LeftLeg = -500f, RightArm = -300f, LeftArm = -600f; 

    [SerializeField]
    bool Player1;

    bool Activated = true, Locked = false, Spawning = false;

    void Update() {

        if (Input.GetKeyDown(KeyCode.Return) || (Player1 && Input.GetKeyDown(KeyCode.Joystick1Button7)) || (!Player1 && Input.GetKeyDown(KeyCode.Joystick2Button7))) Show();

    }

    public void SetImages(Image[] ToSet)
    {
        ControlsImage = ToSet;

        for (int i = 0; i < ControlsImage.Length; i++)
        {
            ControlsImage[i].GetComponent<RectTransform>().anchoredPosition = Positions[i];
        }
    }

    void Show()
    {
        Activated = !Activated;

        for (int i = 0; i < ControlsImage.Length; i++)
        {
            if (Activated || Locked) ControlsImage[i].gameObject.SetActive(true);
            else if (!Activated) ControlsImage[i].gameObject.SetActive(false);

        }
    }

    public void StartSpawning()
    {
        Spawning = true;
        Spawn();

    }

    public void StopSpawning()
    {
        Spawning = false;
    }

    void Spawn()
    {
        int Rand = Random.Range(0, 4);

        for (int i = 0; i < ControlsImage.Length; i++)
        {
            if (Controls[i].ButtonImage = ControlsImage[Rand])
            {
                GameObject NewButton = Instantiate(Controls[i].Spawnable);
                Vector3 StartingPosition = Controls[i].ButtonImage.GetComponent<RectTransform>().anchoredPosition;
                StartingPosition.y = 0f;
                StartingPosition.z = 0f;
                NewButton.GetComponent<RectTransform>().anchoredPosition = StartingPosition;

            }
        }

        if (Spawning) Invoke("Spawn", TimeBetweenSpawn);
    }

}
