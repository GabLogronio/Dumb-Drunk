using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class NetworkClientManager : MonoBehaviour
{
    public static NetworkClientManager instance = null;

    [SerializeField]
    string ServerIP = "192.168.1.6", PlayerNumber = "0";

    [SerializeField]
    Text textUI;

    [SerializeField]
    GameObject GameUI, ConnectButton;

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
        

    }

    public void SendJoystickInfo(float HorDelta, float VerDelta)
    {
        if (client.isConnected)
        {
            //textUI.text = "Sending: P" + PlayerNumber + "|" + HorDelta + "|" + VerDelta;
            StringMessage msg = new StringMessage();
            msg.value = "P" + PlayerNumber + "|AnAx|" + HorDelta + "|" + VerDelta;
            client.Send(888, msg);

        }
    }

    public void SendButtonInfo(string ButtonPressed)
    {
        if (client.isConnected)
        {
            //textUI.text = "Sending: P" + PlayerNumber + "|" + HorDelta + "|" + VerDelta;
            StringMessage msg = new StringMessage();
            msg.value = "P" + PlayerNumber + "|Butt|" + ButtonPressed;
            client.Send(888, msg);

        }
    }

    public void SendGyroscopeInfo(Quaternion Delta)
    {
        if (client.isConnected)
        {   // Delta.x > 0 tilted backwards, Delta.x < 0 tilted forwatd, Delta.y > 0 tilted left, Delta.y < 0 tilted right, *(-1) in order to align the y value.

            StringMessage msg = new StringMessage();
            msg.value = "P" + PlayerNumber + "|Gyro|" + System.Math.Round(Delta.x, 2) + "|" + System.Math.Round(Delta.y, 2)*(-1f) + "|" + System.Math.Round(Delta.z, 2) + "|" + System.Math.Round(Delta.w, 2);
            textUI.text = "Sending: " + msg.value;
            client.Send(888, msg);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (client.isConnected)
        {
            // textUI.text = "Connected";
            GameUI.SetActive(true);
            ConnectButton.SetActive(false);
        }
    }
}
