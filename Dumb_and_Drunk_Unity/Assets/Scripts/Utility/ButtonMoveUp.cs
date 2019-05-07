using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMoveUp : MonoBehaviour {

    float MovementSpeed = 125f, Counter = 0f, FadeOutTime = 2f;

    bool Fading = false;

    Image TextImage;
    Color originalColor;

    void Start()
    {
        TextImage = GetComponent<Image>();
        originalColor = TextImage.color;

        FadeOutTime = 80f / MovementSpeed;

    }

    // Update is called once per frame
    void Update () {

        transform.Translate(Vector3.up * Time.deltaTime * MovementSpeed);

        if (Fading && Counter < FadeOutTime)
        {
            Counter += Time.deltaTime;
            originalColor.a = 1 - Counter / FadeOutTime;
            TextImage.color = originalColor;
        }
        else if (Counter >= FadeOutTime) Destroy(gameObject);
	}

    public void FadeOut()
    {
        Fading = true;
    }

}
