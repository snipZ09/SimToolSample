using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackGround;

public class ServerThread : MonoBehaviour
{
    public static TCPCommunication tcp;
    public static UDPCommunication udp;
    public static SerialComunication serial;

    // Start is called before the first frame update
    void Start()
    {
        serial = new SerialComunication();
        serial.StartThread();
        tcp = new TCPCommunication(6000);
        tcp.StartThread();
        udp = new UDPCommunication();
        udp.StartThread();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
