using ReaderB;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopUHF
{
    public class DesktopRead
    {// 端口号
        int Port = -1;

        // 读写器地址
        byte ComAddr = 0xff;


        /*  波特率对应值
            0	9600bps
            1	19200 bps
            2	38400 bps
            5	57600 bps
            6	115200 bps
        */
        byte Baud = 5;

        //波特率 57600 bps
        int lBaud = 57600;

        // 句柄
        int PortHandle = -1;


        /// <summary>
        /// 连接桌面式读写器。返回true，连接成功；返回false，连接失败
        /// </summary>
        /// <returns></returns>
        public bool  Connect()
        {
            if(Port>0)
            {
                // 关闭串口
                StaticClassReaderB.CloseSpecComPort(PortHandle);
            }
            try
            {
                string[] portNames = SerialPort.GetPortNames();
                foreach (string portName in portNames)
                {
                    SerialPort sp = new SerialPort(portName, 57600, Parity.None, 8, StopBits.One);

                    if (!sp.IsOpen)
                    {
                        sp.Open();
                        sp.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            // 打开串口
            int iRet = StaticClassReaderB.AutoOpenComPort(ref Port, ref ComAddr, Baud, ref PortHandle);
           
            if (iRet == 0)
            {
                return true;    // 串口打开成功
            }
            else
            {                
                return false;   // 串口打开失败
            }

        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <returns></returns>
        public bool DisConnect()
        {
            // 关闭串口
            int iRet = StaticClassReaderB.CloseSpecComPort(Port);
            
            if (iRet == 0)
            {
                Port = -1;
                PortHandle = -1;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 查询EPC值
        /// </summary>
        /// <returns>返回标签EPC值</returns>
        public string ReadEPC()
        {
            return Inventory_G2((byte)0, (byte)0, (byte)0);
        }

        /// <summary>
        /// 查询TID值
        /// </summary>
        /// <returns>返回标签TID值</returns>
        public string ReadTID()
        {
            return Inventory_G2((byte)0, (byte)8, (byte)1);
        }

        /// <summary>
        /// 查询标签EPC或TID信息
        /// </summary>
        /// <param name="AdrTID">查询TID的起始地址,为0</param>
        /// <param name="LenTID">以字为单位的TID长度</param>
        /// <param name="TIDFlag">标志位，0表示查询EPC,1表示查询TID</param>
        /// <returns></returns>
        public string Inventory_G2( byte AdrTID, byte LenTID,byte TIDFlag)
        {
            if (Port == 0)
            {
                return "";
            }
            // 一字节的EPC的长度+ EPC的字节数组表示 
            byte[] EPClenandEPC = new byte[5000];

            // 返回的EPC的实际字节数
            int Totallen = 0;

            // 查询到的标签数量
            int CardNum = 0;                       

            string hexEPC = "";

            //Inventory_G2(读写器地址,询查TID的起始地址,询查TID的字数,询查TID的标志,EPClenandEPC(EPC数据+EPC号),EPClenandEPC的字节数,电子标签的张数,与读写器连接端口对应的句柄)
            int fCmdRet = StaticClassReaderB.Inventory_G2(ref ComAddr, AdrTID, LenTID, TIDFlag, EPClenandEPC, ref Totallen, ref CardNum, PortHandle);

            if ((fCmdRet == 1) | (fCmdRet == 2) | (fCmdRet == 3) | (fCmdRet == 4) | (fCmdRet == 0xFB))  //代表已查找结束
            {
                byte[] desArray = new byte[Totallen];

                Array.Copy(EPClenandEPC, desArray, Totallen);

                //string temps = ByteArrayToHexString(desArray);       
                string temps = ByteArrayToString(desArray); //二进制字节数组转换为ASCII字符串(数据长度+数据部分)

                if (CardNum > 0)
                {
                    int EPClen = desArray[0];   //desArray[0]为读取的数据长度
                    //hexEPC = temps.Substring(2, EPClen * 2);
                    hexEPC = temps.Substring(1, EPClen);    //读取temps中数据部分
                }
            }

            return hexEPC;
        }

        /// <summary>
        /// EPC区写入，十六进制EPC字符串转换成二进制数组
        /// </summary>
        /// <param name="HexEpc">写入EPC的字符串，长度必须为4的倍数，且其最大长度受标签芯片EPC区域存储空间。</param>
        /// <returns></returns>
        public bool WriteEPC(String HexEpc)
        {
            // 返回错误代码
            int ferrorcode = -1;

            int fCmdRet = 0;

            // 标签密码00000000
            byte[] Password = HexStringToByteArray("00000000");

            // 准备写入的EPC值的字节数组表示
            //byte[] writeEpc = HexStringToByteArray(HexEpc);
            byte[] writeEpc = StringToByteArray(HexEpc);

            // EPC的字节数，必须为偶数
            byte WriteEPClen = (byte)writeEpc.Length;

            fCmdRet = StaticClassReaderB.WriteEPC_G2(ref ComAddr, Password, writeEpc, WriteEPClen, ref ferrorcode, PortHandle);

            if (fCmdRet == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// USER区读取，返回ASCII字符串
        /// </summary>
        /// <param name="hexEPC">十六进制epc字符串,调用方法ReadEPC()</param> 
        /// <param name="offset">十进制偏移字数</param>
        /// <param name="readNum">十进制读取字数</param>
        /// <returns></returns>
        public string ReadUSER(string hexEPC, string offset, string readNum)
        {
            string userData = "";

            if (Port == 0)
            {
                return "";
            }

            if (offset.Length <= 0 || readNum.Length <= 0)
            {
                return "";
            }

            byte[] EPC = new byte[hexEPC.Length / 2];
            //EPC = HexStringToByteArray(hexEPC);
            EPC = StringToByteArray(hexEPC);
            byte Mem = 0x03;    //用户区
            byte WordPtr = Convert.ToByte(offset.Trim(), 10);
            byte Num = Convert.ToByte(readNum);
            byte[] fPassWord = HexStringToByteArray("00000000");
            byte maskadr = 0;
            byte maskLen = 0;
            byte maskFlag = 0;
            byte[] Data = new byte[1320];
            byte EPClength = (Byte)EPC.Length;
            Int32 ferrorcode = -1;
            
            Int32 fCmdRet = StaticClassReaderB.ReadCard_G2(ref ComAddr, EPC, Mem, WordPtr, Num, fPassWord, maskadr, maskLen, maskFlag, Data, EPClength, ref ferrorcode, PortHandle);

            if (fCmdRet == 0)
            {
                byte[] daw = new byte[Num * 2];
                Array.Copy(Data, daw, Num * 2);
                userData = ByteArrayToString(daw);  //二进制数组转换为ASCII字符串
            }
            return userData;
        }

        /// <summary>
        /// USER区写入,字符串转换成二进制数组
        /// </summary>
        /// <param name="hexEPC">十六进制EPC</param>
        /// <param name="offset">十进制偏移字节数</param>
        /// <param name="writeNum">需写入字符串,字节数应为偶数</param>
        /// <returns></returns>
        public bool WriteUSER(string hexEPC, int offset, string writeNum)
        {
            byte[] writeData = new byte[writeNum.Length];
            writeData = StringToByteArray(writeNum);

            if (Port == 0)
            {
                return false;
            }

            if (offset < 0 || writeData.Length <= 0)
            {
                return false;
            }
            offset = offset / 2;
           
            byte[] EPC = new byte[hexEPC.Length / 2];
            //EPC = HexStringToByteArray(hexEPC);
            EPC = StringToByteArray(hexEPC);
            byte Mem = 0x03;//用户区
            byte WordPtr = Convert.ToByte(offset.ToString(), 10);
            byte bwriteLen = (Byte)writeData.Length;
            byte[] fPassWord = HexStringToByteArray("00000000");
            byte maskFlag = 0;
            byte maskadr = 0;
            byte maskLen = 0;
            Int32 WrittenDataNum = 0;
            byte EPClength = (Byte)EPC.Length;
            Int32 ferrorcode = -1;

            int fCmdRet = 0;
          
            for (int tryCnt = 0; tryCnt < 20; tryCnt++)     //最多尝试写入20次
            {
                fCmdRet = StaticClassReaderB.WriteCard_G2(ref ComAddr, EPC, Mem, WordPtr, bwriteLen, writeData, fPassWord, maskadr, maskLen, maskFlag, WrittenDataNum, EPClength, ref ferrorcode, PortHandle);

                if (fCmdRet == 0)
                {
                    break;
                }
            }

            if (fCmdRet == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 二进制数组转为十六进制字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            return sb.ToString().ToUpper();

        }

        /// <summary>
        /// 十六进制字符串转为二进制数组
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        /// <summary>
        /// 二进制数组转换为ASCII字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] data)
        {
            return Encoding.Default.GetString(data);
        }

        /// <summary>
        /// ASCII字符串转换为二进制数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private byte[] StringToByteArray(String str)
        {
            return Encoding.Default.GetBytes(str);
        }
    }
}
