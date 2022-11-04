using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alta.Plugin;
using System.IO;
using System;
using System.Text;
using System.Net;
using _360PTF;
using System.Net.Sockets;

public class Demo : MonoBehaviour
{
    public Setting setting;
    private string filePath;
    Vector2 moveInput;
    public int xOffset, yOffset, time;
    int curRotateAngle, curHeightAngle;
    private void Awake()
    {
        filePath = Path.Combine(Application.streamingAssetsPath, "Settings.xml");
        ReadXML();
    }

    void ReadXML()
    {
        if (File.Exists(filePath))
        {
            setting = XmlExtention.Read<Setting>(filePath);
            this.xOffset = setting.xOffset;
            this.yOffset = setting.yOffset;
            this.time = setting.time;
        }
        else
        {
            setting = new Setting() { };
            WriteToXML();
        };
    }

    void WriteToXML()
    {
        XmlExtention.Write(setting, filePath);
    }
    public static string tempStr;
    public static int tempInt;
    public static void SendData(int rotateAngle, int heightAngle, int millisecond = 20)
    {
        bool flag = rotateAngle >= 390;
        if (flag)
        {
            rotateAngle = 390;
        }
        bool flag2 = rotateAngle <= -390;
        if (flag2)
        {
            rotateAngle = -390;
        }
        bool flag3 = heightAngle >= 32;
        if (flag3)
        {
            heightAngle = 32;
        }
        bool flag4 = heightAngle <= 0;
        if (flag4)
        {
            heightAngle = 0;
        }
        StringBuilder stringBuilder = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        tempStr = millisecond.ToString("X8");
        stringBuilder.Append(tempStr + " ");
        tempInt = rotateAngle * 1570;
        tempStr = tempInt.ToString("X8");
        stringBuilder.Append(tempStr + " ");
        tempInt = heightAngle * 26830;
        tempStr = tempInt.ToString("X8");
        stringBuilder.Append(tempStr + " ");
        tempInt = 0;
        tempStr = tempInt.ToString("X8");
        stringBuilder.Append(tempStr + " ");
        stringBuilder2.Append("55 aa 00 00 14 01 00 00 ff ff ff ff 00 00 00 01");
        stringBuilder2.Append(stringBuilder);
        stringBuilder2.Append("12 34 56 78 ab cd");
        Debug.Log(stringBuilder2.ToString());
        sendMsg(stringBuilder2.ToString());
    }
    private static IPEndPoint CylinderPoint = null;
    public static readonly IPAddress Broadcast = IPAddress.Parse("255.255.255.255");
    // Token: 0x06000002 RID: 2 RVA: 0x000021A8 File Offset: 0x000003A8
    private static void sendMsg(string str)
    {
        byte[] array = strToToHexByte(str);
        foreach(var ele in array)
        {
            Debug.Log(ele);
        }

        bool flag = CylinderPoint == null;
        if (flag)
        {
            try
            {
                CylinderPoint = new IPEndPoint(IPAddress.Broadcast, 7408);
            }
            catch (Exception)
            {
                CylinderPoint = null;
            }
        }
        bool flag2 = CylinderPoint == null;
        if (!flag2)
        {
            try
            {
                UdpClient udpClient = new UdpClient();
                udpClient.Send(array, array.Length, CylinderPoint);
            }
            catch (Exception ex)
            {
            }
        }
    }

    private static byte[] strToToHexByte(string hexString)
    {
        hexString = hexString.Replace(" ", "");
        bool flag = hexString.Length % 2 != 0;
        if (flag)
        {
            hexString += " ";
        }
        byte[] array = new byte[hexString.Length / 2];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
        }
        return array;
    }

    private void Start()
    {
        //var rotateAngle = 0;
        //rotateAngle *= 26830;
        //int i = Convert.ToInt32("0002045E", 16);
        //Debug.Log(rotateAngle);
        //Debug.Log("x8 " + rotateAngle.ToString("x8"));
        //var arrays = strToToHexByte("00003D54");
        //foreach (byte element in arrays)
        //{

        //Debug.Log("Hex byte " + element);
        //}
        //Debug.Log("byte " + Convert.ToByte("00012C", 8));
        //SendData(0, 0, 15700);
        //Debug.Log("Int " + Convert.ToInt32("00009C40", 16));
    }

    private void Update()
    {
        //moveInput.x = Input.GetAxis("Horizontal");
        //moveInput.y = Input.GetAxis("Vertical");
        //// Right
        //if(moveInput.x > 0)
        //{
        //    curRotateAngle += xOffset;
        //    if (curRotateAngle >= 390) curRotateAngle = 0;
        //}

        //if (moveInput.x < 0)
        //{
        //    curRotateAngle -= xOffset;
        //    if (curRotateAngle <= -390) curRotateAngle = 0;
        //}

        //if (moveInput.y > 0)
        //{
        //    curHeightAngle += yOffset;
        //    if (curHeightAngle >= 32) curHeightAngle = 32;
        //}

        //if (moveInput.y < 0)
        //{
        //    curHeightAngle -= yOffset;
        //    if (curHeightAngle <= 0) curHeightAngle = 0;
        //}

        //_360PTF.ElectricCylinderMsg.SendData(curRotateAngle, curHeightAngle, time);
    }

    public void Yaw(int rotateAngle)
    {
        _360PTF.ElectricCylinderMsg.SendData(0, rotateAngle, time);
        Debug.Log(rotateAngle);
    }

    public void Heaven(int heightAngle)
    {
        _360PTF.ElectricCylinderMsg.SendData(heightAngle, 0, time);
        Debug.Log(heightAngle);
    }

    public void PitchThree()
    {
        _360PTF.ElectricCylinderMsg.SendData(10, 30, time);
    }

    public void PitchHundre()
    {
        _360PTF.ElectricCylinderMsg.SendData(20, 390, time);
    }

}
public class Setting
{
    public int xOffset = 0, yOffset = 0, time = 42000;
}

