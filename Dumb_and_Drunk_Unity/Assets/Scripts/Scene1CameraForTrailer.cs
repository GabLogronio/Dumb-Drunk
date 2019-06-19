using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1CameraForTrailer : MonoBehaviour
{

    public Transform[] pos = new Transform[6];
    private int count = 0;
    public float speed = 1;
    private float step;

    // Start is called before the first frame update
    void Start()
    {
        step = speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            count = (count + 1) % 6;                    
        }
        transform.position = Vector3.MoveTowards(transform.position, pos[count].position, step);
        transform.rotation = Quaternion.Lerp(transform.rotation, pos[count].rotation, step);
    }
}
