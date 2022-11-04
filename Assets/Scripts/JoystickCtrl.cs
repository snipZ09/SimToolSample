using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

// Token: 0x02000083 RID: 131
public class JoystickCtrl : MonoBehaviour
{
	// Token: 0x0600024D RID: 589 RVA: 0x000148AA File Offset: 0x00012CAA
	private void Awake()
	{
		JoystickCtrl.Instance = this;
	}

	// Token: 0x0600024E RID: 590 RVA: 0x000148B4 File Offset: 0x00012CB4
	private void Start()
	{
		this.OutcurrentButton = "0";
		this.OutcurrentAxis = "0";
		this.OutcurrentAxisFR = "0";
		this.currentButton = "0";
		this.currentAxis = "0";
		this.currentAxisFR = "0";
		this.axisInput = 0f;
		this.LFrist = true;
		this.RFrist = true;
		this.FFrist = true;
		this.BFrist = true;
		this.listString = new List<string>();
		this.GetExternalFile(this.JoyName);
		this.ReadInformation();
	}

	// Token: 0x0600024F RID: 591 RVA: 0x00014954 File Offset: 0x00012D54
	private void ReadInformation()
	{
		for (int i = 0; i < this.listString.Count; i++)
		{
			string text = this.listString[i];
			int num = text.IndexOf("：");
			string text2 = text.Substring(0, num);
			switch (text2)
			{
				case "button1":
					this.button1 = text.Substring(num + 1).Trim();
					break;
				case "button2":
					this.button2 = text.Substring(num + 1).Trim();
					break;
				case "button3":
					this.button3 = text.Substring(num + 1).Trim();
					break;
				case "button4":
					this.button4 = text.Substring(num + 1).Trim();
					break;
				case "button5":
					this.button5 = text.Substring(num + 1).Trim();
					break;
				case "button6":
					this.button6 = text.Substring(num + 1).Trim();
					break;
				case "button7":
					this.button7 = text.Substring(num + 1).Trim();
					break;
				case "button8":
					this.button8 = text.Substring(num + 1).Trim();
					break;
				case "button9":
					this.button9 = text.Substring(num + 1).Trim();
					break;
				case "button10":
					this.button10 = text.Substring(num + 1).Trim();
					break;
				case "button11":
					this.button11 = text.Substring(num + 1).Trim();
					break;
				case "button12":
					this.button12 = text.Substring(num + 1).Trim();
					break;
				case "Left":
					this.Left = text.Substring(num + 1).Trim();
					break;
				case "Right":
					this.Right = text.Substring(num + 1).Trim();
					break;
				case "Forward":
					this.Forward = text.Substring(num + 1).Trim();
					break;
				case "Rear":
					this.Rear = text.Substring(num + 1).Trim();
					break;
				case "LROffset":
					this.LROffset = float.Parse(text.Substring(num + 1).Trim());
					break;
				case "FROffset":
					this.FROffset = float.Parse(text.Substring(num + 1).Trim());
					break;
			}
		}
	}

	// Token: 0x06000250 RID: 592 RVA: 0x00014CE8 File Offset: 0x000130E8
	private void GetButton()
	{
		if (Input.GetButton("joystick button 0"))
		{
			this.currentButton = this.button1;
		}
		if (Input.GetButton("joystick button 1"))
		{
			this.currentButton = this.button2;
		}
		if (Input.GetButton("joystick button 2"))
		{
			this.currentButton = this.button3;
		}
		if (Input.GetButton("joystick button 3"))
		{
			this.currentButton = this.button4;
		}
		if (Input.GetButton("joystick button 4"))
		{
			this.currentButton = this.button5;
		}
		if (Input.GetButton("joystick button 5"))
		{
			this.currentButton = this.button6;
		}
		if (Input.GetButton("joystick button 6"))
		{
			this.currentButton = this.button7;
		}
		if (Input.GetButton("joystick button 7"))
		{
			this.currentButton = this.button8;
		}
		if (Input.GetButton("joystick button 8"))
		{
			this.currentButton = this.button9;
		}
		if (Input.GetButton("joystick button 9"))
		{
			this.currentButton = this.button10;
		}
		if (Input.GetButton("joystick button 10"))
		{
			this.currentButton = this.button11;
		}
		if (Input.GetButton("joystick button 11"))
		{
			this.currentButton = this.button12;
		}
	}

