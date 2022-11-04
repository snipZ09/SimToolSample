using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;

namespace BackGround
{
	// Token: 0x02000012 RID: 18
	public class UDPCommunication
	{
		// Token: 0x06000083 RID: 131 RVA: 0x0000619C File Offset: 0x0000439C
		public UDPCommunication()
		{
			this.ipList = new List<string>();
			for (int i = 100; i <= 150; i++)
			{
				this.ipList.Add("192.168.15." + i.ToString());
			}
			foreach (IPAddress ipaddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
			{
				if (ipaddress.AddressFamily.ToString() == "InterNetwork")
				{
					if (ipaddress.ToString().Equals("192.168.15.101") || ipaddress.ToString().Equals("192.168.15.100"))
					{
						this.localIP = ipaddress.ToString();
					}
					foreach (string value in this.ipList)
					{
						if (ipaddress.ToString().Equals(value))
						{
							this.localIP = ipaddress.ToString();
							break;
						}
					}
				}
			}
			this.builder = new StringBuilder();
			this.localPort = 8410;
			this.remoteIP = "192.168.15.100";
			//Đọc ip từ XML
			//this.remoteIP = UdpIP;
			this.remotePort = 7408;
			this.UDPDataHead = "55 aa 00 00 14 01 00 00 ff ff ff ff 00 00 00 01";
			this.UDPDataTail = "12 34 56 78 ab cd";
			this.UDPDataBuilder = new StringBuilder();
			this.UDPSendData = new StringBuilder();
			this.maxDistance = 400000;
			UDPCommunication.readyPosition = 50;
		}

		public void StartThread()
		{
			UDPCommunication.socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			try
			{
				UDPCommunication.socketServer.Bind(new IPEndPoint(IPAddress.Parse(this.localIP), this.localPort));
			}
			catch (Exception ex)
			{
				WriteLog.writeLog("UDP:" + ex.Message + "  电动缸IP不对");
				Environment.Exit(0);
			}
			UDPCommunication.remotePoint = new IPEndPoint(IPAddress.Parse(this.remoteIP), this.remotePort);
			this.recvThread = new Thread(new ThreadStart(this.ReciveMsg));
			this.recvThread.Start();
			this.shakeThread = new Thread(new ThreadStart(this.ElectricalMachineShake));
			this.shakeThread.Start();
			this.timer = new System.Timers.Timer(10.0);
			this.timer.Elapsed += this.TimerDataSend;
			this.timer.AutoReset = true;
			this.timer.Enabled = true;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00006448 File Offset: 0x00004648
		private void TimerDataSend(object sender, ElapsedEventArgs e)
		{
			string str = "55 aa 00 00 11 01 00 00 ff ff ff ff 00 06 00 02 ";
			this.sendMsg(str);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00006464 File Offset: 0x00004664
		private void sendMsg(string str)
		{
			byte[] buffer = this.strToToHexByte(str);
			try
			{
				UDPCommunication.socketServer.SendTo(buffer, UDPCommunication.remotePoint);
				WriteLog.writeLog("udp sendmsg:" + buffer);
			}
			catch (Exception ex)
			{
				WriteLog.writeLog("udp sendmsg:" + ex.Message);
			}
		}

		private void ReciveMsg()
		{
			EndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
			for (; ; )
			{
				byte[] array = new byte[1024];
				int num = UDPCommunication.socketServer.ReceiveFrom(array, ref endPoint);
				this.builder.Clear();
				for (int i = 0; i < num; i++)
				{
					this.builder.Append(array[i].ToString("X2") + " ");
				}
				this.DataAnalysis(this.builder.ToString());
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00006538 File Offset: 0x00004738
		public void ElectricalMachineShake()
		{
			int millisecondsTimeout = 300;
			string empty = string.Empty;
			bool flag = false;
			for (; ; )
			{
				if (UDPCommunication.readyPosition >= 88)
				{
					this.shake = UDPCommunication.ShakeType.Upper;
					this.isShake = true;
				}
				else if (UDPCommunication.readyPosition <= 12)
				{
					this.shake = UDPCommunication.ShakeType.Lower;
					this.isShake = true;
				}
				else
				{
					this.shake = UDPCommunication.ShakeType.Normal;
					this.isShake = false;
				}
				if (this.isShake)
				{
					switch (this.shake)
					{
						case UDPCommunication.ShakeType.Upper:
							if (flag)
							{
								this.sendMsg("55 aa 00 00 14 01 00 00 ff ff ff ff 00 00 00 01 00 00 01 2C 00 06 1A 80 00 06 1A 80 00 06 1A 80 00 06 1A 80 00 06 1A 80 00 06 1A 80 12 34 56 78 ab cd");
								flag = false;
							}
							else
							{
								this.sendMsg("55 aa 00 00 14 01 00 00 ff ff ff ff 00 00 00 01 00 00 01 2C 00 05 7E 40 00 06 1A 80 00 06 1A 80 00 06 1A 80 00 06 1A 80 00 06 1A 80 12 34 56 78 ab cd");
								flag = true;
							}
							break;
						case UDPCommunication.ShakeType.Lower:
							if (flag)
							{
								this.sendMsg("55 aa 00 00 14 01 00 00 ff ff ff ff 00 00 00 01 00 00 01 2C 00 00 00 00 00 06 1A 80 00 06 1A 80 00 06 1A 80 00 06 1A 80 00 06 1A 80 12 34 56 78 ab cd");
								flag = false;
							}
							else
							{
								this.sendMsg("55 aa 00 00 14 01 00 00 ff ff ff ff 00 00 00 01 00 00 01 2C 00 00 9C 40 00 06 1A 80 00 06 1A 80 00 06 1A 80 00 06 1A 80 00 06 1A 80 12 34 56 78 ab cd");
								flag = true;
							}
							break;
					}
					Thread.Sleep(millisecondsTimeout);
				}
				else
				{
					Thread.Sleep(1);
				}
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000660C File Offset: 0x0000480C
		private byte[] strToToHexByte(string hexString)
		{
			hexString = hexString.Replace(" ", "");
			if (hexString.Length % 2 != 0)
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


		// Token: 0x0600008B RID: 139 RVA: 0x000066C8 File Offset: 0x000048C8
		public void SendData(string str, bool isRise = true, bool isPlayer = false)
		{
			this.UDPDataBuilder.Clear();
			this.UDPSendData.Clear();
			string[] array = str.Split(new char[]
			{
				'#'
			});
			if (array.Length > 4)
			{
				string text = Convert.ToInt32(array[3]).ToString("X8");
				this.UDPDataBuilder.Append(" " + text + " ");
			}
			for (int i = 4; i < array.Length - 1; i++)
			{
				int num = Convert.ToInt32(array[i]);
				if (num >= 100)
				{
					num = 100;
				}
				else if (num <= 0)
				{
					num = 0;
				}
				if (i == 4)
				{
					if (isPlayer)
					{
						UDPCommunication.readyPosition = 50;
					}
					else
					{
						UDPCommunication.readyPosition = num;
					}
					if ((num >= 90 && this.currentPos >= 90) || (num <= 10 && this.currentPos <= 10))
					{
						return;
					}
				}
				string text = (this.maxDistance / 100 * num).ToString("X8");
				this.UDPDataBuilder.Append(text + " ");
			}
			this.UDPSendData.Append(this.UDPDataHead);
			this.UDPSendData.Append(this.UDPDataBuilder);
			this.UDPSendData.Append(this.UDPDataTail);
			this.sendMsg(this.UDPSendData.ToString());
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00006814 File Offset: 0x00004A14
		public void SendSinData(int time, int distance)
		{
			if (distance >= 200)
			{
				distance = 200;
			}
			if (distance <= 0)
			{
				distance = 0;
			}
			double num = (Math.Sin(0.031415926535897934 * (double)distance - 1.5707963267948966) + 1.0) / 2.0;
			if (num >= 1.0)
			{
				num = 1.0;
			}
			if (num <= 0.0)
			{
				num = 0.0;
			}
			int num2 = (int)Math.Floor(num * (double)this.maxDistance);
			this.UDPSendData.Clear();
			string value = time.ToString("X8");
			string value2 = num2.ToString("X8");
			this.UDPSendData.Append(this.UDPDataHead);
			this.UDPSendData.Append(value);
			for (int i = 0; i < 6; i++)
			{
				this.UDPSendData.Append(value2);
			}
			this.UDPSendData.Append(this.UDPDataTail);
			try
			{
				this.sendMsg(this.UDPSendData.ToString());
			}
			catch (Exception)
			{
				WriteLog.writeLog(this.UDPSendData.ToString());
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00006950 File Offset: 0x00004B50
		public void Restart()
		{
			string str = "55 aa 00 00 12 01 00 02 ff ff ff ff 00 00 00 01 00 00";
			this.sendMsg(str);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000696C File Offset: 0x00004B6C
		public void EmergencyStop()
		{
			string str = "55 aa 00 00 12 01 00 00 ff ff ff ff 00 90 00 01 00 01";
			this.sendMsg(str);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00006988 File Offset: 0x00004B88
		public void EmergencyEscStop()
		{
			string str = "55 aa 00 00 12 01 00 00 ff ff ff ff 00 90 00 01 00 00";
			this.sendMsg(str);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000069A4 File Offset: 0x00004BA4
		public void GetPosition(int num)
		{
			string str = null;
			switch (num)
			{
				case 0:
					str = "55 aa 00 00 11 01 00 00 ff ff ff ff 00 06 00 02 ";
					break;
				case 1:
					str = "55 aa 00 00 11 01 00 00 ff ff ff ff 00 08 00 02 ";
					break;
				case 2:
					str = "55 aa 00 00 11 01 00 00 ff ff ff ff 00 0a 00 02 ";
					break;
			}
			this.sendMsg(str);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000069E4 File Offset: 0x00004BE4
		private void DataAnalysis(string str)
		{
			if (str[13] == '1' && str[16] == '2')
			{
				string value = string.Concat(new string[]
				{
					str[54].ToString(),
					str[55].ToString(),
					str[57].ToString(),
					str[58].ToString(),
					str[48].ToString(),
					str[49].ToString(),
					str[51].ToString(),
					str[52].ToString()
				});
				this.currentPos = Convert.ToInt32(value, 16);
				if (this.currentPos >= this.maxDistance)
				{
					this.currentPos = this.maxDistance;
				}
				else if (this.currentPos <= 0)
				{
					this.currentPos = 0;
				}
				this.currentPos = this.currentPos * 100 / this.maxDistance;
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00006B04 File Offset: 0x00004D04
		public void ZeroPosition()
		{
			string str = "55 aa 00 00 14 01 ff ff ff ff 00 00 00 01 00 00 0f c4 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 05 12 34 56 78 ab cd";
			this.sendMsg(str);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00006B1E File Offset: 0x00004D1E
		public void DisanbleShake()
		{
			UDPCommunication.readyPosition = 50;
		}

		// Token: 0x04000080 RID: 128
		private static Socket socketServer;

		// Token: 0x04000081 RID: 129
		private StringBuilder builder;

		// Token: 0x04000082 RID: 130
		private string localIP;

		// Token: 0x04000083 RID: 131
		private int localPort;

		// Token: 0x04000084 RID: 132
		private string remoteIP;

		// Token: 0x04000085 RID: 133
		private int remotePort;

		// Token: 0x04000086 RID: 134
		private static EndPoint remotePoint;

		// Token: 0x04000087 RID: 135
		private Thread recvThread;

		// Token: 0x04000088 RID: 136
		private Thread shakeThread;

		// Token: 0x04000089 RID: 137
		private string UDPDataHead;

		// Token: 0x0400008A RID: 138
		private string UDPDataTail;

		// Token: 0x0400008B RID: 139
		private StringBuilder UDPDataBuilder;

		// Token: 0x0400008C RID: 140
		private StringBuilder UDPSendData;

		// Token: 0x0400008D RID: 141
		private int maxDistance;

		// Token: 0x0400008E RID: 142
		public int currentPos;

		// Token: 0x0400008F RID: 143
		private System.Timers.Timer timer;

		// Token: 0x04000090 RID: 144
		public bool isShake;

		// Token: 0x04000091 RID: 145
		private UDPCommunication.ShakeType shake;

		// Token: 0x04000092 RID: 146
		private static int readyPosition;

		// Token: 0x04000093 RID: 147
		private List<string> ipList;

		// Token: 0x0200001A RID: 26
		public enum ShakeType
		{
			// Token: 0x040000A8 RID: 168
			Normal,
			// Token: 0x040000A9 RID: 169
			Upper,
			// Token: 0x040000AA RID: 170
			Lower
		}

		internal class WriteLog
		{
			// Token: 0x060000A3 RID: 163 RVA: 0x00006B28 File Offset: 0x00004D28
			public static void writeLog(string msg)
			{
				try
				{
					StreamWriter streamWriter = File.AppendText("D:/日志.txt");
					streamWriter.WriteLine(DateTime.Now.ToString() + ":   " + msg);
					streamWriter.Close();
					streamWriter.Dispose();
				}
				catch (Exception)
				{
					FileStream fileStream = new FileStream("D:/日志.txt", FileMode.Create);
					fileStream.Close();
					fileStream.Dispose();
				}
			}
		}
	}
}
