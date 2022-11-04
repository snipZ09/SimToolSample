using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;
using UnityEngine;

namespace TcpToServer
{
    // Token: 0x02000002 RID: 2
    public class TCPOp
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public static string OpenConnect()
        {
            string result = "successed";
            bool flag = !TCPOp.isConnect;
            if (flag)
            {
                IPAddress address = IPAddress.Parse(TCPOp.host);
                IPEndPoint remoteEP = new IPEndPoint(address, TCPOp.port);
                bool flag2 = TCPOp.clientSocket == null;
                if (flag2)
                {
                    TCPOp.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }
                try
                {
                    TCPOp.clientSocket.Connect(remoteEP);
                    TCPOp.isConnect = true;
                    TCPOp.receiveThread = new Thread(new ParameterizedThreadStart(TCPOp.ReceiveMessage));
                    TCPOp.receiveThread.Start(TCPOp.clientSocket);
                    TCPOp.timer = new System.Timers.Timer(10.0);
                    TCPOp.timer.Elapsed += TCPOp.TimerDataSend;
                    TCPOp.timer.AutoReset = true;
                    TCPOp.timer.Enabled = true;
                }
                catch (Exception ex)
                {
                    TCPOp.isConnect = false;
                    result = "failed:" + ex.Message;
                }
            }
            return result;
        }

        // Token: 0x06000002 RID: 2 RVA: 0x00002160 File Offset: 0x00000360
        private static void TimerDataSend(object sender, ElapsedEventArgs e)
        {
            TCPOp.SendMessageToServer(TCPProtocol.GetElectricCylinderPosition("AirCraft"));
        }