	// Token: 0x06000251 RID: 593 RVA: 0x00014E3A File Offset: 0x0001323A
	private void Update()
	{

			this.IsJoystick = true;
			this.GetAxis();
			this.GetcurrentButton();
			this.BtnState();
		
	}

	// Token: 0x06000252 RID: 594 RVA: 0x00014E64 File Offset: 0x00013264
	private void GetAxis()
	{
		if (Input.GetAxisRaw("X axis") > this.LROffset && this.LFrist)
		{
			this.currentAxis = this.Left;
			this.axisInput = Input.GetAxisRaw("X axis");
			this.m_IsLeft = true;
		}
		else
		{
			this.m_IsLeft = false;
		}
		if (Input.GetAxisRaw("X axis") < -this.LROffset && this.RFrist)
		{
			this.currentAxis = this.Right;
			this.axisInput = Input.GetAxisRaw("X axis");
			this.m_IsRight = true;
		}
		else
		{
			this.m_IsRight = false;
		}
		if (Input.GetAxisRaw("Y axis") > this.FROffset)
		{
			this.currentAxisFR = this.Forward;
			this.axisInput = Input.GetAxisRaw("Y axis");
			this.FFrist = false;
			this.m_IsForward = true;
		}
		else
		{
			this.m_IsForward = false;
		}
		if (Input.GetAxisRaw("Y axis") < -this.FROffset)
		{
			this.currentAxisFR = this.Rear;
			this.axisInput = Input.GetAxisRaw("Y axis");
			this.m_IsRear = true;
		}
		else
		{
			this.m_IsRear = false;
		}
		this.f_5th_axis = Input.GetAxisRaw("5th axis");
		if ((double)Input.GetAxisRaw("5th axis") > 0.3)
		{
			this.currentAxis = "5th axis";
			this.twist = JoystickCtrl.Twist.Left;
		}
		else if ((double)Input.GetAxisRaw("5th axis") < -0.3)
		{
			this.twist = JoystickCtrl.Twist.Right;
		}
		else
		{
			this.twist = JoystickCtrl.Twist.Nome;
		}
		if ((double)Input.GetAxisRaw("6th axis") > 0.3 || (double)Input.GetAxisRaw("6th axis") < -0.3)
		{
			this.currentAxis = "6th axis";
			this.axisInput = Input.GetAxisRaw("6th axis");
			this.f_6th_axis = Input.GetAxisRaw("6th axis");
			if ((double)this.f_6th_axis > 0.3)
			{
				Debug.Log("摇杆圆盘左");
			}
			if ((double)this.f_6th_axis < 0.3)
			{
				Debug.Log("摇杆圆盘右");
			}
		}
		if ((double)Input.GetAxisRaw("7th axis") > 0.3 || (double)Input.GetAxisRaw("7th axis") < -0.3)
		{
			this.currentAxis = "7th axis";
			this.f_7th_axis = Input.GetAxisRaw("7th axis");
			Debug.Log("摇杆最上的圆盘上下" + this.f_7th_axis);
		}
		if ((double)Input.GetAxisRaw("8th axis") > 0.3 || (double)Input.GetAxisRaw("8th axis") < -0.3)
		{
			this.currentAxis = "8th axis";
			this.f_8th_axis = Input.GetAxisRaw("8th axis");
			Debug.Log("8th axis" + this.currentAxis);
		}
	}

	// Token: 0x06000253 RID: 595 RVA: 0x0001518C File Offset: 0x0001358C
	private void BtnState()
	{
		this.m_Btn1 = Input.GetButton("joystick button 0");
		this.m_Btn2 = Input.GetButton("joystick button 1");
		this.m_Btn3 = Input.GetButton("joystick button 2");
		this.m_Btn4 = Input.GetButton("joystick button 3");
		this.m_Btn5 = Input.GetButton("joystick button 4");
		this.m_Btn6 = Input.GetButton("joystick button 5");
	}

