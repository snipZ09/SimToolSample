using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TcpToServer;
using UnityEngine;

public class TCPOp1 : MonoBehaviour
{
    static TcpClient client;
    static NetworkStream stream;
    private static bool isConnect;

    public static void Connect()
    {
        try
        {
            // Create a TcpClient.
            // Note, for this client to work you need to have a TcpServer
            // connected to the same address as specified by the server, port
            // combination.
            Int32 port = 6000;
            String server = "127.0.0.1";
            String message = "Hello bae";
            // Prefer using declaration to ensure the instance is Disposed later.
            client = new TcpClient(server, port);

            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

            // Get a client stream for reading and writing.
            stream = client.GetStream();

            // Send the message to the connected TcpServer.
            stream.Write(data, 0, data.Length);

            Console.WriteLine("Sent: {0}", message);

            //// Receive the server response.

            //// Buffer to store the response bytes.
            //data = new Byte[256];

            //// String to store the response ASCII representation.
            //String responseData = String.Empty;

            //// Read the first batch of the TcpServer response bytes.
            //Int32 bytes = stream.Read(data, 0, data.Length);
            //responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            //Console.WriteLine("Received: {0}", responseData);

            // Explicit close is not necessary since TcpClient.Dispose() will be
            // called automatically.
            // stream.Close();
            // client.Close();
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine("ArgumentNullException: {0}", e);
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
    }

    public static void CloseConnect()
    {
        stream.Close();
        client.Close();
    }

    private static bool SendMessageToServer(string str)
    {
        bool flag = true;
        if (flag)
        {
            try
            {
                MonoBehaviour.print("send str" + str);
                Debug.Log("send str" + str);
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(str);
                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);
                //int num = TCPOp.clientSocket.Send(bytes);
                return true;
            }
            catch (Exception ex)
            {
                isConnect = false;
                Debug.Log("send exception" + ex.GetBaseException().ToString());
                return false;
            }
        }
        else
        {
            Debug.Log("flag == false");
        }
        
        return false;
    }

    public static bool RightHandedRotation(int spreedAngle)
    {
        spreedAngle = Math.Abs(spreedAngle);
        return SendMessageToServer(TCPProtocol.BrushlessElectricMachine("AirCraft", ElectricPattern.Spread, spreedAngle, 0));
    }

    // Token: 0x06000008 RID: 8 RVA: 0x00002394 File Offset: 0x00000594
    public static bool LeftHandedRotation(int spreedAngle)
    {
        spreedAngle = Math.Abs(spreedAngle);
        Debug.Log("LeftHandedRotation:" + spreedAngle);
        return SendMessageToServer(TCPProtocol.BrushlessElectricMachine("AirCraft", ElectricPattern.Spread, spreedAngle * -1, 0));
    }

    // Token: 0x06000009 RID: 9 RVA: 0x000023C4 File Offset: 0x000005C4
    public static bool RightPositionRotation(int speed, int pos)
    {
        speed = Math.Abs(speed);
        return SendMessageToServer(TCPProtocol.BrushlessElectricMachine("AirCraft", ElectricPattern.Position, speed, pos));
    }

    // Token: 0x0600000A RID: 10 RVA: 0x000023F0 File Offset: 0x000005F0
    public static bool LeftPositionRotation(int speed, int pos)
    {
        speed = Math.Abs(speed);
        return SendMessageToServer(TCPProtocol.BrushlessElectricMachine("AirCraft", ElectricPattern.Position, speed * -1, pos));
    }

    // Token: 0x0600000B RID: 11 RVA: 0x00002420 File Offset: 0x00000620
    public static bool StopRotation()
    {
        return SendMessageToServer(TCPProtocol.BrushlessElectricMachine("AirCraft", ElectricPattern.Spread, 0, 0));
    }

    // Token: 0x0600000C RID: 12 RVA: 0x00002444 File Offset: 0x00000644
    public static bool Fan(int num)
    {
        return SendMessageToServer(TCPProtocol.SerialSpecialEffects("AirCraft", num));
    }

    public static bool OpenMouseKey()
    {
        return SendMessageToServer(TCPProtocol.SerialMouseKeyboardEnable("AirCraft"));
    }

    // Token: 0x0600000E RID: 14 RVA: 0x0000248C File Offset: 0x0000068C
    public static bool CloseMouseKey()
    {
        return SendMessageToServer(TCPProtocol.SerialMouseKeyBoardDisable("AirCraft"));
    }

    // Token: 0x0600000F RID: 15 RVA: 0x000024B0 File Offset: 0x000006B0
    public static bool SerialRestart()
    {
        return SendMessageToServer(TCPProtocol.SerialRestart("AirCraft"));
    }

    // Token: 0x06000010 RID: 16 RVA: 0x000024D4 File Offset: 0x000006D4
    public static bool ElectricCylinderDistance(int time, int distance, bool isShake = false)
    {
        return SendMessageToServer(TCPProtocol.ElectricCylinderDistance("AirCraft", isShake, time, distance, 0, 0, 0, 0, 0));
    }

    // Token: 0x06000011 RID: 17 RVA: 0x00002500 File Offset: 0x00000700
    public static bool ElectricCylinderSin(int time, int distance, bool isShake = false)
    {
        return SendMessageToServer(TCPProtocol.ElectricCylinderSin("AirCraft", isShake, time, distance));
    }

    // Token: 0x06000012 RID: 18 RVA: 0x00002524 File Offset: 0x00000724
    public static bool WirelessSwimFunction(WirelessSwim fun)
    {
        return SendMessageToServer(TCPProtocol.WirelessSwimFunction("WirelessSwin", fun));
    }

    // Token: 0x06000013 RID: 19 RVA: 0x00002548 File Offset: 0x00000748
    public static bool GetPlayerID()
    {
        return SendMessageToServer(TCPProtocol.GetPlayerID("AirCraft"));
    }
}
