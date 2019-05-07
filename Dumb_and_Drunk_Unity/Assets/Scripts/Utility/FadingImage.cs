using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingImage : MonoBehaviour {

    [SerializeField]
    float Delay, FadeOutTime, Speed;
    float counter = 0f;
    bool fading = false;
    Image TextImage;
    Color originalColor;
    Vector3 StartingPosition;

    void Start()
    {
        TextImage = GetComponent<Image>();
        originalColor = TextImage.color;
        originalColor.a = 0.0f;
        TextImage.color = originalColor;
        StartingPosition = transform.position;
    }

    void Update()
    {
        if (fading && counter < FadeOutTime)
        {
            counter += Time.deltaTime;
            originalColor.a = 1 - counter / FadeOutTime;
            TextImage.color = originalColor;
            transform.position += transform.up * Speed * Time.deltaTime;

        }
        else if (counter >= FadeOutTime) Deactivate();
    }

    public void Activate()
    {
        Color originalColor = TextImage.color;
        originalColor.a = 1f;
        TextImage.color = originalColor;
        CancelInvoke();

        Invoke("StartFadeout", Delay);
    }

    void StartFadeout()
    {
        fading = true;
    }

    void Deactivate()
    {
        Color originalColor = TextImage.color;
        originalColor.a = 0f;
        TextImage.color = originalColor;
        transform.position = StartingPosition;
        fading = false;
        counter = 0f;
    }

}
