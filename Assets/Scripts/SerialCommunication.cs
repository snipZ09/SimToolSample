using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Timers;
using UnityEngine;

namespace BackGround
{
	// Token: 0x02000007 RID: 7
	public class SerialComunication
    {
        // Token: 0x06000027 RID: 39 RVA: 0x000034C0 File Offset: 0x000016C0
        public SerialComunication()
        {
            this.portName = "COM1";
            this.baudRat = 115200;
            this.builder = new StringBuilder();
            this.sendStr = new StringBuilder();
            this.sendSpecialBuffer = new StringBuilder();
            SerialComunication.currentAngle = 0;
            this.correctTag = 0;
        }

        // Token: 0x06000028 RID: 40 RVA: 0x00003517 File Offset: 0x00001717
        public void StartThread()
        {
            if (this.serialThread == null)
            {
                this.serialThread = new Thread(new ThreadStart(this.OpenSerialPort));
                this.serialThread.Start();
            }
        }

        // Token: 0x06000029 RID: 41 RVA: 0x00003544 File Offset: 0x00001744
        private void OpenSerialPort()
        {
            this.com = new SerialPort(this.portName, this.baudRat, Parity.None, 8, StopBits.One);
            try
            {
                this.com.Open();
                this.EscStop();
                this.com.DataReceived += this.comDataReceived;
                this.EscStop();
                this.SwipeCard(false);
                this.timer = new System.Timers.Timer(500.0);
                this.timer.Elapsed += this.TimerDataSend;
                this.timer.AutoReset = true;
                this.timer.Enabled = true;
            }
            catch (Exception ex)
            {
                Debug.Log("serial connect: " + ex.Message);
                
                Environment.Exit(0);
            }
        }

        // Token: 0x0600002A RID: 42 RVA: 0x0000362C File Offset: 0x0000182C
        private void TimerDataSend(object sender, ElapsedEventArgs e)
        {
            this.comDataSend("FF 4C 05 D2 00 00 00 00 D2 8F");
        }

        // Token: 0x0600002B RID: 43 RVA: 0x0000363C File Offset: 0x0000183C
        public void comDataSend(string str)
        {
            byte[] array = SerialComunication.strToToHexByte(str);
            try
            {
                this.com.Write(array, 0, array.Length);
            }
            catch (Exception ex)
            {
               Debug.Log("comDataSend: " + ex.Message);
            }
        }

        // Token: 0x0600002C RID: 44 RVA: 0x0000368C File Offset: 0x0000188C
        private void comDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int bytesToRead = this.com.BytesToRead;
            byte[] array = new byte[bytesToRead];
            try
            {
                this.com.Read(array, 0, bytesToRead);
            }
            catch (Exception ex)
            {
                Debug.Log("comDataRecv: " + ex.Message);
            }
            foreach (byte b in array)
            {
                this.builder.Append(b.ToString("X2") + " ");
                if (b.ToString("X2").Equals("8F"))
                {
                    Console.WriteLine(this.builder.ToString());
                    this.DataAnalysis(this.builder.ToString());
                    this.builder.Clear();
                }
            }
        }

        // Token: 0x0600002D RID: 45 RVA: 0x00003768 File Offset: 0x00001968
        private static byte[] strToToHexByte(string hexString)
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

        // Token: 0x0600002E RID: 46 RVA: 0x000037D0 File Offset: 0x000019D0
        public void CloseThread()
        {
            try
            {
                this.timer.Enabled = false;
                this.timer.Close();
                this.com.Close();
                this.serialThread.Abort();
            }
            catch (Exception)
            {
            }
        }

        // Token: 0x0600002F RID: 47 RVA: 0x00003820 File Offset: 0x00001A20
        public void ReStart(string str)
        {
            string str2 = "FF 4C 0E C0" + str + "ff 31 8f";
            string str3 = this.CheckCode(str2);
            for (int i = 0; i < 5; i++)
            {
                this.comDataSend(str3);
                Thread.Sleep(10);
            }
        }

