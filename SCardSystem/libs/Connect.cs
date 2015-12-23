using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace SCardSystem.libs
{
    /// <summary>
    /// 建立串口连接，并操作
    /// </summary>
    class Connect
    {

        public static SerialPort serialPort1 = new SerialPort();
        public static string receivedData = null;
        public static bool receiving;
        public static string mID;
        public static string mReadData;
        public static bool isLogin = false;
        public static bool isManager = false;
        public static string staff_id = null;
        public static string device_id = null;

        enum ReceiveStatus
        {
            NULL,
            end1,
            end2
        }

        private static ReceiveStatus receiveStatus = ReceiveStatus.NULL;

        enum Status
        {
            NULL,
            CONNECT, // 默认值为0
            GETID,
            LoadKey,
            READDATA,
            WRITEDATA
        }

        private static Status status = Status.NULL;

        //通过身份证号判断是否被注册过了
        public static bool isRegisteredById(string id)
        {
            string cmd = "select * from base where ID ='"
                + id +
                "';";
            MySqlDataReader reader = DbConnect.getmysqlread(cmd);
            if (reader.Read())
            {
                return true;
            }
            return false;
        }

        //通过身份证和名字判断是否存在
        public static bool isRegisteredBy_ID_Name(string ID ,string Name)
        {
            string cmd = "select name='"+Name+"' from base where ID ='"
                + ID +"';";
            MySqlDataReader reader = DbConnect.getmysqlread(cmd);
            if(reader.Read())
            {
                return true;
            }
            return false;
        }

        //通过卡号判断是否被注册过了
        public static bool isRegisteredByNum(string id)
        {
            string cmd = "select * from base where number ='"
                + id +
                "';";
            MySqlDataReader reader = DbConnect.getmysqlread(cmd);
            if (reader.Read())
            {
                return true;
            }
            return false;
        }

        //销卡
        public static bool makeInactivated(string cardID)
        {
            string cmd = "update base set activate = '0' where number = '"
                + cardID +
                "';";
            DbConnect.getmysqlcom(cmd);
            return true;
        }
        //激活
        public static bool makeActivated(string cardID)
        {
            string cmd = "update base set activate = '1' where number = '"
                + cardID +
                "';";
            DbConnect.getmysqlcom(cmd);
            return true;
        }

        public static string getStuNumByCardId(string cardID)
        {
            string cmd = "select * from schoolcard where cardNum ='"
               + cardID +
               "';";
            MySqlDataReader reader = DbConnect.getmysqlread(cmd);
            if (reader.Read())
            {
                return reader["UID"].ToString();
            }
            return null;
        }

        public static string getCardIdByStuNum(string stuNum)
        {
            string cmd = "select * from schoolcard where UID ='"
               + stuNum +
               "';";
            MySqlDataReader reader = DbConnect.getmysqlread(cmd);
            if (reader.Read())
            {
                return reader["cardNum"].ToString();
            }
            return null;
        }

        /**
        
            判断校园卡是否激活
        */

        public static bool isActivated(string cardID)
        {
            string cmd = "select * from base where number ='"
               + cardID +
               "';";
            MySqlDataReader reader = DbConnect.getmysqlread(cmd);
            if (reader.Read())
            {
                if (reader["activate"].ToString()=="1")
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        //连接串口
        public static bool connectCom()
        {
            serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(serialPort1_DataReceived);
            status = Status.CONNECT;
            try
            {
                if (getComs() != true)
                {
                    return false;
                }
                /**
                上位机串口参数设置要求:   
                1.	波特率:  9600bps  
                2.	数据位:  8位
                3.  停止位:  1位
                4:  奇偶校验: 无
                */
                serialPort1.PortName = "COM3";
                serialPort1.BaudRate = 9600;
                serialPort1.DataBits = 8;
                serialPort1.StopBits = StopBits.One;
                serialPort1.Parity = Parity.None;
                if (serialPort1.IsOpen)
                {
                    serialPort1.Close();
                }
                serialPort1.Open();
                sendCom_str("F1 1F FF FF 53 54 5B F2 2F");
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Error");
                return false;
            }
            return true;
        }

        //检测是否有串口
        public static bool getComs()
        {
            String[] ports = SerialPort.GetPortNames();
            if (ports.Length == 0)
            {
                MessageBox.Show("设备未连接，未找到串口！", "Warning");
                return false;
            }
            MessageBox.Show("设备连接成功！！", "Warning");
            return true;
        }

        public static byte getCheckSum(byte[] bytes)
        {
            byte sum = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                sum += bytes[i];
            }
            sum = (byte)(~sum + 1);
            if (sum > 0xf0)
            {
                sum -= 16;
            }
            return sum;
        }

        /**
            读取ID的工具函数
            返回一个string
            正确返回时ID长度为12位
            调用后需要判断返回ID是否正确
        */
        public static string readID()
        {
            status = Status.GETID;
            receiving = true;
            sendCom_str("F1 1F FF FF C7 C7 74 F2 2F");
            Thread.Sleep(400);
            return mID;
        }

        public static void sendCom_str(string str)
        {
            if (serialPort1.IsOpen)
            {
                byte[] sendData = convertstringtobyte(str);
                serialPort1.Write(sendData, 0, sendData.Length);
            }
        }

        public static byte[] convertstringtobyte(string txt)
        {
            MatchCollection mc = Regex.Matches(txt, @"(?i)[\da-f]{2}");
            List<byte> list = new List<byte>();//填充到这个临时列表中
            //依次添加到列表中
            foreach (Match m in mc)
            {
                list.Add(byte.Parse(m.Value, System.Globalization.NumberStyles.HexNumber));
            }
            byte[] byteBuffer = new byte[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                byteBuffer[i] = (byte)list[i];
            }
            return byteBuffer;
        }

        /**
            加载秘钥，默认秘钥 FF FF FF FF FF FF
        */
        public static bool loadKey(String key)
        {
            status = Status.LoadKey;
            Byte[] bytes = { 0xFF, 0xFF, 0xC1, 0xC1, 0x06, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x80, 0xf2, 0x2f };
            sendCom_bytes(bytes);
            return true;
        }


        //读卡
        public static string readData(int block_id)
        {
            status = Status.READDATA;
            Byte[] tempbytes = new Byte[12];
            tempbytes[0] = 0xF1;
            tempbytes[1] = 0x1F;
            tempbytes[2] = 0xFF;
            tempbytes[3] = 0xFF;
            tempbytes[4] = 0xC2;
            tempbytes[5] = 0xC2;
            tempbytes[6] = 0x02;
            tempbytes[7] = (byte)block_id;//要读的块号
            tempbytes[8] = 0x60;
            Byte[] dataBytes = {tempbytes[2],tempbytes[3],
                tempbytes[4],tempbytes[5] ,
                tempbytes[6], tempbytes[7],tempbytes[8] };
            tempbytes[9] = getCheckSum(dataBytes);
            tempbytes[10] = 0xF2;
            tempbytes[11] = 0x2F;
            sendCom_bytes(tempbytes);
            Thread.Sleep(200);
            return mReadData;
        }

        //写卡
        public static void writeData(string data_to_write, int block_id)
        {
            status = Status.WRITEDATA;
            Byte[] tempbytes = new Byte[28];
            Byte[] data_to_write_bytes =Encoding.ASCII.GetBytes(data_to_write);
            tempbytes[0] = 0xF1;
            tempbytes[1] = 0x1F;
            tempbytes[2] = 0xFF;
            tempbytes[3] = 0xFF;
            tempbytes[4] = 0xC3;
            tempbytes[5] = 0xC3;
            tempbytes[6] = 0x12;
            tempbytes[7] = (byte)block_id;//写入的块号
            tempbytes[8] = 0x60;
            for (int i = 0; i < 16; i++)
            {
                if (i >= data_to_write_bytes.Length)
                {
                    tempbytes[9 + i] = 32;
                }
                else
                {
                    tempbytes[9 + i] = data_to_write_bytes[i];
                }
            }
            Byte[] bb = new Byte[23];
            for (int i = 0; i < bb.Length; i++)
            {
                bb[i] = tempbytes[i + 2];
            }
            tempbytes[25] = getCheckSum(bb);
            tempbytes[26] = 0xF2;
            tempbytes[27] = 0x2F;
            sendCom_bytes(tempbytes);
            Thread.Sleep(200);
        }

        public static void sendCom_bytes(Byte[] sendData)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Write(sendData, 0, sendData.Length);
            }
        }
        public static void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            receiving = true;
            Thread.Sleep(300);
            try
            {
                byte[] bytesRead = new byte[serialPort1.BytesToRead];
                serialPort1.Read(bytesRead, 0, bytesRead.Length);
                receivedData = null;
                for (int i = 0; i < bytesRead.Length; i++)
                {
                    byte temp = bytesRead[i];
                    var dataRe = temp.ToString("X2") + " ";
                    receivedData += dataRe;
                    Debug.WriteLine(receivedData);
                    switch (receiveStatus)
                    {
                        case ReceiveStatus.NULL:
                            if (dataRe.Equals("F4 "))
                                receiveStatus = ReceiveStatus.end1;
                            break;
                        case ReceiveStatus.end1:
                            if (dataRe.Equals("4F "))
                            {
                                receiveStatus = ReceiveStatus.end2;
                                //  MessageBox.Show("接收完毕", "info");
                                receivedDone();
                            }
                            else
                            {
                                receiveStatus = ReceiveStatus.NULL;
                            }
                            break;
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private static void receivedDone()
        {
            if (receiveStatus == ReceiveStatus.end2)
            {
                receiveStatus = ReceiveStatus.NULL;
            }
            switch (status)
            {
                case Status.CONNECT:
                    if (true)
                    {//根据接收到的数据判断是否成功连接设备
                        MessageBox.Show("设备连接成功！", "info");
                    }
                    receivedData = null;
                    break;

                case Status.LoadKey:
                    break;

                case Status.READDATA:
                    Byte[] received_bytes = convertstringtobyte(receivedData);
                    mReadData = null;
                    Byte[] bb = new Byte[16];
                    for (int i = 8; i < received_bytes.Length - 3; i++)
                    {
                        bb[i - 8] = received_bytes[i];
                    }
                    mReadData = Encoding.ASCII.GetString(bb);
                    receivedData = null;
                    break;

                case Status.GETID:
                    string id = null;
                    byte[] bytes = convertstringtobyte(receivedData);
                    for (int i = 8; i < bytes.Length - 3; i++)
                    {
                        id += bytes[i].ToString("X2") + " ";
                    }
                    mID = id;
                    receivedData = null;
                    break;

                default:
                    receivedData = null;
                    break;
            }
        }



    }
}