	// Token: 0x06000254 RID: 596 RVA: 0x000151FC File Offset: 0x000135FC
	private void GetcurrentButton()
	{
		if (this.currentButton != "0" && this.currentButton != "Disabled")
		{
			this.OutcurrentButton = this.currentButton;
		}
		else
		{
			this.OutcurrentButton = "0";
		}
		if (this.currentAxis != "0" && this.currentAxis != "Disabled")
		{
			this.OutcurrentAxis = this.currentAxis;
		}
		else
		{
			this.OutcurrentAxis = "0";
		}
		if (this.currentAxisFR != "0" && this.currentAxisFR != "Disabled")
		{
			this.OutcurrentAxisFR = this.currentAxisFR;
		}
		else
		{
			this.OutcurrentAxisFR = "0";
		}
		if (this.currentButton == this.button1 && this.button1 != "Disabled")
		{
			this.currentButton = "0";
		}
		if (this.currentButton == this.button2 && this.button2 != "Disabled")
		{
			this.currentButton = "0";
		}
		if (this.currentButton == this.button3 && this.button3 != "Disabled")
		{
			this.currentButton = "0";
		}
		if (this.currentButton == this.button4 && this.button4 != "Disabled")
		{
			this.currentButton = "0";
		}
		if (this.currentButton == this.button5 && this.button5 != "Disabled")
		{
			this.currentButton = "0";
		}
		if (this.currentButton == this.button6 && this.button6 != "Disabled")
		{
			this.currentButton = "0";
		}
		if (this.currentButton == this.button7 && this.button7 != "Disabled")
		{
			this.currentButton = "0";
		}
		if (this.currentButton == this.button8 && this.button8 != "Disabled")
		{
			this.currentButton = "0";
		}
		if (this.currentButton == this.button9 && this.button9 != "Disabled")
		{
			this.currentButton = "0";
		}
		if (this.currentButton == this.button10 && this.button10 != "Disabled")
		{
			this.currentButton = "0";
		}
		if (this.currentButton == this.button11 && this.button11 != "Disabled")
		{
			this.currentButton = "0";
		}
		if (this.currentButton == this.button12 && this.button12 != "Disabled")
		{
			this.currentButton = "0";
		}
		if (this.currentAxisFR == this.Forward)
		{
			this.currentAxisFR = "0";
		}
		if (this.currentAxisFR == this.Rear)
		{
			this.currentAxisFR = "0";
		}
		if (this.currentAxis == this.Left)
		{
			this.currentAxis = "0";
		}
		if (this.currentAxis == this.Right)
		{
			this.currentAxis = "0";
		}
	}

	// Token: 0x06000255 RID: 597 RVA: 0x0001562C File Offset: 0x00013A2C
	

	// Token: 0x06000256 RID: 598 RVA: 0x00015778 File Offset: 0x00013B78
	private void GetExternalFile(string joyName)
	{
		string joyToKeyTxt = this.GetJoyToKeyTxt(joyName);
		if (File.Exists(joyToKeyTxt))
		{
			Debug.Log("Doc dc");
			FileStream fileStream = new FileStream(joyToKeyTxt, FileMode.Open);
			StreamReader streamReader = new StreamReader(fileStream);
			for (string text = streamReader.ReadLine(); text != null; text = streamReader.ReadLine())
			{
				this.UTF8bytes = new byte[text.Length];
				this.UTF8bytes = Encoding.UTF8.GetBytes(text);
				this.listString.Add(Encoding.UTF8.GetString(this.UTF8bytes));
			}
			fileStream.Close();
		}
	}

	// Token: 0x06000257 RID: 599 RVA: 0x00015808 File Offset: 0x00013C08
	private string GetJoyToKeyTxt(string joyName)
	{
		return Application.streamingAssetsPath + "/" + joyName + ".txt";
	}

	// Token: 0x06000258 RID: 600 RVA: 0x0001583C File Offset: 0x00013C3C
	public string Getpath()
	{
		string dataPath = Application.dataPath;
		int length = dataPath.LastIndexOf("/");
		return dataPath.Substring(0, length);
	}

