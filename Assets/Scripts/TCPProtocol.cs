using System;

namespace TcpToServer
{
	// Token: 0x02000008 RID: 8
	internal class TCPProtocol
	{
		// Token: 0x06000016 RID: 22 RVA: 0x000025C0 File Offset: 0x000007C0
		private TCPProtocol()
		{
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000025CC File Offset: 0x000007CC
		public static string ElectricCylinderDistance(string gameName, bool isShake = true, int time = 0, int distance1 = 0, int distance2 = 0, int distance3 = 0, int distance4 = 0, int distance5 = 0, int distance6 = 0)
		{
			return string.Format("{0}#UDP#{1}#{2}#{3}#{4}#{5}#{6}#{7}#{8}#{9}$", new object[]
			{
				gameName,
				1,
				time,
				distance1,
				distance2,
				distance3,
				distance4,
				distance5,
				distance6,
				isShake
			});
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002650 File Offset: 0x00000850
		public static string ElectricCylinderSin(string gameName, bool isShake = true, int time = 0, int distance1 = 0)
		{
			return string.Format("{0}#UDP#{1}#{2}#{3}#{4}$", new object[]
			{
				gameName,
				4,
				time,
				distance1,
				isShake
			});
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000026A0 File Offset: 0x000008A0
		public static string ElectricCylinderStop(string gameName)
		{
			return string.Format("{0}#UDP#{1}#Stop$", gameName, 2);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000026C8 File Offset: 0x000008C8
		public static string ElectricCylinderRestart(string gameName)
		{
			return string.Format("{0}#UDP#{1}#Restart$", gameName, 2);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000026F0 File Offset: 0x000008F0
		public static string ElectricCylinderInitializedPosition(string gameName)
		{
			return string.Format("{0}#UDP#{1}#{2}#{3}#{4}#{5}#{6}#{7}#{8}$", new object[]
			{
				gameName,
				1,
				100,
				0,
				0,
				0,
				0,
				0,
				0
			});
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002764 File Offset: 0x00000964
		public static string ElectricCylinderEscStop(string gameName)
		{
			return string.Format("{0}#UDP#{1}#EscStop$", gameName, 2);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000278C File Offset: 0x0000098C
		public static string GetElectricCylinderPosition(string gameName)
		{
			return string.Format("{0}#UDP#{1}#0$", gameName, 3);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000027B4 File Offset: 0x000009B4
		public static string BrushlessElectricMachine(string gameName, ElectricPattern ep, int speed, int pos = 0)
		{
			return string.Format("{0}#Serial#{1}#{2}#{3}#{4}$", new object[]
			{
				gameName,
				3,
				(int)ep,
				speed,
				pos
			});
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002804 File Offset: 0x00000A04
		public static string SerialSpecialEffects(string gameName, int data)
		{
			return string.Format("{0}#Serial#{1}#{2}$", gameName, 4, data);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002834 File Offset: 0x00000A34
		public static string SerialMouseKeyboardEnable(string gameName)
		{
			return string.Format("{0}#Serial#{1}#true$", gameName, 2);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000285C File Offset: 0x00000A5C
		public static string SerialMouseKeyBoardDisable(string gameName)
		{
			return string.Format("{0}#Serial#{1}#false$", gameName, 2);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002884 File Offset: 0x00000A84
		public static string SerialRestart(string gameName)
		{
			return string.Format("{0}#Serial#{1}$", gameName, 0);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000028AC File Offset: 0x00000AAC
		public static string WirelessSwimFunction(string gameName, WirelessSwim fun)
		{
			return string.Format("{0}#Wireless#{1}$", gameName, (int)fun);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000028D4 File Offset: 0x00000AD4
		public static string GetPlayerID(string gameName)
		{
			return string.Format("{0}#WeChat$", gameName);
		}
	}
}
