using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class NetworkServerManager : MonoBehaviour
{
    public string ipaddress;
    public Text DebugText;

    void OnGUI()
    {
        ipaddress = LocalIPAddress();
        GUI.Box(new Rect(10, Screen.height - 50, 100, 50), ipaddress);
        GUI.Label(new Rect(20, Screen.height - 35, 100, 20), "Status: " + NetworkServer.active);
        GUI.Label(new Rect(20, Screen.height - 20, 100, 20), "Connected: " + NetworkServer.connections.Count);
    }

    // Start is called before the first frame update
    void Start()
    {
        NetworkServer.Listen(25000);
        NetworkServer.RegisterHandler(888, ServerMessageReceiver);

    }

    void ServerMessageReceiver(NetworkMessage message)
    {
        StringMessage msg = new StringMessage();
        msg.value = message.ReadMessage<StringMessage>().value;
        string[] deltas = msg.value.Split('|');

        switch (deltas[0])
        {
            case "P1":
                switch (deltas[1])
                {
                    case "AnAx":
                        PlayersInputManager.instance.P1Hor = StringToFloat(deltas[2]);
                        PlayersInputManager.instance.P1Ver = StringToFloat(deltas[3]);
                        break;
                    case "Butt":
                        PlayersInputManager.instance.PressedButton(1, deltas[2]);
                        break;
                    case "Gyro":
                        PlayersInputManager.instance.P1RotX = StringToFloat(deltas[2]);
                        PlayersInputManager.instance.P1RotY = StringToFloat(deltas[3]);
                        PlayersInputManager.instance.P1RotZ = StringToFloat(deltas[4]);

                        break;
                }
                break;

            case "P2":
                switch (deltas[1])
                {
                    case "AnAx":
                        PlayersInputManager.instance.P2Hor = float.Parse(deltas[2]);
                        PlayersInputManager.instance.P2Ver = float.Parse(deltas[3]);
                        break;
                    case "Butt":
                        PlayersInputManager.instance.PressedButton(2, deltas[2]);
                        break;
                    case "Gyro":
                        PlayersInputManager.instance.P2RotX = StringToFloat(deltas[2]);
                        PlayersInputManager.instance.P2RotY = StringToFloat(deltas[3]);
                        PlayersInputManager.instance.P2RotZ = StringToFloat(deltas[4]);
                        break;
                }
                break;

            case "P3":
                switch (deltas[1])
                {
                    case "AnAx":
                        PlayersInputManager.instance.P3Hor = float.Parse(deltas[2]);
                        PlayersInputManager.instance.P3Ver = float.Parse(deltas[3]);
                        break;
                    case "Butt":
                        PlayersInputManager.instance.PressedButton(3, deltas[2]);
                        break;
                    case "Gyro":
                        PlayersInputManager.instance.P3RotX = StringToFloat(deltas[2]);
                        PlayersInputManager.instance.P3RotY = StringToFloat(deltas[3]);
                        PlayersInputManager.instance.P3RotZ = StringToFloat(deltas[4]);
                        break;
                }
                break;

            case "P4":
                switch (deltas[1])
                {
                    case "AnAx":
                        PlayersInputManager.instance.P4Hor = float.Parse(deltas[2]);
                        PlayersInputManager.instance.P4Ver = float.Parse(deltas[3]);
                        break;
                    case "Butt":
                        PlayersInputManager.instance.PressedButton(4, deltas[2]);
                        break;
                    case "Gyro":
                        PlayersInputManager.instance.P4RotX = StringToFloat(deltas[2]);
                        PlayersInputManager.instance.P4RotY = StringToFloat(deltas[3]);
                        PlayersInputManager.instance.P4RotZ = StringToFloat(deltas[4]);
                        break;
                }
                break;

            default:
                Debug.Log("Player not recognized");
                break;
        }
    }

    public string LocalIPAddress() { IPHostEntry host; string localIP = ""; host = Dns.GetHostEntry(Dns.GetHostName()); foreach (IPAddress ip in host.AddressList) { if (ip.AddressFamily == AddressFamily.InterNetwork) { localIP = ip.ToString(); break; } } return localIP; }

    float StringToFloat(string ToConvert)
    {
        float Converted = float.Parse(ToConvert);
        while (Converted > 1 || Converted < -1) Converted /= 10;
        return Converted;

    }

}
