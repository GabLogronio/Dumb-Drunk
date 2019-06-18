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

    // Start is called before the first frame update
    void Start()
    {
        //search();
        winners = MatchManager.getInstance().GetWinnersObjects();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!found) search();

        /*if (Player1 && Player2)
        {
            transform.position = new Vector3( 0, 7.5f, Mathf.Min(Player1.transform.position.z, Player2.transform.position.z) - 9.5f);
        }*/

        transform.position = new Vector3(0, 7.5f, winners[0].transform.position.z - 9.5f);
        Debug.Log("seguo: " + winners[0].layer);
    }

    /*private void search()
    {
        for (int i = 0; i < PlayersLayers.Length; i++)
        {
            Collider[] Player = new Collider[1];
            Physics.OverlapSphereNonAlloc(transform.position, 50, Player, PlayersLayers[i]);
            if (Player[0] != null)
            {
                if (Player[0].gameObject.transform.position.z > transform.position.z)
                {
                    if (Player1 != null) Player1 = Player[0].gameObject.GetComponent<PlayerObstacleManager>().GetPlayerController();
                    else Player2 = Player[0].gameObject.GetComponent<PlayerObstacleManager>().GetPlayerController();
                }
            }
        }
    }*/

    /*public void SetWinners(GameObject[] w)
    {
        winners = w;
    }*/
}
