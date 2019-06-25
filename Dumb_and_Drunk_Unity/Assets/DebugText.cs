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
    }

    public void Add(string ToAdd)
    {
     //   if (OutText.text.Length > 1000) OutText.text = "";
       // OutText.text += ToAdd + " - ";
    }

    public void Set(string ToAdd)
    {
       // OutText.text = ToAdd;
    }

    public void Audio(string ToAdd)
    {
        // PLAY THE AUDIO
    }

}