	// Token: 0x04000378 RID: 888
	public static JoystickCtrl Instance;

	// Token: 0x04000379 RID: 889
	private string currentButton;

	// Token: 0x0400037A RID: 890
	private string currentAxis;

	// Token: 0x0400037B RID: 891
	private string currentAxisFR;

	// Token: 0x0400037C RID: 892
	private float axisInput;

	// Token: 0x0400037D RID: 893
	[HideInInspector]
	public string Left;

	// Token: 0x0400037E RID: 894
	[HideInInspector]
	public string Right;

	// Token: 0x0400037F RID: 895
	[HideInInspector]
	public string Forward;

	// Token: 0x04000380 RID: 896
	[HideInInspector]
	public string Rear;

	// Token: 0x04000381 RID: 897
	private float LROffset;

	// Token: 0x04000382 RID: 898
	private float FROffset;

	// Token: 0x04000383 RID: 899
	[HideInInspector]
	public string button1;

	// Token: 0x04000384 RID: 900
	private string button2;

	// Token: 0x04000385 RID: 901
	private string button3;

	// Token: 0x04000386 RID: 902
	private string button4;

	// Token: 0x04000387 RID: 903
	private string button5;

	// Token: 0x04000388 RID: 904
	private string button6;

	// Token: 0x04000389 RID: 905
	private string button7;

	// Token: 0x0400038A RID: 906
	private string button8;

	// Token: 0x0400038B RID: 907
	private string button9;

	// Token: 0x0400038C RID: 908
	private string button10;

	// Token: 0x0400038D RID: 909
	private string button11;

	// Token: 0x0400038E RID: 910
	private string button12;

	// Token: 0x0400038F RID: 911
	[HideInInspector]
	public string OutcurrentButton;

	// Token: 0x04000390 RID: 912
	[HideInInspector]
	public string OutcurrentAxis;

	// Token: 0x04000391 RID: 913
	[HideInInspector]
	public string OutcurrentAxisFR;

	// Token: 0x04000392 RID: 914
	private bool LFrist;

	// Token: 0x04000393 RID: 915
	private bool RFrist;

	// Token: 0x04000394 RID: 916
	private bool FFrist;

	// Token: 0x04000395 RID: 917
	private bool BFrist;

	// Token: 0x04000396 RID: 918
	private float f_5th_axis = 0f;

	// Token: 0x04000397 RID: 919
	public JoystickCtrl.Twist twist = JoystickCtrl.Twist.Nome;

	// Token: 0x04000398 RID: 920
	private float f_6th_axis = 0f;

	// Token: 0x04000399 RID: 921
	private float f_7th_axis = 0f;

	// Token: 0x0400039A RID: 922
	private float f_8th_axis = 0f;

	// Token: 0x0400039B RID: 923
	public bool m_IsForward;

	// Token: 0x0400039C RID: 924
	public bool m_IsRear;

	// Token: 0x0400039D RID: 925
	public bool m_IsLeft;

	// Token: 0x0400039E RID: 926
	public bool m_IsRight;

	// Token: 0x0400039F RID: 927
	public bool m_Btn1 = false;

	// Token: 0x040003A0 RID: 928
	public bool m_Btn2 = false;

	// Token: 0x040003A1 RID: 929
	public bool m_Btn3 = false;

	// Token: 0x040003A2 RID: 930
	private bool m_Btn6 = false;

	// Token: 0x040003A3 RID: 931
	private bool m_Btn4 = false;

	// Token: 0x040003A4 RID: 932
	private bool m_Btn5 = false;

	// Token: 0x040003A5 RID: 933
	private bool IsJoystick = false;

	// Token: 0x040003A6 RID: 934
	private byte[] UTF8bytes;

	// Token: 0x040003A7 RID: 935
	private List<string> listString;

	// Token: 0x040003A8 RID: 936
	private string JoyName = "JoyToKey";

	// Token: 0x02000084 RID: 132
	public enum Twist
	{
		// Token: 0x040003AB RID: 939
		Nome,
		// Token: 0x040003AC RID: 940
		Left,
		// Token: 0x040003AD RID: 941
		Right
	}
}