        // Token: 0x06000030 RID: 48 RVA: 0x00003860 File Offset: 0x00001A60
        public void SwipeCard(bool b)
        {
            if (b)
            {
                for (int i = 0; i < 5; i++)
                {
                    string str = this.CheckCode("ff 4c 05 ca 00 00 " + SerialComunication.needCoin.ToString("X2") + "01 00 8f");
                    this.comDataSend(this.CheckCode(str));
                    Thread.Sleep(10);
                }
                return;
            }
            for (int j = 0; j < 5; j++)
            {
                this.comDataSend("FF 4C 05 CA 00 00 00 00 ca 8F");
                Thread.Sleep(10);
            }
        }

        // Token: 0x06000031 RID: 49 RVA: 0x00002483 File Offset: 0x00000683
        public void KeybordMouse(bool b)
        {
        }

        // Token: 0x06000032 RID: 50 RVA: 0x000038D4 File Offset: 0x00001AD4
        public void ElectricalMachine(bool b, string speed, string pos = "0")
        {
            int num = Convert.ToInt32(speed);
            int num2 = Convert.ToInt32(pos);
            string text;
            if (b)
            {
                if (num > 120)
                {
                    num = 120;
                }
                else if (num < -120)
                {
                    num = -120;
                }
                if (num >= 0)
                {
                    text = num.ToString("X2");
                }
                else
                {
                    text = num.ToString("X2");
                    text = text.Substring(6, 2);
                }
                string str = this.CheckCode("ff 4c 05 d1 00" + text + "00 00 00 8f");
                this.comDataSend(str);
                return;
            }
            if (num > 120)
            {
                num = 120;
            }
            else if (num < -120)
            {
                num = -120;
            }
            if (num2 > 360)
            {
                num2 = 360;
            }
            else if (num2 < 0)
            {
                num2 = 0;
            }
            if (num >= 0)
            {
                text = num.ToString("X2");
            }
            else
            {
                text = num.ToString("X2");
                text = text.Substring(6, 2);
            }
            string text2;
            if (num2 >= 0)
            {
                text2 = num2.ToString("X4");
            }
            else
            {
                text2 = num2.ToString("X4");
                text2 = text2.Substring(4, 4);
            }
            string str2 = this.CheckCode("ff 4c 05 d1 01 " + text + text2 + "00 8f");
            this.comDataSend(str2);
        }

        // Token: 0x06000033 RID: 51 RVA: 0x000039EC File Offset: 0x00001BEC
        public void SpecialEffects(string str)
        {
            string text = Convert.ToString(Convert.ToInt32(str), 16);
            for (int i = text.Length; i < 8; i++)
            {
                text = "0" + text;
            }
            this.sendSpecialBuffer.Clear();
            this.sendSpecialBuffer.Append("FF 4C 14 ");
            for (int j = 0; j < 4; j++)
            {
                switch (j)
                {
                    case 0:
                        this.sendSpecialBuffer.Append(" F1 00 00 00 ");
                        this.sendSpecialBuffer.Append(text[6]);
                        this.sendSpecialBuffer.Append(text[7]);
                        break;
                    case 1:
                        this.sendSpecialBuffer.Append(" F2 00 00 00 ");
                        this.sendSpecialBuffer.Append(text[4]);
                        this.sendSpecialBuffer.Append(text[5]);
                        break;
                    case 2:
                        this.sendSpecialBuffer.Append(" F3 00 00 00 ");
                        this.sendSpecialBuffer.Append(text[2]);
                        this.sendSpecialBuffer.Append(text[3]);
                        break;
                    case 3:
                        this.sendSpecialBuffer.Append(" F4 00 00 00 ");
                        this.sendSpecialBuffer.Append(text[0]);
                        this.sendSpecialBuffer.Append(text[1]);
                        break;
                }
            }
            this.sendSpecialBuffer.Append(" F8 8F");
            string str2 = this.CheckCode(this.sendSpecialBuffer.ToString());
            this.comDataSend(str2);
        }

        // Token: 0x06000034 RID: 52 RVA: 0x00003B80 File Offset: 0x00001D80
        private string CheckCode(string str)
        {
            byte b = 0;
            byte[] array = SerialComunication.strToToHexByte(str);
            for (int i = 0; i < (int)array[2]; i++)
            {
                b += array[3 + i];
            }
            array[(int)(array[2] + 3)] = b;
            return SerialComunication.byteToHexStr(array);
        }

