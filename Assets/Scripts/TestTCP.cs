using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using TcpToServer;
using UnityEngine;
public class TestTCP : MonoBehaviour
{
	public static TestTCP m_Instance;

	// Token: 0x040004AD RID: 1197
	private bool TurnLfet = true;

	// Token: 0x040004AE RID: 1198
	private bool TurnRight = true;

	// Token: 0x040004AF RID: 1199
	private bool bool1;

	// Token: 0x040004B0 RID: 1200
	private float TImers;

	// Token: 0x040004B1 RID: 1201
	//public GameObject m_Camera_eye;

	// Token: 0x040004B2 RID: 1202
	//public float QJn_Speed = 2f;

	// Token: 0x040004B3 RID: 1203
	private float currentTime = 0f;

	// Token: 0x040004B4 RID: 1204
	private float totalTime;

	// Token: 0x040004B5 RID: 1205
	private bool b_Rise = true;

	// Token: 0x040004B6 RID: 1206
	private bool b_Decline = true;

	// Token: 0x040004B7 RID: 1207
	private bool IsUpFen = false;

	// Token: 0x040004B8 RID: 1208
	private bool IsDownFen = false;

	// Token: 0x040004B9 RID: 1209
	private bool IsNomeFen = false;

	// Token: 0x040004BA RID: 1210
	private Vector3 temp;

	// Token: 0x040004BB RID: 1211
	private float tempUpSpeed;

	// Token: 0x040004BC RID: 1212
	private float f1;

	// Token: 0x040004BD RID: 1213
	private float f2;

	// Token: 0x040004BE RID: 1214
	private bool fristUp = true;

	// Token: 0x040004BF RID: 1215
	private bool fristDown = true;

	// Token: 0x040004C0 RID: 1216
	private int pos = 0;

	// Token: 0x040004C1 RID: 1217
	private int distance = 0;

	// Token: 0x040004C2 RID: 1218
	private bool IsOpenDouDongUp = false;

	// Token: 0x040004C3 RID: 1219
	private bool IsOpenDouDongDown = false;

	// Token: 0x040004C4 RID: 1220
	private bool jiajian;

	// Token: 0x040004C5 RID: 1221
	private float SpaceTime;

	// Token: 0x040004C6 RID: 1222
	private int DouDongDicUp = 85;

	// Token: 0x040004C7 RID: 1223
	private int DouDongDicDown;

	// Token: 0x040004C8 RID: 1224
	private float FirstTimeUp;

	// Token: 0x040004C9 RID: 1225
	private float FirstTimeDown;

	// Token: 0x040004CA RID: 1226
	private byte[] UTF8bytes;

	[SerializeField] private TCPOp1 TCPOp1;

	// Token: 0x040004CB RID: 1227
	private List<string> liststring;
	// Start is called before the first frame update
	void Start()
    {
		//TCPOp1.OpenConnect();
		TCPOp1.Connect();

		//var remotePoint = new IPEndPoint(IPAddress.Parse("192.168.15.100"), 7408);
		//Debug.Log(remotePoint);
	}

