using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class NetworkServerManager : MonoBehaviour
{
    // public string ipaddress;
    // public Text DebugText;
    private static NetworkServerManager instance = null;

    [SerializeField]
    InputManager[] PlayersInputManagers = new InputManager[4];

    [SerializeField]
    Text IPAddressText;

    [SerializeField]
    RawImage[] PlayersImages = new RawImage[4];

    [SerializeField]
    Vector3[] PlayersPosition = new Vector3[4];

    Dictionary<int, InputManager> CurrentConnections = new Dictionary<int, InputManager>();

    void OnGUI()
    {
        //GUI.Box(new Rect(10, Screen.height - 50, 100, 50), LocalIPAddress());

    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this);
        DontDestroyOnLoad(gameObject);

        NetworkServer.Listen(25000);

        NetworkServer.RegisterHandler(888, ServerStringMessageReceiver);
        NetworkServer.RegisterHandler(MsgType.Connect, OnClientConnected);

        IPAddressText.text = LocalIPAddress();

    }

    public static NetworkServerManager getInstance()
    {
        return instance;
    }

    private void Update()
    {
        //TestingInput();
    }

    void OnClientConnected(NetworkMessage NetMsg)
    {
        //if (CurrentConnections.Count <= Characters.Length) <---------- TO PUT BACK
        if (CurrentConnections.Count <= 4)
            {
            CurrentConnections.Add(NetMsg.conn.connectionId, PlayersInputManagers[CurrentConnections.Count]);
            ServerStringMessageSender(CurrentConnections[NetMsg.conn.connectionId], "Player|" + NetMsg.conn.connectionId);
            PlayersImages[CurrentConnections.Count - 1].GetComponent<BouncingFace>().SetImage(PlayersPosition[CurrentConnections.Count - 1]);
            //if (CurrentConnections.Count == Characters.Length)  <---------- TO PUT BACK
            if (CurrentConnections.Count == 1) 
            {
                foreach (int ConnectionID in CurrentConnections.Keys)
                {
                    ServerStringMessageSender(CurrentConnections[ConnectionID], "Start");
                }
                MatchManager.getInstance().LoadMatchmakingScene();
            }
        }
    }

    public void ServerStringMessageSender(int i, string ToSend)
    {
        ServerStringMessageSender(PlayersInputManagers[i], ToSend);
    }

    public void ServerStringMessageSender (InputManager Player, string ToSend)
    {
        StringMessage msg = new StringMessage();
        msg.value = ToSend;
        NetworkServer.SendToClient(CurrentConnections.First(ConnId => ConnId.Value == Player).Key, 888, msg);
    }

    public void ServerStringMessageSenderToAll(string ToSend)
    {
        foreach (int ConnectionID in CurrentConnections.Keys)
        {
            ServerStringMessageSender(CurrentConnections[ConnectionID], ToSend);
        }
    }

    void ServerStringMessageReceiver(NetworkMessage NetMsg)
    {
        
        StringMessage msg = new StringMessage();
        msg.value = NetMsg.ReadMessage<StringMessage>().value;
        //DebugText.instance.Log(msg.value);
        string[] deltas = msg.value.Split('|');

        switch (deltas[0])
        {
            case "AnAx":
                float Hor = Mathf.Round(float.Parse(deltas[1]) * 100f) / 100f, Ver = Mathf.Round(float.Parse(deltas[2]) * 100f) / 100f;
                CurrentConnections[NetMsg.conn.connectionId].SetAnalogAxis(Hor, Ver);
                break;
            case "Butt":
                CurrentConnections[NetMsg.conn.connectionId].PressedButton(deltas[1], deltas[2] == "Down");
                //DebugText.instance.Log(CurrentConnections[NetMsg.conn.connectionId].gameObject.name + "pressed " + deltas[1]);
                break;
            case "Gyro":
                CurrentConnections[NetMsg.conn.connectionId].SetGyroscope(deltas[1][0], deltas[2][0], deltas[3][0], deltas[4][0]);
                //DebugText.instance.Log("ricevuto gyro");
                break;
            case "GetUp":
                CurrentConnections[NetMsg.conn.connectionId].Fallen(false);
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

    public void SwitchInputManager(int i, bool player)
    {
        PlayersInputManagers[i].gameObject.GetComponent<PlayerInputManager>().enabled = player;
        PlayersInputManagers[i].gameObject.GetComponent<ShooterInputManager>().enabled = !player;
        if (player) PlayersInputManagers[i] = PlayersInputManagers[i].gameObject.GetComponent<PlayerInputManager>();
        else PlayersInputManagers[i] = PlayersInputManagers[i].gameObject.GetComponent<ShooterInputManager>();
    }

    void TestingInput() // ONLY USED IN TESTING
    {
        // FOR TESTING PURPOSE ONLY --------------------------------------------------

        if (Input.GetKeyDown(KeyCode.JoystickButton0)) PlayersInputManagers[0].PressedButton("Green", true);
        if (Input.GetKeyUp(KeyCode.JoystickButton0)) PlayersInputManagers[0].PressedButton("Green", false);

        if (Input.GetKeyDown(KeyCode.JoystickButton1)) PlayersInputManagers[0].PressedButton("Red", true);
        if (Input.GetKeyUp(KeyCode.JoystickButton1)) PlayersInputManagers[0].PressedButton("Red", false);

        if (Input.GetKeyDown(KeyCode.JoystickButton2)) PlayersInputManagers[0].PressedButton("Blue", true);
        if (Input.GetKeyUp(KeyCode.JoystickButton2)) PlayersInputManagers[0].PressedButton("Blue", false);

        if (Input.GetKeyDown(KeyCode.JoystickButton3)) PlayersInputManagers[0].PressedButton("Yellow", true);
        if (Input.GetKeyUp(KeyCode.JoystickButton3)) PlayersInputManagers[0].PressedButton("Yellow", false);

        PlayersInputManagers[0].SetAnalogAxis(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        PlayersInputManagers[0].SetGyroscope(BoolToChar(Input.GetAxis("RightStickHor") > 0.5f), BoolToChar(Input.GetAxis("RightStickHor") < -0.5f), BoolToChar(Input.GetAxis("RightStickVer") < -0.5f), BoolToChar(Input.GetAxis("RightStickVer") > 0.5f));

        //Hor = Input.GetAxis("Horizontal");
        //Ver = Input.GetAxis("Vertical");

        // FOR TESTING PURPOSE ONLY --------------------------------------------------
    }

    char BoolToChar(bool ToSet)
    {
        if (ToSet) return 'T';
        else return 'F';
    }

}
