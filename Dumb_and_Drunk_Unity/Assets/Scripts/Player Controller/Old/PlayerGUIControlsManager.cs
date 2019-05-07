using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUIControlsManager : MonoBehaviour {

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

    [SerializeField]
    Image[] ControlsImage;

    [SerializeField]
    Image TPose;

    List<GameObject> SpawnedButtons = new List<GameObject>();

    [SerializeField]
    Vector3[] Positions;

    [SerializeField]
    float TimeBetweenSpawn = 1f;

    [SerializeField]
    bool Player1;

    bool Activated = false, Spawning = false, FirstSetup = true;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return) || (Player1 && Input.GetKeyDown(KeyCode.Joystick1Button6)) || (!Player1 && Input.GetKeyDown(KeyCode.Joystick2Button6))) Show();

    }

    public void SetImages(Image[] ToSet)
    {
        ControlsImage = ToSet;

        for (int i = 0; i < ControlsImage.Length; i++)
        {
            if(FirstSetup) ControlsImage[i].GetComponent<RectTransform>().anchoredPosition = Positions[i];
            else ControlsImage[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(Positions[i].x, 350f, 0f);

        }

        FirstSetup = false;
    }

    public void Show()
    {
        if (!Spawning)
        {
            Activated = !Activated;

            for (int i = 0; i < ControlsImage.Length; i++)
            {
                if (Activated)
                {
                    ControlsImage[i].gameObject.SetActive(true);
                    TPose.gameObject.SetActive(true);
                }
                else if (!Activated)
                {
                    ControlsImage[i].gameObject.SetActive(false);
                    TPose.gameObject.SetActive(false);
                }

            }
        }
        else
        {
            Activated = true;

            for (int i = 0; i < ControlsImage.Length; i++)
            {
                ControlsImage[i].gameObject.SetActive(true);
                TPose.gameObject.SetActive(true);

            }
        }

    }

    public void StartSpawning()
    {
        Spawning = true;
        Show();
        Spawn();

    }

    public void StopSpawning()
    {
        Spawning = false;
        Activated = false;
        Show();
        ClearList();

        for (int i = 0; i < ControlsImage.Length; i++)
        {
            ControlsImage[i].GetComponent<RectTransform>().anchoredPosition = Positions[i];
        }
    }

    void Spawn()
    {
        CancelInvoke();

        if (Spawning)
        {
            int Rand = Random.Range(0, 4);

            for (int i = 0; i < ControlsImage.Length; i++)
            {
                if (Controls[i].ButtonImage == ControlsImage[Rand])
                {
                    GameObject NewButton = Instantiate(Controls[i].Spawnable);
                    NewButton.transform.parent = gameObject.transform;
                    Vector3 Position = Controls[i].ButtonImage.GetComponent<RectTransform>().anchoredPosition;
                    Position.y = -650f;
                    NewButton.GetComponent<RectTransform>().anchoredPosition = Position;
                    NewButton.GetComponent<RectTransform>().localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    SpawnedButtons.Add(NewButton);

                }
            }

            Invoke("Spawn", TimeBetweenSpawn);
        }

    }

    void ClearList()
    {
        foreach (GameObject Button in SpawnedButtons)
        {
            Destroy(Button);
        }
        SpawnedButtons.Clear();
    }
}
