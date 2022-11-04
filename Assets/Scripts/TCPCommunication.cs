using BackGround;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;
using UnityEngine;
using static BackGround.UDPCommunication;

namespace BackGround
{
    public class TCPCommunication
{
	// Token: 0x06000071 RID: 113 RVA: 0x00005A4D File Offset: 0x00003C4D
	public TCPCommunication(int port)
	{
		this.ip = "127.0.0.1";
		this.port = port;
		this.receiveBuffer = new byte[256];
	}

	// Token: 0x06000072 RID: 114 RVA: 0x00005A77 File Offset: 0x00003C77
	public void StartThread()
	{
		this.startSderverThread = new Thread(new ThreadStart(this.StartServer));
		this.startSderverThread.Start();
	}

	// Token: 0x06000073 RID: 115 RVA: 0x00005A9C File Offset: 0x00003C9C
	private void StartServer()
	{
		this.serverSocket = null;
		this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		this.serverSocket.Bind(new IPEndPoint(IPAddress.Parse(this.ip), this.port));
		this.serverSocket.Listen(10);
		for (; ; )
		{
			try
			{
				this.clientSocket = this.serverSocket.Accept();
			}
			catch (Exception ex)
			{
				WriteLog.writeLog(ex.Message);
			}
			this.receiveThread = new Thread(new ParameterizedThreadStart(this.ReceiveMessage));
			this.receiveThread.Start(this.clientSocket);
		}
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00005B48 File Offset: 0x00003D48
	private void TimerDataSend(object sender, ElapsedEventArgs e)
	{
		this.SendUdpCurrentPosAndCurrentAngle();
	}

	// Token: 0x06000075 RID: 117 RVA: 0x00005B50 File Offset: 0x00003D50
	private void ReceiveMessage(object client)
	{
		Socket socket = (Socket)client;
		for (; ; )
		{
			try
			{
				int num = socket.Receive(this.receiveBuffer);
				if (num > 0)
				{
					string[] array = Encoding.ASCII.GetString(this.receiveBuffer, 0, num).Split(new char[]
					{
							'$'
					});
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] != "")
						{
							this.DataAnalysis(array[i]);
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}

	// Token: 0x06000076 RID: 118 RVA: 0x00005BD4 File Offset: 0x00003DD4
	public void SendMesage(string str)
	{
		try
		{
			byte[] bytes = Encoding.UTF8.GetBytes(str);
			this.clientSocket.Send(bytes);
		}
		catch (Exception)
		{
			this.CloseClient();
		}
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00005C18 File Offset: 0x00003E18
	public void CloseClient()
	{
		if (this.clientSocket != null)
		{
			this.clientSocket.Shutdown(SocketShutdown.Both);
			this.clientSocket.Close();
			this.clientSocket = null;
		}
		if (this.receiveThread != null)
		{
			try
			{
				this.receiveThread.Abort();
			}
			catch (Exception)
			{
			}
		}
	}

	// Token: 0x06000078 RID: 120 RVA: 0x00005C74 File Offset: 0x00003E74
	public void CloseThread()
	{
		if (this.clientSocket != null)
		{
			this.clientSocket.Shutdown(SocketShutdown.Both);
			this.clientSocket.Close();
			this.clientSocket = null;
		}
		if (this.serverSocket != null)
		{
			this.serverSocket.Close();
			this.serverSocket.Dispose();
			this.serverSocket = null;
		}
		if (this.receiveThread != null)
		{
			try
			{
				this.receiveThread.Abort();
			}
			catch (Exception)
			{
			}
		}
	}

	[SerializeField] UDPCommunication udp;
    [SerializeField] SerialComunication serial;

    // Token: 0x06000079 RID: 121 RVA: 0x00005CF4 File Offset: 0x00003EF4
    private void DataAnalysis(string str)
    {
        string[] array = str.Split(new char[]
        {
                '#'
        });
        if (array[1].Equals("UDP"))
        {
            switch (Convert.ToInt32(array[2]))
            {
                case 0:
                    if (array[10] != null)
                    {
                        if (Convert.ToBoolean(array[10]))
                        {
                            udp.SendData(str, true, false);
                        }
                        else
                        {
                            udp.SendData(str, true, true);
                        }
                    }
                    else
                    {
                        udp.SendData(str, true, false);
                    }
                    break;
                case 1:
                    if (array[10] != null)
                    {
                        if (Convert.ToBoolean(array[10]))
                        {
                            udp.SendData(str, false, false);
                        }
                        else
                        {
                            udp.SendData(str, false, true);
                        }
                    }
                    else
                    {
                        udp.SendData(str, false, false);
                    }
                    break;
                case 2:
                    if (array[3].Equals("Restart"))
                    {
                        udp.Restart();
                    }
                    else if (array[3].Equals("Stop"))
                    {
                        udp.EmergencyStop();
                    }
                    else if (array[3].Equals("EscStop"))
                    {
                        udp.EmergencyEscStop();
                    }
                    break;
                case 3:
                    this.SendUdpCurrentPosAndCurrentAngle();
                    break;
                case 4:
                    udp.SendSinData(Convert.ToInt32(array[3]), Convert.ToInt32(array[4]));
                    break;
            }
        }
        if (array[1].Equals("Serial"))
        {
            switch (Convert.ToInt32(array[2]))
            {
                case 0:
                    serial.RestartPosition();
                    break;
                case 1:
                    if (array[3].Equals("true"))
                    {
                        serial.SwipeCard(true);
                    }
                    else
                    {
                        serial.SwipeCard(false);
                    }
                    break;
                case 2:
                    if (array[3].Equals("true"))
                    {
                        serial.KeybordMouse(true);
                    }
                    else
                    {
                        serial.KeybordMouse(false);
                    }
                    break;
                case 3:
                    if (Convert.ToInt32(array[3]) == 0)
                    {
                        serial.ElectricalMachine(true, array[4], "0");
                    }
                    else
                    {
                        serial.ElectricalMachine(false, array[4], array[5]);
                    }
                    break;
                case 4:
                    serial.SpecialEffects(array[3]);
                    break;
            }
        }
        if (array[1].Equals("Monitor"))
        {
            switch (Convert.ToInt32(array[2]))
            {
                case 0:
                    this.SendGameContexMessage();
                    break;
                case 1:
                    {
                        int num = 0;
                        try
                        {
                            num = Convert.ToInt32(array[6]);
                        }
                        catch (Exception)
                        {
                            num = 0;
                        }
                        if (num == 0)
                        {
                            //monitoring.StartNewGame(array[3], array[4], array[5], 600);
                        }
                        else
                        {
                            //monitoring.StartNewGame(array[3], array[4], array[5], num);
                        }
                        break;
                    }
                case 2:
                    this.SendPunchCardMessage();
                    break;
                case 3:
                    this.SendPunchCardLogMessage();
                    break;
                case 4:
                    this.SendCurrentDayGameLog(Convert.ToInt32(array[3]));
                    break;
                case 5:
                    this.SendGamePSW();
                    break;
                case 6:
                    if (array[3].Equals("true"))
                    {
                        //sql.UpdatePSW(true, array[4]);
                    }
                    else
                    {
                        //sql.UpdatePSW(false, array[5]);
                    }
                    break;
                case 7:
                    //sql.ClearAccount();
                    break;
                case 11:
                    //playerID = array[3];
                    break;
                case 12:
                    this.SendDogID();
                    break;
            }
        }
        if (array[1].Equals("WeChat"))
        {
            this.SendPlayerID();
        }
    }

    // Token: 0x0600007A RID: 122 RVA: 0x00006070 File Offset: 0x00004270
    public void SendPunchCardMessage()
    {
        //this.SendMesage(string.Format("BackGround#TCP#{0}#{1}$", 2, punchCardNum));
    }

    // Token: 0x0600007B RID: 123 RVA: 0x00006092 File Offset: 0x00004292
    public void SendPunchCardLogMessage()
    {
        //this.SendMesage(string.Format("BackGround#TCP#{0}#{1}$", 3, sql.ReadPunchCardLog()));
    }

    // Token: 0x0600007C RID: 124 RVA: 0x000060B4 File Offset: 0x000042B4
    public void SendCurrentDayGameLog(int page)
    {
        //this.SendMesage(string.Format("BackGround#TCP#{0}#{1}$", 4, sql.ReadCurrentDayGameLog(page)));
    }

    // Token: 0x0600007D RID: 125 RVA: 0x000060D7 File Offset: 0x000042D7
    public void SendGameContexMessage()
    {
        //this.SendMesage(string.Format("BackGround#TCP#{0}#{1}$", 0, sql.ReadGameContex()));
    }

    // Token: 0x0600007E RID: 126 RVA: 0x000060F9 File Offset: 0x000042F9
    public void SendGamePSW()
    {
        //this.SendMesage(string.Format("BackGround#TCP#{0}#{1}$", 5, sql.ReadPSW()));
    }

    // Token: 0x0600007F RID: 127 RVA: 0x0000611B File Offset: 0x0000431B
    public void SendShootMsg()
    {
        this.SendMesage(string.Format("BackGround#TCP#{0}$", 10));
    }

    // Token: 0x06000080 RID: 128 RVA: 0x00006134 File Offset: 0x00004334
    public void SendDogID()
    {
        //this.SendMesage(string.Format("BackGround#TCP#{0}#{1}$", 12, DogID));
    }

    // Token: 0x06000081 RID: 129 RVA: 0x00006152 File Offset: 0x00004352
    public void SendUdpCurrentPosAndCurrentAngle()
    {
        this.SendMesage(string.Format("BackGround#UDP#{0}#{1}#{2}$", 3, udp.currentPos, SerialComunication.currentAngle));
    }

    // Token: 0x06000082 RID: 130 RVA: 0x00006183 File Offset: 0x00004383
    public void SendPlayerID()
    {
        //this.SendMesage(string.Format("BackGround#WeChat#{0}$", playerID));
    }

    // Token: 0x04000079 RID: 121
    private string ip;

    // Token: 0x0400007A RID: 122
    private int port;

    // Token: 0x0400007B RID: 123
    private Socket serverSocket;

    // Token: 0x0400007C RID: 124
    private Socket clientSocket;

    // Token: 0x0400007D RID: 125
    private Thread startSderverThread;

    // Token: 0x0400007E RID: 126
    private Thread receiveThread;

    // Token: 0x0400007F RID: 127
    private byte[] receiveBuffer;
}

}