        // Token: 0x06000035 RID: 53 RVA: 0x00003BBC File Offset: 0x00001DBC
        public static string byteToHexStr(byte[] bytes)
        {
            string text = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    text += bytes[i].ToString("X2");
                }
            }
            return text;
        }

        // Token: 0x06000036 RID: 54 RVA: 0x00003BFC File Offset: 0x00001DFC
        public void RestartPosition()
        {
            string str = "ff 4c 05 d0 01 00 00 00 d1 8f";
            string str2 = this.CheckCode(str);
            for (int i = 0; i < 10; i++)
            {
                this.comDataSend(str2);
                Thread.Sleep(10);
            }
        }

        // Token: 0x06000037 RID: 55 RVA: 0x00003C32 File Offset: 0x00001E32
        public void CloseSpeial()
        {
            this.SpecialEffects("0");
        }

        // Token: 0x06000038 RID: 56 RVA: 0x00003C40 File Offset: 0x00001E40
        public void ChangeAcceleratedSpeed(int speed)
        {
            string str = string.Empty;
            if (speed <= 0)
            {
                speed = 0;
            }
            str = "ff 4c 05 d0 00 01 00 " + speed.ToString("X2") + " d2 8f";
            string str2 = this.CheckCode(str);
            for (int i = 0; i < 5; i++)
            {
                this.comDataSend(str2);
                Thread.Sleep(10);
            }
        }

        // Token: 0x06000039 RID: 57 RVA: 0x00003C98 File Offset: 0x00001E98
        public void Calibration()
        {
            string str = "ff 4c 05 cd 00 cd 00 cd 12 8f";
            string str2 = this.CheckCode(str);
            for (int i = 0; i < 5; i++)
            {
                this.comDataSend(str2);
                Thread.Sleep(10);
            }
        }

        // Token: 0x0600003A RID: 58 RVA: 0x00003CD0 File Offset: 0x00001ED0
        public void EscStop()
        {
            for (int i = 0; i < 5; i++)
            {
                this.comDataSend("FF 4C 05 cd 00 00 00 00 cd 8F");
                Thread.Sleep(10);
            }
        }

        // Token: 0x0600003B RID: 59 RVA: 0x00003CFC File Offset: 0x00001EFC
        public void GetCorrectTag()
        {
            for (int i = 0; i < 5; i++)
            {
                this.comDataSend("FF 4C 05 d5 00 00 00 00 d5 8F");
                Thread.Sleep(10);
            }
            Thread.Sleep(1000);
            if (this.correctTag == 0)
            {
                this.GetCorrectAngle();
                Thread.Sleep(100);
                this.SetCorrectAngle();
            }
        }

        // Token: 0x0600003C RID: 60 RVA: 0x00003D4C File Offset: 0x00001F4C
        public void GetCorrectAngle()
        {
            for (int i = 0; i < 5; i++)
            {
                this.comDataSend("FF 4C 05 d3 00 00 00 00 d3 8F");
                Thread.Sleep(10);
            }
        }

        // Token: 0x0600003D RID: 61 RVA: 0x00003D78 File Offset: 0x00001F78
        private void SetCorrectAngle()
        {
            string[] array = "abc#ac".Split(new char[]
            {
                '#'
            });
            if (!array[0].Equals(this.divTag) || !array[1].Equals(this.AngleTag))
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Clear();
                stringBuilder.Append("ff 4c 05 d3 00 ");
                stringBuilder.Append(array[0]);
                stringBuilder.Append(array[1]);
                stringBuilder.Append(" 00 8f");
                string str = this.CheckCode(stringBuilder.ToString());
                for (int i = 0; i < 5; i++)
                {
                    this.comDataSend(str);
                    Thread.Sleep(10);
                }
            }
            for (int j = 0; j < 5; j++)
            {
                this.comDataSend("FF 4C 05 d5 cd 02 00 00 a4 8F");
                Thread.Sleep(10);
            }
        }

        // Token: 0x0600003E RID: 62 RVA: 0x00003E44 File Offset: 0x00002044
        public void SetZeroPosition()
        {
            for (int i = 0; i < 5; i++)
            {
                this.comDataSend("ff 4c 05 cd 00 dd 00 00 aa 8f");
                Thread.Sleep(10);
            }
        }

        // Token: 0x0600003F RID: 63 RVA: 0x00003E70 File Offset: 0x00002070
        private void DataAnalysis(string str)
        {
            str = str.Trim();
            try
            {
                switch (str[9])
                {
                    case 'C':
                        if (str[10].Equals('A') && str[16].Equals('1'))
                        {
                            //sql.InsertPunchCard(true, 1);
                            //punchCardNum++;
                            //tcp.SendPunchCardMessage();
                        }
                        else if (str[10].Equals('A') && str[13].Equals('1'))
                        {
                            //sql.InsertPunchCard(false, 1);
                            //punchCardNum++;
                            //tcp.SendPunchCardMessage();
                        }
                        else if (str[10].Equals('A') && str[13].Equals('0') && str[22].Equals('0'))
                        {
                            this.isCard = false;
                        }
                        else if (str[10].Equals('2'))
                        {
                            try
                            {
                                //if (monitoring.gameProcess != null)
                                //{
                                //    try
                                //    {
                                //        monitoring.gameProcess.Kill();
                                //        goto IL_16A;
                                //    }
                                //    catch (Exception)
                                //    {
                                //        goto IL_16A;
                                //    }
                                //}
                                //if (monitoring.playerProcess != null)
                                //{
                                //    try
                                //    {
                                //        monitoring.playerProcess.Kill();
                                //    }
                                //    catch (Exception)
                                //    {
                                //    }
                                //}
                            IL_16A:;
                            }
                            catch (Exception)
                            {
                            }
                            this.EscStop();
                        }
                        break;
                    case 'D':
                        if (str[10].Equals('2'))
                        {
                            if (str.Length >= 23)
                            {
                                SerialComunication.currentAngle = Convert.ToInt32(str[18].ToString() + str[19].ToString() + str[21].ToString() + str[22].ToString(), 16);
                                if (SerialComunication.currentAngle >= 360)
                                {
                                    SerialComunication.currentAngle = 360;
                                }
                                else if (SerialComunication.currentAngle <= 0)
                                {
                                    SerialComunication.currentAngle = 0;
                                }
                            }
                        }
                        else if (str[10].Equals('5'))
                        {
                            if (str[16].Equals('0'))
                            {
                                this.correctTag = 0;
                            }
                            else if (str[16].Equals('1'))
                            {
                                this.correctTag = 1;
                            }
                            else if (str[16].Equals('2'))
                            {
                                this.correctTag = 2;
                            }
                            else
                            {
                                this.correctTag = 0;
                            }
                        }
                        else if (str[10].Equals('3'))
                        {
                            this.divTag = str[15].ToString() + str[16].ToString();
                            this.AngleTag = str[18].ToString() + str[19].ToString() + str[21].ToString() + str[22].ToString();
                        }
                        break;
                    case 'E':
                        str[10].Equals('1');
                        break;
                }
            }
            catch (Exception)
            {
            }
        }

        // Token: 0x0400002C RID: 44
        public SerialPort com;

        // Token: 0x0400002D RID: 45
        private StringBuilder builder;

        // Token: 0x0400002E RID: 46
        private string portName;

        // Token: 0x0400002F RID: 47
        private int baudRat;

        // Token: 0x04000030 RID: 48
        private Thread serialThread;

        // Token: 0x04000031 RID: 49
        private System.Timers.Timer timer;

        // Token: 0x04000032 RID: 50
        private StringBuilder sendStr;

        // Token: 0x04000033 RID: 51
        private StringBuilder sendSpecialBuffer;

        // Token: 0x04000034 RID: 52
        public static int currentAngle;

        // Token: 0x04000035 RID: 53
        private Thread recvAngleThread;

        // Token: 0x04000036 RID: 54
        private int correctTag;

        // Token: 0x04000037 RID: 55
        public string divTag;

        // Token: 0x04000038 RID: 56
        public string AngleTag;

        // Token: 0x04000039 RID: 57
        private bool isCard;

        // Token: 0x0400003A RID: 58
        public static int needCoin = 1;
    }
}
