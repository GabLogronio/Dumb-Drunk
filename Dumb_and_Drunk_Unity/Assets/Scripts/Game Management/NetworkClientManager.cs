using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System;

public class NetworkClientManager : MonoBehaviour
{
    public static NetworkClientManager instance = null;

    [SerializeField]
    string ServerIP = "192.168.1.6";

    int PlayerID;

    [SerializeField]
    Text textUI;

    [SerializeField]
    GameObject[] PlayersImages;

    [SerializeField]
    GameObject Scene1UI, Scene2UI, ConnectButton;

    NetworkClient client;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        client = new NetworkClient();
    }

    public void ConnectToServer()
    {
        client.Connect(ServerIP, 25000);
        textUI.text = "Connecting";

        client.RegisterHandler(888, ServerMessageReceiver);

    }

    private void ServerMessageReceiver(NetworkMessage NetMsg)
    {
        StringMessage msg = new StringMessage();
        msg.value = NetMsg.ReadMessage<StringMessage>().value;
        textUI.text = msg.value;
        string[] deltas = msg.value.Split('|');
        switch (deltas[0])
        {
            case "Player":
                int ToSetID;
                if (Int32.TryParse(deltas[1], out ToSetID))
                {
                    PlayerID = ToSetID;
                    PlayersImages[PlayerID - 1].SetActive(true);
                    ConnectButton.SetActive(false);

                }
                break;
            case "Start":
                Scene1UI.SetActive(true);
                PlayersImages[PlayerID - 1].SetActive(false);
                break;
            case "Scene1":
                Scene1UI.SetActive(true);
                Scene2UI.SetActive(false);
                break;
            case "Scene2":
                Scene1UI.SetActive(false);
                Scene2UI.SetActive(true);
                break;
            default:
                Debug.Log("Message");
                break;
        }
    }

    public void SendJoystickInfo(float HorDelta, float VerDelta)
    {
        if (client.isConnected)
        {
            //textUI.text = "Sending: P" + PlayerNumber + "|" + HorDelta + "|" + VerDelta;
            StringMessage msg = new StringMessage();
            msg.value = "AnAx|" + HorDelta + "|" + VerDelta;
            client.Send(888, msg);

        }
    }

    public void SendButtonInfo(string ButtonPressed)
    {
        if (client.isConnected)
        {
            //textUI.text = "Sending: P" + PlayerNumber + "|" + HorDelta + "|" + VerDelta;
            StringMessage msg = new StringMessage();
            msg.value = "Butt|" + ButtonPressed;
            client.Send(888, msg);

        }
    }

    public void SendGyroscopeInfo(Quaternion Delta)
    {
        if (client.isConnected)
        {   // Delta.x > 0 tilted backwards, Delta.x < 0 tilted forwatd, Delta.y > 0 tilted left, Delta.y < 0 tilted right, *(-1) in order to align the y value.

            StringMessage msg = new StringMessage();
            // OLD msg.value = "Gyro|" + System.Math.Round(Delta.x, 2) + "|" + System.Math.Round(Delta.y, 2)*(-1f) + "|" + System.Math.Round(Delta.z, 2) + "|" + System.Math.Round(Delta.w, 2);
            msg.value = "Gyro|" + System.Math.Round(Delta.x, 2) + "|" + System.Math.Round(Delta.y, 2) * (-1f);
            client.Send(888, msg);

        }
    }

}
