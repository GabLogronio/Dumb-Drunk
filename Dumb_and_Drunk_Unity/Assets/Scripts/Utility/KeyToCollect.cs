using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyToCollect : MonoBehaviour {

    [SerializeField]
    Gate LevelGate;

    [SerializeField]
    float RotationSpeed;

    void Update()
    {
        transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9 || other.gameObject.layer == 10)
        {
            LevelGate.CollectedKey();
            gameObject.SetActive(false);
        }
    }

}
