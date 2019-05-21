using ModuleTech;
using ModuleTech.Gen2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleReader
{
    public class TagInfo
    {
        //public TagInfo(string epc, int rcnt, int ant, DateTime time, byte[] emd)
        public TagInfo(string epc, int ant)
        {
            epcid = epc;
            //readcnt = rcnt;
            antid = ant;
            //timestamp = time;
            //ebdata = emd;           
        }
        public string epcid;
        //public int readcnt;
        public int antid;
        //public DateTime timestamp;
        //public byte[] ebdata;               

    }

    public class ModuleRead
    {

        public Reader modulerdr = null;


        //string cur_dir = Environment.CurrentDirectory;

        TagReadData[] reads;

        //Thread readThread = null;

        /// <summary>
        /// 读写器连接和参数设置
        /// </summary>
        public bool Connect(string sourceip, int antcounts)
        {
            try
            {
                modulerdr = Reader.Create(sourceip, ModuleTech.Region.NA, antcounts);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("连接失败，请检查读写器地址是否正确" + ex.ToString());
                return false;
            }


            //读写器参数设置
            try
            {
                //Environment.CurrentDirectory = cur_dir;     //??
                StreamReader confReader = new StreamReader("sygole.conf", System.Text.Encoding.Default);        //sygole.conf文件
                char[] dep1 = new char[1];
                dep1[0] = ':';
                string sLine = "";
                while (sLine != null)
                {
                    sLine = confReader.ReadLine();
                    if (sLine != null && !sLine.Equals(""))
                    {
                        string[] para = sLine.Split(dep1, StringSplitOptions.None);
                        if (para[0] == "defaultpower")
                        {
                            float dp = float.Parse(para[1]);
                            //ushort power = (ushort)((float)3000);
                            AntPower[] apwrs = new AntPower[4];
                            for (int c = 0; c < 4; ++c)
                            {
                                apwrs[c].AntId = (byte)(c + 1);
                                apwrs[c].WritePower = (ushort)((int)modulerdr.ParamGet("RfPowerMax") * 0.8);
                                apwrs[c].ReadPower = (ushort)((int)modulerdr.ParamGet("RfPowerMax") * 0.8);
                            }
                            modulerdr.ParamSet("AntPowerConf", apwrs);      //每个天线读写功率的具体设置

                        }
                        else if (para[0] == "checkantenna")
                        {
                            if (para[1] == "true")
                            {
                                modulerdr.ParamSet("CheckAntConnection", true);     //是否在发射信号前检查天线的物理连接设置
                            }
                            else if (para[1] == "false")
                            {
                                modulerdr.ParamSet("CheckAntConnection", false);
                            }

                        }
                        else if (para[0] == "gen2session")
                        {
                            int sess = int.Parse(para[1]);
                            modulerdr.ParamSet("Gen2Session", (Session)sess);       //Gen2协议session设置，注：sygole.conf文件中“gen2session:0”
                        }
                        else if (para[0] == "SetGPO1")
                        {
                            if (para[1] == "true")
                            {
                                modulerdr.GPOSet(1, true);
                            }
                            else if (para[1] == "false")
                            {
                                modulerdr.GPOSet(1, false);
                            }
                        }
                    }
                }

                confReader.Close();
                confReader = null;
            }
            catch (Exception exxp)
            {
                //MessageBox.Show("读配置文件错误");
                return false;

            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void DisConnect()
        {
            modulerdr.Disconnect();
        }


        /// <summary>
        ///进行Read操作
        /// </summary>
        public void ReadStart(int antcounts)
        {

            //int[] ants = new int[] { 1 };
            int[] ants = new int[antcounts] ; 
            for(int i=0;  i<antcounts; i++ )
            {
                ants[i] = i+1;

            }

            EmbededCmdData ecd = new EmbededCmdData(MemBank.USER, 0, 20);        //读USER区从第0个块开始的20-0个块的数据。
            modulerdr.ParamSet("EmbededCmdOfInventory", ecd);

            // Gen2TagFilter filter = new Gen2TagFilter(ByteFormat.FromHex("1234"), MemBank.EPC, 32, false); //只读EPC码以0x1234开头的标签
            //modulerdr.ParamSet("Singulation", filter);

            SimpleReadPlan readplan = new SimpleReadPlan(TagProtocol.GEN2, ants);      //用于指定操作执行的协议（Gen2）和天线（1，2，3，4）

            modulerdr.ParamSet("ReadPlan", readplan);       //执行Read操作时的天线设置

            reads = modulerdr.Read(200);

            //return reads;

        }


        /*Dictionary<string, TagInfo> tagsMessage = new Dictionary<string, TagInfo>();      
        /// <summary>
        /// 读取各个标签信息
        /// </summary>
        public Dictionary<string, TagInfo> GettagsMessage()
        {
            tagsMessage.Clear();
            foreach (TagReadData read in reads)
            {
                //string keystr = read.EPCString;
                string keystr = Encoding.Default.GetString(read.EPC);
                if (tagsMessage.ContainsKey(keystr))
                {
                    tagsMessage[keystr].readcnt += read.ReadCount;
                    tagsMessage[keystr].antid = read.Antenna;                   
                    tagsMessage[keystr].timestamp = read.Time;
                    tagsMessage[keystr].ebdata = read.EbdData;

                }
                else
                {
                    TagInfo tag = new TagInfo(keystr, read.ReadCount, read.Antenna, read.Time, read.EbdData);
                   
                    tagsMessage.Add(keystr, tag);
                }
            }

            return tagsMessage;
        }*/


        List<TagInfo> tagsMessage = new List<TagInfo>();
        /// <summary>
        /// 读取图书标签信息(图书id，天线id(即书架id))
        /// </summary>
        public List<TagInfo> Getbookmessages()
        {
            tagsMessage = null;
            foreach (TagReadData read in reads)
            {
                string epc = Encoding.Default.GetString(read.EPC);
                int antenna = read.Antenna;
                TagInfo tag = new TagInfo(epc, antenna);
                if (tagsMessage.Contains(tag))
                {
                    continue;
                }
                else
                {
                    tagsMessage.Add(tag);
                }
            }
            return tagsMessage;
        }

        List<string> epcids = new List<string>();
        /// <summary>
        /// 读取校园卡信息（用户id）
        /// </summary>
        /// <returns></returns>
        public List<string> Getusermessages()
        {
            epcids = null;
             
            foreach (TagReadData read in reads)
            {
                string epc = Encoding.Default.GetString(read.EPC);
                if(epcids.Contains(epc))
                {
                    continue;
                }
                else
                {
                    epcids.Add(epc);
                }
            }
            return epcids;
        }

    }
}
