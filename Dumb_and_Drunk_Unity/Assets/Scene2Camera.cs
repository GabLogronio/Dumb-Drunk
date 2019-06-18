using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2Camera : MonoBehaviour
{
    [SerializeField]
    LayerMask[] PlayersLayers = new LayerMask[4];

    GameObject Player1, Player2;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < PlayersLayers.Length; i++)
        {
            Collider[] Player = new Collider[1];
            Physics.OverlapSphereNonAlloc(transform.position, 25, Player, PlayersLayers[i]);
            if (Player[0] != null)
            {
                if (Player[0].gameObject.transform.position.z > transform.position.z)
                {
                    if (Player1 != null) Player1 = Player[0].gameObject.GetComponent<PlayerObstacleManager>().GetPlayerController();
                    else Player2 = Player[0].gameObject.GetComponent<PlayerObstacleManager>().GetPlayerController();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Player1 && Player2)
        {
            transform.position = new Vector3( 0, 7.5f, Mathf.Min(Player1.transform.position.z, Player2.transform.position.z) - 9.5f);
        }
    }
}
