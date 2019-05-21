using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModuleReader;
using Model;
using MySql.Data.MySqlClient;
using DesktopUHF;
using Newtonsoft.Json;
using System.Threading;

namespace ReaderManager
{
    //位置摆放有误的图书
    public class Misbook
    {
        public string id { get; set; }
        public string name { get; set; }
        public string correctpositon { get; set; }      //图书正确摆放位置
        public string currentposition { get; set; }     //图书当前摆放位置      
        
        public Misbook(string idstr,string namestr,string corp,string curp)
        {
            id = idstr;
            name = namestr;
            correctpositon = corp;
            currentposition = curp;
        }
    }

    

    /// <summary>
    /// 固定式读写器的相关功能操作
    /// </summary>
    public class ModuleReadManage
    {
        ModuleRead mreader = new ModuleRead();

        List<Misbook> misbooks = new List<Misbook>();   //用于存放所有放错位置图书书的信息

        /// <summary>
        /// 盘存，查询各书架上的图书位置是否摆放正确
        /// </summary>
        /// <returns></returns>
        public string  Inventory()
        {
            misbooks.Clear();
            
            string ip = "192.168.0.103";
            mreader.Connect(ip, 4);
            mreader.ReadStart(4);
            List<TagInfo> tags = mreader.Getbookmessages();
            foreach (TagInfo tag in tags)
            {
                MySqlDataReader myread1 = null;
                MySqlDataReader myread2 = null;
                try
                {
                    Mysql mysql = new Mysql();
                    //根据图书id查询图书正确的摆放位置
                    string sqlstr1 = "SELECT * FROM book where bookid = " + "'" + tag.epcid + "'";                   
                    myread1 =  mysql.Getmysqlread(sqlstr1);
                    //根据天线id查询图书当前的摆放位置
                    string sqlstr2 = "SELECT * FROM bookhelf where shelfid = " + "'" + tag.antid + "'";
                    myread2 = mysql.Getmysqlread(sqlstr2);
                }
                catch(Exception ex) { }
                myread1.Read();
                string corp = myread1["position"].ToString();
                myread2.Read();
                string curp = myread2["shelfname"].ToString();
                if (corp!= curp)
                {
                    //string bookid = tag.epcid;
                    Misbook misbook = new Misbook(tag.epcid, myread1["bookname"].ToString(), myread1["position"].ToString(), myread2["name"].ToString());
                    misbooks.Add(misbook);
                }
            }

            return JsonConvert.SerializeObject(misbooks);
        }


        Thread thread1;
        /// <summary>
        /// 学生/教师/职工离开
        /// </summary>
        public void  UserOut()
        {
            string ip = "192.168.0.102";
            mreader.Connect(ip, 1);
            thread1 = new Thread(new ThreadStart(Userread));
            thread1.Start();                    
        }
        public void Userread()
        {
            while (true)
            {
                mreader.ReadStart(1);
                List<string> idstrs = mreader.Getusermessages();
                foreach (string idstr in idstrs)
                {
                    if (idstr.StartsWith("U") | idstr.StartsWith("M") | idstr.StartsWith("D") | idstr.StartsWith("T"))
                    {
                        Mysql mysql = new Mysql();
                        //更改usertag表中与用户id对应的有源标签的状态
                        string sqlstr1 = "update usertag set tagstatus = 2 where userid = " + "'" + idstr + "'";
                        mysql.Getmysqlcom(sqlstr1);
                        //查询usertag表中与用户id对应的tagno,更改activetag表中有源标签的状态
                        string sqlstr2 = "select * from usertag where userid = " + "'" + idstr + "'";
                        MySqlDataReader myread = mysql.Getmysqlread(sqlstr2);
                        myread.Read();
                        string tagnostr = myread["tagno"].ToString();
                        string sqlstr3 = "update activetag set tagstatus = 0 where tagno = " + "'" + tagnostr + "'";
                        mysql.Getmysqlcom(sqlstr3);
                    }
                }
                Thread.CurrentThread.Join(100);//阻止设定时间
            }

        }
        //断开线程
        public void UseroutStop()
        {
            thread1.Abort();
        }


        /// <summary>
        /// 查询图书信息
        /// </summary>
        /// <returns></returns>
        public string QueryBook()
        {
            return "";
        }
    }

    /// <summary>
    /// 桌面式发卡器的相关功能操作
    /// </summary>
    public class DesktopReadManage
    {
        DesktopRead dreader = new DesktopRead();