        // Token: 0x06000003 RID: 3 RVA: 0x00002174 File Offset: 0x00000374
        private static void ReceiveMessage(object client)
        {
            Socket socket = (Socket)client;
            for (; ; )
            {
                try
                {
                    int num = socket.Receive(TCPOp.receiveBuffer);
                    bool flag = num > 0;
                    if (flag)
                    {
                        string @string = Encoding.ASCII.GetString(TCPOp.receiveBuffer, 0, num);
                        string[] array = @string.Split(new char[]
                        {
                            '$'
                        });
                        for (int i = 0; i < array.Length; i++)
                        {
                            bool flag2 = array[i] != "";
                            if (flag2)
                            {
                                TCPOp.DataAnalysis(array[i]);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    TCPOp.isConnect = false;
                }
            }
        }

        // Token: 0x06000004 RID: 4 RVA: 0x0000222C File Offset: 0x0000042C
        private static void DataAnalysis(string str)
        {
            string[] array = str.Split(new char[]
            {
                '#'
            });
            bool flag = array[1].Equals("UDP");
            if (flag)
            {
                int num = Convert.ToInt32(array[2]);
                if (num == 3)
                {
                    TCPOp.currentPos = Convert.ToInt32(array[3]);
                    TCPOp.currentAngle = Convert.ToInt32(array[4]);
                }
            }
            else
            {
                bool flag2 = array[1].Equals("WeChat");
                if (flag2)
                {
                    TCPOp.PlayerID = array[2];
                }
            }
        }

        // Token: 0x06000005 RID: 5 RVA: 0x000022AC File Offset: 0x000004AC
        public static void CloseConnect()
        {
            bool flag = TCPOp.isConnect;
            if (flag)
            {
                try
                {
                    TCPOp.receiveThread.Abort();
                    TCPOp.clientSocket.Shutdown(SocketShutdown.Both);
                    TCPOp.clientSocket.Close();
                    TCPOp.clientSocket = null;
                    TCPOp.isConnect = false;
                }
                catch (Exception)
                {
                }
            }
        }

        // Token: 0x06000006 RID: 6 RVA: 0x0000230C File Offset: 0x0000050C
        private static bool SendMessageToServer(string str)
        {
            bool flag = TCPOp.isConnect;
            if (flag)
            {
                try
                {
                    MonoBehaviour.print("send str" + str);
                    Debug.Log("send str" + str);
                    byte[] bytes = Encoding.ASCII.GetBytes(str);
                    int num = TCPOp.clientSocket.Send(bytes);
                    return true;
                }
                catch (Exception ex)
                {
                    TCPOp.isConnect = false;
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

        // Token: 0x06000007 RID: 7 RVA: 0x00002368 File Offset: 0x00000568
        public static bool RightHandedRotation(int spreedAngle)
        {
            spreedAngle = Math.Abs(spreedAngle);
            return TCPOp.SendMessageToServer(TCPProtocol.BrushlessElectricMachine("AirCraft", ElectricPattern.Spread, spreedAngle, 0));
        }

        // Token: 0x06000008 RID: 8 RVA: 0x00002394 File Offset: 0x00000594
        public static bool LeftHandedRotation(int spreedAngle)
        {
            spreedAngle = Math.Abs(spreedAngle);
            Debug.Log("LeftHandedRotation:" + spreedAngle);
            return TCPOp.SendMessageToServer(TCPProtocol.BrushlessElectricMachine("AirCraft", ElectricPattern.Spread, spreedAngle * -1, 0));
        }

        // Token: 0x06000009 RID: 9 RVA: 0x000023C4 File Offset: 0x000005C4
        public static bool RightPositionRotation(int speed, int pos)
        {
            speed = Math.Abs(speed);
            return TCPOp.SendMessageToServer(TCPProtocol.BrushlessElectricMachine("AirCraft", ElectricPattern.Position, speed, pos));
        }

        // Token: 0x0600000A RID: 10 RVA: 0x000023F0 File Offset: 0x000005F0
        public static bool LeftPositionRotation(int speed, int pos)
        {
            speed = Math.Abs(speed);
            return TCPOp.SendMessageToServer(TCPProtocol.BrushlessElectricMachine("AirCraft", ElectricPattern.Position, speed * -1, pos));
        }

        // Token: 0x0600000B RID: 11 RVA: 0x00002420 File Offset: 0x00000620
        public static bool StopRotation()
        {
            return TCPOp.SendMessageToServer(TCPProtocol.BrushlessElectricMachine("AirCraft", ElectricPattern.Spread, 0, 0));
        }

        // Token: 0x0600000C RID: 12 RVA: 0x00002444 File Offset: 0x00000644
        public static bool Fan(int num)
        {
            return TCPOp.SendMessageToServer(TCPProtocol.SerialSpecialEffects("AirCraft", num));
        }

        // Token: 0x0600000D RID: 13 RVA: 0x00002468 File Offset: 0x00000668
        public static bool OpenMouseKey()
        {
            return TCPOp.SendMessageToServer(TCPProtocol.SerialMouseKeyboardEnable("AirCraft"));
        }

        // Token: 0x0600000E RID: 14 RVA: 0x0000248C File Offset: 0x0000068C
        public static bool CloseMouseKey()
        {
            return TCPOp.SendMessageToServer(TCPProtocol.SerialMouseKeyBoardDisable("AirCraft"));
        }

        // Token: 0x0600000F RID: 15 RVA: 0x000024B0 File Offset: 0x000006B0
        public static bool SerialRestart()
        {
            return TCPOp.SendMessageToServer(TCPProtocol.SerialRestart("AirCraft"));
        }

        // Token: 0x06000010 RID: 16 RVA: 0x000024D4 File Offset: 0x000006D4
        public static bool ElectricCylinderDistance(int time, int distance, bool isShake = false)
        {
            return TCPOp.SendMessageToServer(TCPProtocol.ElectricCylinderDistance("AirCraft", isShake, time, distance, 0, 0, 0, 0, 0));
        }

        // Token: 0x06000011 RID: 17 RVA: 0x00002500 File Offset: 0x00000700
        public static bool ElectricCylinderSin(int time, int distance, bool isShake = false)
        {
            return TCPOp.SendMessageToServer(TCPProtocol.ElectricCylinderSin("AirCraft", isShake, time, distance));
        }

        // Token: 0x06000012 RID: 18 RVA: 0x00002524 File Offset: 0x00000724
        public static bool WirelessSwimFunction(WirelessSwim fun)
        {
            return TCPOp.SendMessageToServer(TCPProtocol.WirelessSwimFunction("WirelessSwin", fun));
        }

        // Token: 0x06000013 RID: 19 RVA: 0x00002548 File Offset: 0x00000748
        public static bool GetPlayerID()
        {
            return TCPOp.SendMessageToServer(TCPProtocol.GetPlayerID("AirCraft"));
        }

        // Token: 0x04000001 RID: 1
        private static int port = 6000;

        // Token: 0x04000002 RID: 2
        private static string host = "127.0.0.1";

        // Token: 0x04000003 RID: 3
        public static bool isConnect = false;

        // Token: 0x04000004 RID: 4
        private static Socket clientSocket;

        // Token: 0x04000005 RID: 5
        private static Thread receiveThread;

        // Token: 0x04000006 RID: 6
        private static System.Timers.Timer timer;

        // Token: 0x04000007 RID: 7
        private static byte[] receiveBuffer = new byte[256];

        // Token: 0x04000008 RID: 8
        public static int currentPos = 0;

        // Token: 0x04000009 RID: 9
        public static int currentAngle = 0;

        // Token: 0x0400000A RID: 10
        public static string PlayerID = "0";
    }
}
