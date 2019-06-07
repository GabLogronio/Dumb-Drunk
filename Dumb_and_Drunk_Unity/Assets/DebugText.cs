using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
    Text OutText;

    public static DebugText instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        OutText = GetComponent<Text>();
        Log("started\n");
    }

    public void Log(string ToAdd)
    {
        if (OutText.text.Length > 1000) OutText.text = "";
        OutText.text += ToAdd + " - ";
    }

}