        /// <summary>
        /// 学生/教师/职工/管理员进入             
        /// </summary>
        /// <returns></returns>
        public bool UserEnter()
        {
            bool flag = false;
            string idstr = null;
            MySqlDataReader myread1 = null;
            MySqlDataReader myread2 = null;            

            dreader.Connect();
            idstr = dreader.ReadEPC();

            Mysql mysql = new Mysql();

            if (idstr.StartsWith("U")| idstr.StartsWith("M")| idstr.StartsWith("D")|idstr.StartsWith("T"))
            {
                string sqlstr1 = "select * from user where userid = " + "'" + idstr + "'";
                myread1 = mysql.Getmysqlread(sqlstr1);
                if(myread1 != null)
                {
                    flag = true;

                    myread1.Read();
                    string useridstr = myread1["userid"].ToString();

                    string sqlstr2 = "select * from activetag where tagstatus = 0";
                    myread2 = mysql.Getmysqlread(sqlstr2);
                    myread2.Read();
                    string tagnostr = myread2["tagno"].ToString();
                    string timestr = DateTime.Now.ToString();

                    string sqlstr3 = "update activetag set tagstatus = 1 where tagno = " + "'" + tagnostr + "'";
                    mysql.Getmysqlcom(sqlstr3);
                    string sqlstr4 = "insert into usertag (userid,tagno,time,tagstatus) value " + "('" + useridstr + "','" + tagnostr + "','" + timestr + "',1)";
                    mysql.Getmysqlcom(sqlstr4);
                }
            }
            else if (idstr.StartsWith("S"))
            {
                string sqlstr = "select * from user where userid = " + "'" + idstr + "'";
                myread1 = mysql.Getmysqlread(sqlstr);
                if(myread1 != null)
                {
                    flag = true;
                }
            }
            else if (idstr.StartsWith("A"))
            {
                string sqlstr = "select * from libadmin where adminid = " + "'" + idstr + "'";
                myread1 = mysql.Getmysqlread(sqlstr);
                if (myread1 != null)
                {
                    flag = true;
                }
            }           
            else
            {
                flag = false;
            }
            
            return flag ;
        }
       

        /// <summary>
        /// 借书时获取用户id、name
        /// </summary>
        /// <returns></returns>
        public string GetUserid()
        {
            string idstr = null;
            MySqlDataReader myread = null;

            dreader.Connect();
            idstr = dreader.ReadEPC();

            Mysql mysql = new Mysql();
            string sqlstr = "select * from user where userid = " + "'" + idstr + "'";
            myread = mysql.Getmysqlread(sqlstr);
            myread.Read();
            User user = new User(myread["userid"].ToString(), myread["username"].ToString());

            return JsonConvert.SerializeObject(user);

        }

        Thread thread2;
        /// <summary>
        /// 借书时，获取图书信息??????????
        /// </summary>
        /// <returns></returns>
        public void  GetBook(string userid)
        {
            dreader.Connect();
            thread2 = new Thread(new ThreadStart(Getbookread));
            thread2.Start();

        }
        public string  Getbookread(string userid)
        {
            Book book = null;
            string bookidstr = dreader.ReadEPC();   //获取图书id

            MySqlDataReader myread = null;
            Mysql mysql = new Mysql();
            string sqlstr1 = "update book set bookstatus = 1 where bookid = " + "'" + bookidstr + "'";
            mysql.Getmysqlcom(sqlstr1);
            string sqlstr2 = "insert bookorder (userid,bookid,borrowdate,outdate,orderstatus) value " + "('" + userid + "','" + bookidstr + "','" + DateTime.Now.ToShortDateString().ToString() + "','" + DateTime.Now.Date.AddDays(30).ToShortDateString().ToString() + "',0 )";
            mysql.Getmysqlcom(sqlstr2);
            string sqlstr3 = "select * from book where bookid = " +"'"+ bookidstr+"'";
            myread = mysql.Getmysqlread(sqlstr3);
            while (myread.Read())
            {
                book = new Book(myread["callnumber"].ToString(), myread["name"].ToString(),1);  //1:被借阅，借阅成功；0：借阅失败
            }
            return JsonConvert.SerializeObject(book);
        }

        public void GetbookStop()
        {
            thread2.Abort();
        }


        Thread thread3;
        /// <summary>
        /// 归还图书
        /// </summary>
        public void ReturnBook()
        {
            dreader.Connect();
            thread3 = new Thread(new ThreadStart(Bookread));
            thread3.Start();
        }

        public void Bookread()
        {
            while(true)
            {
                string bookidstr = dreader.ReadEPC();
                
                Mysql mysql = new Mysql();
                //修改book表中的图书馆藏状态bookstatus
                string sqlstr1 = "update book set bookstatus = 0 where bookid = " + "'" + bookidstr + "'";
                mysql.Getmysqlcom(sqlstr1);
                //修改bookorder表中的借阅状态orderstatus
                string sqlstr2 = "update bookorder set orderstatus = 2,returndate =" +"'" +DateTime.Now.ToShortDateString().ToString() +"'"+ "where bookid = " + "'" + bookidstr + "'";
                mysql.Getmysqlcom(sqlstr2);
                
                Thread.CurrentThread.Join(100);//阻止设定时间
            }            
        }

        public void ReturnbookStop()
        {
            thread3.Abort();
        }
    }

    




    /// <summary>
    /// 图书管理相关功能操作（增、增、删、查、改）
    /// </summary>
    public class BooksManage
    {
        /// <summary>
        /// 新书入库
        /// </summary>
        public void Addbook(string idstr, string number, string namestr, string authorstr, string publisherstr, string positionstr, string statusstr)
        {

        }

        
    }

    

}
