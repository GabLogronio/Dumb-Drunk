using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BouncingFace : MonoBehaviour
{
    [SerializeField]
    float MovementSpeed = 10f;

    bool Hor = true, Ver = true, Moving = true;

    RectTransform rt;

    private void Start()
    {
        Hor = Random.value > 0.5f;
        Hor = Random.value > 0.5f;

        rt = GetComponent<RectTransform>();

    }

    private void Update()
    {
        if (Moving)
        {
            if (Hor) rt.localPosition += new Vector3(MovementSpeed * Time.deltaTime, 0f, 0f);
            else rt.localPosition += new Vector3(-MovementSpeed * Time.deltaTime, 0f, 0f);

            if (Ver) rt.localPosition += new Vector3(0f, MovementSpeed * Time.deltaTime, 0f);
            else rt.localPosition += new Vector3(0f, -MovementSpeed * Time.deltaTime, 0f);
        }

    }

    public void ChangeDirection(bool dir)
    {
        if (dir) Hor = !Hor;
        else Ver = !Ver;

    }

    public void SetImage(Vector3 position)
    {
        Moving = false;
        rt.localPosition = position;
        var tempColor = GetComponent<RawImage>().color;
        tempColor.a = 1f;
        GetComponent<RawImage>().color = tempColor;
    }

}
