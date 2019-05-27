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

    [SerializeField]
    InputManager[] Characters;

    Dictionary<int, InputManager> CurrentConnections = new Dictionary<int, InputManager>();

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

        NetworkServer.RegisterHandler(888, ServerStringMessageReceiver);
        NetworkServer.RegisterHandler(MsgType.Connect, OnClientConnected);

    }

    void OnClientConnected(NetworkMessage NetMsg)
    {
        //if (CurrentConnections.Count <= Characters.Length) <---------- TO PUT BACK
        if (CurrentConnections.Count <= 2)
            {
            CurrentConnections.Add(NetMsg.conn.connectionId, Characters[CurrentConnections.Count]);
            ServerStringMessageSender(NetMsg.conn.connectionId, "Player|" + NetMsg.conn.connectionId);
            //if (CurrentConnections.Count == Characters.Length)  <---------- TO PUT BACK
            if (CurrentConnections.Count == 1) 
            {
                foreach (int ConnectionID in CurrentConnections.Keys)
                {
                    ServerStringMessageSender(ConnectionID, "Start");
                }
            }
        }
    }

    void ServerStringMessageSender (int ConnectionID, string ToSend)
    {
        StringMessage msg = new StringMessage();
        msg.value = ToSend;
        NetworkServer.SendToClient(ConnectionID, 888, msg);
    }

    void ServerStringMessageReceiver(NetworkMessage NetMsg)
    {
        StringMessage msg = new StringMessage();
        msg.value = NetMsg.ReadMessage<StringMessage>().value;
        string[] deltas = msg.value.Split('|');

        switch (deltas[0])
        {
            case "AnAx":
                CurrentConnections[NetMsg.conn.connectionId].SetAnalogAxis(StringToFloat(deltas[1]), StringToFloat(deltas[2]));
                break;
            case "Butt":
                CurrentConnections[NetMsg.conn.connectionId].PressedButton(deltas[1], deltas[2] == "Down");
                break;
            case "Gyro":
                CurrentConnections[NetMsg.conn.connectionId].SetGyroscope(StringToFloat(deltas[1]), StringToFloat(deltas[2]));
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

    public void SwitchInputManager(int i)
    {
        if(Characters[i].gameObject.GetComponent<PlayerInputManager>().enabled)
        {
            Characters[i].gameObject.GetComponent<ShooterInputManager>().enabled = true;
            Characters[i].gameObject.GetComponent<PlayerInputManager>().enabled = false;
            Characters[i] = Characters[i].gameObject.GetComponent<ShooterInputManager>();
        }
        else
        {
            Characters[i].gameObject.GetComponent<PlayerInputManager>().enabled = true;
            Characters[i].gameObject.GetComponent<ShooterInputManager>().enabled = false;
            Characters[i] = Characters[i].gameObject.GetComponent<PlayerInputManager>();
        }
    }

}