	private void FixedUpdate()
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			//TCPOp1.CloseConnect();
			Application.Quit();
		}
		this.SetPlayerRotation();
		this.CamMove();
	}

	// Token: 0x06000353 RID: 851 RVA: 0x00019A18 File Offset: 0x00017E18
	private void SetPlayerRotation()
	{
		if (JoystickCtrl.Instance.m_IsForward || Input.GetKey(KeyCode.W))
		{
			//base.transform.Translate(this.m_Camera_eye.transform.localRotation * Vector3.forward / 3f);
			if (!this.IsDownFen)
			{
				TCPOp1.Fan(3);
				this.IsUpFen = false;
				this.IsDownFen = true;
				this.IsNomeFen = false;
			}
		}
		if (!JoystickCtrl.Instance.m_IsForward && (!JoystickCtrl.Instance.m_IsRear || !Input.GetKey(KeyCode.S)))
		{
			if (!this.IsNomeFen)
			{
				TCPOp1.Fan(0);
				this.IsNomeFen = true;
				this.IsUpFen = false;
				this.IsDownFen = false;
			}
		}
		else
		{
			this.IsNomeFen = false;
		}



		if (JoystickCtrl.Instance.twist == JoystickCtrl.Twist.Left || JoystickCtrl.Instance.m_IsLeft || Input.GetKey(KeyCode.A))
		{
			if (this.TurnLfet)
			{
				TCPOp1.LeftHandedRotation(25);
				this.TurnLfet = false;
				this.bool1 = true;
			}
		}
		else if (JoystickCtrl.Instance.twist == JoystickCtrl.Twist.Right || JoystickCtrl.Instance.m_IsRight || Input.GetKey(KeyCode.D))
		{
			if (this.TurnRight)
			{
				TCPOp1.RightHandedRotation(25);
				this.TurnRight = false;
				this.bool1 = true;
			}
		}
		else if (this.bool1)
		{
			TCPOp1.StopRotation();
			this.TurnLfet = true;
			this.TurnRight = true;
			this.bool1 = false;
		}
	}

    private void OnDestroy()
	{
		//TCPOp1.CloseConnect();
    }

	public void StopRotation()
    {
		TCPOp1.StopRotation();
    }

	public void SerialRestart()
    {
		TCPOp1.SerialRestart();
    }

	//public void ElectricCylinderDistance()
 //   {
	//	TCPOp1.ElectricCylinderDistance(1000, 50, false);
	//}

	public void LeftPositionRotation(int pos)
	{
		TCPOp1.LeftPositionRotation(1, pos);
	}
	
	public void RightPositionRotation(int pos)
    {
		TCPOp1.RightPositionRotation(1, pos);
    }

	private void FristUp()
	{
		if (this.f1 <= 0f && this.fristUp)
		{
			this.fristUp = false;
		}
		else
		{
			this.f1 -= Time.deltaTime;
		}
	}

	// Token: 0x06000355 RID: 853 RVA: 0x00019C26 File Offset: 0x00018026
	private void FristDown()
	{
		if (this.f2 <= 0f && this.fristDown)
		{
			this.fristDown = false;
		}
		else
		{
			this.f2 -= Time.deltaTime;
		}
	}

	private void DownDoudong(ref int pos)
	{
		if (pos == this.DouDongDicDown)
		{
			this.jiajian = true;
		}
		if (pos == 0)
		{
			this.jiajian = false;
		}
		if (this.jiajian)
		{
			pos--;
		}
		else
		{
			pos++;
			if (pos >= this.DouDongDicDown)
			{
				pos = this.DouDongDicDown;
			}
		}
		TCPOp1.ElectricCylinderSin(20, pos, false);
	}

	// Token: 0x06000358 RID: 856 RVA: 0x0001A044 File Offset: 0x00018444
	private void UpDouDong(ref int pos)
	{
		if (pos == this.DouDongDicUp)
		{
			this.jiajian = true;
		}
		if (pos == 100)
		{
			this.jiajian = false;
		}
		if (this.jiajian)
		{
			pos++;
		}
		else
		{
			pos--;
			if (pos <= this.DouDongDicUp)
			{
				pos = this.DouDongDicUp;
			}
		}
		//Debug.Log("UpDouDong" + pos);
		TCPOp1.ElectricCylinderSin(20, pos, false);
	}

	private void CamMove()
	{
		if (this.IsOpenDouDongUp)
		{
			this.UpDouDong(ref this.pos);
		}
		if (this.IsOpenDouDongDown)
		{
			this.DownDoudong(ref this.pos);
		}
		if (JoystickCtrl.Instance.m_IsRear || Input.GetKey(KeyCode.S))
		{
			//MonoBehaviour.print("上升");
			this.tempUpSpeed += 0.1f;
			if (this.tempUpSpeed >= 5f)
			{
				this.tempUpSpeed = 5f;
			}
			//base.transform.GetComponent<Rigidbody>().velocity = base.transform.up * this.tempUpSpeed * 1f;
			if (this.b_Rise)
			{
				this.IsOpenDouDongDown = false;
				this.f1 = this.FirstTimeUp;
				this.currentTime = 0f;
				this.fristUp = true;
				this.b_Decline = true;
				this.b_Rise = false;
				if (!this.IsUpFen)
				{
					TCPOp1.Fan(4);
					this.IsDownFen = false;
					this.IsUpFen = true;
					this.IsNomeFen = false;
					//MonoBehaviour.print("上风");
				}
			}
			this.FristUp();
			if (!this.fristUp)
			{
				this.currentTime += Time.fixedDeltaTime;
				if (this.currentTime >= this.totalTime)
				{
					this.currentTime = 0f;
					if (this.pos < this.DouDongDicUp)
					{
						this.pos++;
						if (this.pos >= 100)
						{
							this.pos = 100;
						}
						this.distance = this.pos;
						if (this.distance >= 100)
						{
							this.distance = 100;
						}
						else if (this.distance <= 0)

						{
							this.distance = 0;
						}
						//MonoBehaviour.print("上升" + this.distance);
						TCPOp1.ElectricCylinderSin(20, this.distance, false);
					}
					else
					{
						this.IsOpenDouDongUp = true;
					}
				}
			}
		}
		else
		{
			this.tempUpSpeed = 0f;
			if (this.b_Decline)
			{
				this.f2 = this.FirstTimeDown;
				this.currentTime = 0f;
				this.fristDown = true;
				this.b_Rise = true;
				this.b_Decline = false;
				this.IsOpenDouDongUp = false;
			}
			this.FristDown();
			if (!this.fristDown)
			{
				this.currentTime += Time.fixedDeltaTime;
				if (this.currentTime >= this.totalTime)
				{
					this.currentTime = 0f;
					if (this.pos > this.DouDongDicDown)
					{
						this.pos--;
						if (this.pos <= 0)
						{
							this.pos = 0;
						}
						this.distance = 200 - this.pos;
						if (this.distance >= 200)
						{
							this.distance = 200;
						}
						else if (this.distance <= 100)
						{
							this.distance = 100;
						}
						TCPOp1.ElectricCylinderSin(20, this.distance, false);
					}
					else
					{
						this.IsOpenDouDongDown = true;
					}
				}
			}
		}
	}
}
