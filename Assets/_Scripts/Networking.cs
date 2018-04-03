using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using System;
using UnityEngine;

public class Networking : MonoBehaviour {

    enum code_recv
    {
        //Client was not initalized or failed to start
        UNINITALIZED,

        //Recv codes
        RECEIVED,
        RECV_CALLED,
        ACTIVE_THREAD,
        NO_SERVER_CONNECTION,

        //Server connection codes
        CONNECTED,
        CONNECTION_DENIED,
        WAITING_FOR_RESPONSE,
        GAME_DATA,
        INVALID_DATA,
        NO_MESSAGE

    };

    #region Imports
    [DllImport("client")]
    static extern void init(int bufferSize);

    [DllImport("client")]
    static extern bool connectToUDP(byte[] address, int size);

    //pings the server to let it now we exits
    //This will return true when we have recieved a confirmation from the server
    //if we pass it true the function will hang until the connection times out or we connect to the server
    [DllImport("client")]
    static extern int connectToServer(bool hang = false);

    //ping the server to let it know we are disconnecting
    //this will return true when we have received a confirmation from the server
    //If we pas it true the function will hang until the connection times out or we connect to the server
    [DllImport("client")]
    static extern int disconnectFromServer(bool hang = false);
    //This will return true when the game officially starts
    static extern bool startGame();

    //Will return true when the buffer has been filled
    //Buffer is filled with names of clients and comma seperated
    //if hang is true the funtion will hang until the connection times out or we connect to the server 
    [DllImport("client")]
    static extern int getClients(char[] buffer, bool hang = false);

    //check for UDP message, save into given buffer
    //returns an int that will tell you what state the threaded function is in
    [DllImport("client")]
    static extern int receiveUDP(char[] buffer);
    //Send message via UDP
    [DllImport("client")]
    static extern void sendUDP(byte[] msg);

    [DllImport("client")]
    static extern void shutDownClient();
    #endregion

    char []msgContainer;
    int size;
    bool isConnected;

    public Controls controller;
    public Rigidbody rigidbody;
    public PlayerStats playerStats;
    public PlayerStats[] allPlayers;
    public Rigidbody[] allRigidbodies;
    public void initUDP()
    {
        isConnected = false; 
        size = 512;
        init(size);
        msgContainer = new char[size];

        byte[] msg = System.Text.Encoding.ASCII.GetBytes("UOSL17-1JHF4H2\0");
        Debug.Log(connectToUDP(msg, msg.Length));
        code_recv connectionState;

        if (isConnected == false)
        {
            if ((connectionState = (code_recv)connectToServer(true)) == code_recv.CONNECTED)
            {
                Debug.Log("Connected succesfully");
                isConnected = true;
            }
            Debug.Log("Failed to connect with " + connectionState.ToString());
        }
        //write hang function so we can all join in
    }

    public byte[] getBytes(string msg)
    {
        return System.Text.Encoding.ASCII.GetBytes(msg + "\0");
    }

    public String getData()
    {
        return "";
    }

    public void sendData()
    {
        return;
    }
    // Use this for initialization
    void Start () {

        controller = GetComponent<Controls>();
        rigidbody = GetComponent<Rigidbody>();
        playerStats = GetComponent<PlayerStats>();

        allPlayers = GetComponentInParent<Unique>().GetComponentsInChildren<PlayerStats>();
        allRigidbodies = new Rigidbody[allPlayers.Length];
        for (int i =0; i < allPlayers.Length; i++)
        {
            allRigidbodies[i] = allPlayers[i].GetComponent<Rigidbody>();
        }


        if (playerStats.m_PlayerID == 0)
        {
            initUDP();
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (playerStats.m_PlayerID == 0)
        {
            //msg format
            // comma's seperate values in a whole
            // Semi colons seperate items position;rotation;
            String msg;
            byte idCode = 1;
            msg = idCode + getString(rigidbody.position) + ";" + getString(rigidbody.rotation.eulerAngles) + ";";

            if (controller.GetShootLaser())
            {
                //get aim
                msg += controller.GetAim() + ";";
            }
            else
            {
                //aim vector and then 
                msg += " ;";
            }

            if (controller.GetBoost())
            {
                msg += controller.GetMove();
            }
            else
            {
                msg += " ";
            }
            sendUDP(getBytes(msg));

            if (receiveUDP(msgContainer) == 1)
            {
                string conversion = msgContainer.ToString();
                Debug.Log(conversion);
                string []parts = conversion.Split(':');
                for (int i = 0; i < allRigidbodies.Length; i++)
                {
                    if (allRigidbodies[i] != rigidbody)
                    {
                        string[] messages = parts[i].Split(';');
                        //postion
                        allRigidbodies[i].position = getVector3(parts[0]);

                        //rotation
                        allRigidbodies[i].rotation = Quaternion.Euler(getVector3(parts[1]));

                        //controller aim + shooting
                        if (!parts[2].Equals(" "))
                        {

                        }

                        //movement + boost
                        if (!parts[2].Equals(" "))
                        {

                        }
                    }
                }
            }
        }


    }

    public String getString(Vector3 vec)
    {
        return vec.x.ToString("G4") + "," + vec.y.ToString("G4") + "," + vec.z.ToString("G4");
    }
    public String getString(Vector2 vec)
    {
        return vec.x.ToString("G4") + "," + vec.y.ToString("G4");
    }

    public Vector3 getVector3(string msg)
    {
        Vector3 value;
        String[] parse = msg.Split(',');
        value.x = float.Parse(parse[0]);
        value.y = float.Parse(parse[1]);
        value.z = float.Parse(parse[2]);
        return value;
    }

    public Vector2 getVector2(string msg)
    {
        Vector2 value;
        String[] parse = msg.Split(',');
        value.x = float.Parse(parse[0]);
        value.y = float.Parse(parse[1]);
        return value;
    }

    void OnDestroy()
    {
        if (playerStats.m_PlayerID == 0)
        {
            shutDownClient();
        }
    }
}
