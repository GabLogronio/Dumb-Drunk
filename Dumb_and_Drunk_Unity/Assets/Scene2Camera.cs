using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2Camera : MonoBehaviour
{
    [SerializeField]
    LayerMask[] PlayersLayers = new LayerMask[4];

    GameObject Player1, Player2;
    private bool found = false;
    private GameObject[] winners = new GameObject[2];

    void Start()
    {
        winners = MatchManager.getInstance().GetWinnersObjects();
    }

    void Update()
    {

        if(winners[0] && winners[1])
            transform.position = new Vector3(0, 7.5f, Mathf.Min( winners[0].transform.position.z, winners[1].transform.position.z) - 9.5f);
        else transform.position = new Vector3(0, 7.5f, winners[0].transform.position.z - 9.5f);

    }
}
