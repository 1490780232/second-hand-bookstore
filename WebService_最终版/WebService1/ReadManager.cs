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
        public string callnumber { get; set; }  //索书号
        public string correctposition { get; set; }      //图书正确摆放位置
        public string currentposition { get; set; }     //图书当前摆放位置  
        public string name { get; set; }    //书名(包括版本)
        public string author { get; set; }  //作者
        public string publisher { get; set; }

        public Misbook()
        {

        }
                  
        public Misbook(string number,string corp,string curp,string namestr,string authorstr,string publisherstr)
        {
            callnumber = number;
            correctposition = corp;
            currentposition = curp;
            name = namestr;
            author = authorstr;
            publisher = publisherstr;
        }
    }

    public class Querybook
    {
        public string bookid { get; set; }      //图书唯一标识
        public string isbn { get; set; }    //条形码
        public string callnumber { get; set; }  //索书号
        public string bookname { get; set; }        //题名
        public string author { get; set; }       //作者
        public string publisher { get; set; }   //出版社
        public string position { get; set; }       //馆藏位置，等同于Bookshelf中的Name
        public int bookstatus { get; set; }     //馆藏状态：0:在架上；1：被借阅；2：馆内阅读；3：损坏遗失
        public string curposition { get; set; }     //图书当前摆放位置      

        public Querybook(string idstr,string isbnstr, string number, string namestr, string authorstr, string publisherstr, string positionstr, int statusstr,string curstr)
        {
            bookid = idstr;
            isbn = isbnstr;
            callnumber = number;
            bookname = namestr;
            author = authorstr;
            publisher = publisherstr;
            position = positionstr;
            bookstatus = statusstr;
            curposition = curstr;
        }
    }



    /// <summary>
    /// 固定式读写器的相关功能操作
    /// </summary>
    public class ModuleReadManage
    {
        ModuleRead mreader = new ModuleRead();

        /// <summary>
        /// 断开固定式读写器的连接
        /// </summary>
        /// <returns></returns>/*
        public void  MouduledisConnect()
        {
            //thread1.Abort();
            //thread2.Abort();
            mreader.DisConnect();
            
        }
       
        

        /// <summary>
        /// 盘存，查询各书架上的图书位置是否摆放正确
        /// </summary>
        /// <returns></returns>
        public string  Inventory()
        {
            List<Misbook> misbooks = new List<Misbook>();   //用于存放所有放错位置图书书的信息
            
            string ip = "192.168.0.103";
            mreader.Connect(ip, 4);
            List<TagInfo> tags = mreader.ReadStart(4);
            
            foreach (TagInfo tag in tags)
            {
                MySqlDataReader myread1 = null;
                MySqlDataReader myread2 = null;
                string curp = null;
                string corp = null;

                try
                {
                    Mysql mysql = new Mysql();
                    //根据图书id查询图书正确的摆放位置
                    string sqlstr1 = "SELECT * FROM book where bookid = " + "'" + tag.epcid + "'";                   
                    myread1 =  mysql.Getmysqlread(sqlstr1);
                    //根据天线id查询图书当前的摆放位置
                    string sqlstr2 = "SELECT * FROM bookshelf where shelfid = " + "'" + tag.antid + "'";
                    myread2 = mysql.Getmysqlread(sqlstr2);
                }
                catch(Exception ex) { }
                while(myread1.Read())
                {
                    corp = myread1["position"].ToString();
                }
                while (myread2.Read())
                {
                    curp = myread2["shelfname"].ToString();
                }

                if (corp!=null&curp!=null&corp != curp)
                {
                    //string bookid = tag.epcid;
                    Misbook misbook = new Misbook(myread1["callnumber"].ToString(),corp, curp, myread1["bookname"].ToString(), myread1["author"].ToString(),myread1["publisher"].ToString());
                    misbooks.Add(misbook);
                }
                else
                {
                    continue;
                }
            }

            mreader.DisConnect();

            return JsonConvert.SerializeObject(misbooks);
            //return JsonConvert.SerializeObject(tags);
            
        }

        /// <summary>
        /// 定时盘点，通过将读取当前在架上的图书与数据库中的数据进行比较，从而修改图书状态
        /// </summary>
        public void TimedInventory()
        {
            List<string> bookson = new List<string>();
            List<string> allbooks = new List<string>();
            Mysql mysql = new Mysql();

            string ip = "192.168.0.103";
            mreader.Connect(ip, 4);
            List<TagInfo> tags = mreader.ReadStart(4);
            
            foreach (TagInfo tag in tags)
            {
                if(bookson.Contains(tag.epcid))
                {
                    continue;
                }
                else
                {
                    bookson.Add(tag.epcid);

                    MySqlDataReader myread1 = null;
                    int status1 = 0;
                    
                    string sqlstr1 = "select * from book where bookid = '" + tag.epcid + "'";
                    myread1 = mysql.Getmysqlread(sqlstr1);
                    while (myread1.Read())
                    {
                        status1 = Convert.ToInt32(myread1["bookstatus"].ToString());
                    }
                  
                    if (status1 == 2 )
                    {
                        string sqlstr2 = "update book set bookstatus=0,marktime =null where bookid = '" + tag.epcid + "'";
                        mysql.Getmysqlcom(sqlstr2);
                    }
                    else if(status1 == 3)
                    {
                        string sqlstr3 = "update book set bookstatus=0 where bookid = '" + tag.epcid + "'";
                        mysql.Getmysqlcom(sqlstr3);

                    }
                    else
                    {
                        continue;
                    }
                }
                
            }

            string sqlstr4 = "select * from book";
            MySqlDataReader myread2 = null;
            myread2 = mysql.Getmysqlread(sqlstr4);
            while(myread2.Read())
            {
                allbooks.Add(myread2["bookid"].ToString());
            }

            foreach(string bookidstr in allbooks)
            {
                if(bookson.Contains(bookidstr))
                {
                    continue;
                }
                else
                {
                    MySqlDataReader myread3 = null;
                    int status2 = 0;
                    string marktimestr = null;
                    string sqlstr5 = "select * from book where bookid = '" + bookidstr + "'";
                    myread3 = mysql.Getmysqlread(sqlstr5);
                    while(myread3.Read())
                    {
                        status2 = Convert.ToInt32(myread3["bookstatus"].ToString());
                        marktimestr = myread3["marktime"].ToString();

                    }
                    if(status2==0)  //将图书的馆藏状态由”在架上“gaiwei”馆内阅读“
                    {
                        string sqlstr6 = "update book set bookstatus=2,marktime= '"+DateTime.Now.ToShortDateString().ToString()+"' where bookid = '" + bookidstr + "'";
                        mysql.Getmysqlcom(sqlstr6);
                    }
                    else if(status2==2)
                    {
                        DateTime marktime = Convert.ToDateTime(marktimestr);
                        DateTime timenow = DateTime.Now;
                        TimeSpan TS = timenow - marktime;
                            int ndays = TS.Days;
                        if(ndays>=30)
                        {
                            string sqlstr7 = "update book set bookstatus=3,marktime= '" + DateTime.Now.ToShortDateString().ToString() + "' where bookid = '" + bookidstr + "'";
                            mysql.Getmysqlcom(sqlstr7);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }


        Thread thread1;
        /// <summary>
        /// 学生/教师/职工/管理员离开（连接192.168.0.102大天线——调用线程、不返回信息）
        /// </summary>
        public void  UserOut()
        {
            string ip = "192.168.0.102";
            mreader.Connect(ip, 1);
            thread1 = new Thread(new ThreadStart(ThreadOut));
            thread1.Start();
            //mreader.DisConnect();
                               
        }
        public void ThreadOut()
        {
            while(true)
            {
                List<TagInfo> tags = mreader.ReadStart(1);

                if(tags!= null)
                {
                    foreach (TagInfo tag in tags)
                    {
                        string idstr = tag.epcid;
                        Mysql mysql = new Mysql();

                        //查询usertag表中与用户id对应的tagno,更改activetag表中有源标签的状态
                        string sqlstr2 = "select * from usertag where enteruserid = " + "'" + idstr + "' and tagstatus =1";
                        MySqlDataReader myread = mysql.Getmysqlread(sqlstr2);
                        if (myread!=null)
                        {
                            while(myread.Read())
                            {
                                string tagnostr = myread["tagno"].ToString();
                                string sqlstr3 = "update activetag set tagstatus = 0 where tagno = " + "'" + tagnostr + "'";
                                mysql.Getmysqlcom(sqlstr3);

                                //更改usertag表中与用户id对应的有源标签的状态
                                string sqlstr1 = "update usertag set tagstatus = 2 where enteruserid = " + "'" + idstr + "'";
                                mysql.Getmysqlcom(sqlstr1);
                            }
                            
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    continue;
                }
              
                Thread.CurrentThread.Join(400);//阻止设定时间
            }
        }
        

        /// <summary>
        /// 学生/教师/职工/管理员离开（连接192.168.0.102大天线——返回ID）
        /// </summary>
        /// <returns></returns>
        public string UseroutMessage()
        {
            string ip = "192.168.0.102";
            mreader.Connect(ip, 1);
            List<TagInfo> tags = mreader.ReadStart(1);
            List<string> idstrs = new List<string>();

            if(tags != null )
            {
                foreach (TagInfo tag in tags)
                {
                    string idstr = tag.epcid;
                    Mysql mysql = new Mysql();

                    //查询usertag表中与用户id对应的tagno,更改activetag表中有源标签的状态
                    string sqlstr2 = "select * from usertag where enteruserid = " + "'" + idstr + "' and tagstatus =1";
                    MySqlDataReader myread = mysql.Getmysqlread(sqlstr2);
                    if(myread.Read())
                    {
                        string tagnostr = myread["tagno"].ToString(); 
                        string sqlstr3 = "update activetag set tagstatus=0 where tagno = " + "'" + tagnostr + "'";
                        mysql.Getmysqlcom(sqlstr3);

                        //更改usertag表中与用户id对应的有源标签的状态
                        string sqlstr1 = "update usertag set tagstatus = 2 where enteruserid = " + "'" + idstr + "'";
                        mysql.Getmysqlcom(sqlstr1);

                        idstrs.Add(idstr);
                    }
                    else
                    {
                        continue;
                    }

                }
            }
            mreader.DisConnect();

            return JsonConvert.SerializeObject(idstrs);
        }

        Thread thread2;
        /// <summary>
        /// 用户进入（连接192.168.0.101大天线——调用线程、无返回值）
        /// </summary>
        public void UserEnter()
        {
            string ip = "192.168.0.101";
            mreader.Connect(ip, 1);
            thread2 = new Thread(new ThreadStart(ThreadEnter));
            thread2.Start();

            //mreader.DisConnect();
        }

        public void ThreadEnter()
        {
            while(true)
            {
                List<TagInfo> tags = mreader.ReadStart(1);

                if (tags != null)
                {
                    foreach (TagInfo tag in tags)
                    {
                        string idstr = tag.epcid;

                        MySqlDataReader myread1 = null;
                        MySqlDataReader myread2 = null;
                        MySqlDataReader myread3 = null;
                        Mysql mysql = new Mysql();

                        if (idstr.StartsWith("U") | idstr.StartsWith("M") | idstr.StartsWith("D") | idstr.StartsWith("T") | idstr.StartsWith("S"))
                        {
                            string sqlstr = "select * from user where userid = " + "'" + idstr + "'";
                            myread1 = mysql.Getmysqlread(sqlstr);
                            if (myread1.Read())
                            {
                                string sqlstr1 = "select * from usertag where enteruserid = " + "'" + idstr + "' and tagstatus =1"; //判断数据库中是否有该用户正在馆内的记录
                                myread2 = mysql.Getmysqlread(sqlstr1);
                                if (!myread2.Read())
                                {
                                    string sqlstr2 = "select * from activetag where tagstatus = 0";
                                    myread3 = mysql.Getmysqlread(sqlstr2);
                                    myread3.Read();
                                    string tagnostr = myread3["tagno"].ToString();
                                    string timestr = DateTime.Now.ToString();

                                    string sqlstr3 = "update activetag set tagstatus = 1 where tagno = " + "'" + tagnostr + "'";
                                    mysql.Getmysqlcom(sqlstr3);
                                    string sqlstr4 = "insert into usertag (enteruserid,tagno,entertime,tagstatus) value " + "('" + idstr + "','" + tagnostr + "','" + timestr + "',1)";
                                    mysql.Getmysqlcom(sqlstr4);
                                }

                            }
                        }
                        else if (idstr.StartsWith("A"))
                        {
                            string sqlstr = "select * from libadmin where adminid = " + "'" + idstr + "'";
                            myread1 = mysql.Getmysqlread(sqlstr);
                            if (myread1.Read())
                            {
                                string sqlstr1 = "select * from usertag where enteruserid = " + "'" + idstr + "' and tagstatus =1";
                                myread2 = mysql.Getmysqlread(sqlstr1);
                                if (!myread2.Read())
                                {
                                    string sqlstr2 = "select * from activetag where tagstatus = 0";
                                    myread3 = mysql.Getmysqlread(sqlstr2);
                                    myread3.Read();
                                    string tagnostr = myread3["tagno"].ToString();
                                    string timestr = DateTime.Now.ToString();

                                    string sqlstr3 = "update activetag set tagstatus = 1 where tagno = " + "'" + tagnostr + "'";
                                    mysql.Getmysqlcom(sqlstr3);
                                    string sqlstr4 = "insert into usertag (enteruserid,tagno,entertime,tagstatus) value " + "('" + idstr + "','" + tagnostr + "','" + timestr + "',1)";
                                    mysql.Getmysqlcom(sqlstr4);
                                }
                            }
                        }
                        else
                        {
                            continue;
                        }

                    }
                }
                else
                {
                    continue;
                }
                Thread.CurrentThread.Join(400);//阻止设定时间
            }
        }
    

        
        /// <summary>
        /// 查询图书信息（根据关键词及查询语句）
        /// </summary>
        /// <returns></returns>
        public string QueryBook1(int querynum,string querystr)
        {
            List<Book> querybooks = new List<Book>();
            Mysql mysql = new Mysql();
            MySqlDataReader myread = null;
            if(querynum == 1)   //索书号
            {
                string sqlstr = "select * from book where callnumber like '%" + querystr + "%'";
                myread = mysql.Getmysqlread(sqlstr);
                while(myread.Read())
                {
                    Book book = new Book(myread["callnumber"].ToString(), myread["bookname"].ToString(), myread["author"].ToString(), myread["publisher"].ToString());
                    if(querybooks.Contains(book))
                    {
                        continue;
                    }
                    else
                    {
                        querybooks.Add(book);
                    }
                    
                }

            }
            else if(querynum ==2)   //书名
            {
                string sqlstr = "select * from book where bookname like '%" + querystr + "%'";
                myread = mysql.Getmysqlread(sqlstr);
                while (myread.Read())
                {
                    Book book = new Book(myread["callnumber"].ToString(), myread["bookname"].ToString(), myread["author"].ToString(), myread["publisher"].ToString());
                    if (querybooks.Contains(book))
                    {
                        continue;
                    }
                    else
                    {
                        querybooks.Add(book);
                    }
                }
            }
            else  //作者
            {
                string sqlstr = "select * from book where author like '%" + querystr + "%'";
                myread = mysql.Getmysqlread(sqlstr);
                while (myread.Read())
                {
                    Book book = new Book(myread["callnumber"].ToString(), myread["bookname"].ToString(), myread["author"].ToString(), myread["publisher"].ToString());
                    if (querybooks.Contains(book))
                    {
                        continue;
                    }
                    else
                    {
                        querybooks.Add(book);
                    }
                }
            }

            return JsonConvert.SerializeObject(querybooks);
        }
        
        
        /// <summary>
        /// 图书查询（根据图书索书号具体查询多本索书号相同的图书详细信息）
        /// </summary>
        /// <param name="numberstr"></param>
        /// <returns></returns>
        public string QueryBook2(string numberstr)
        {
            List<Book> books = new List<Book>();
            Mysql mysql = new Mysql();
            MySqlDataReader myread = null;
            string sqlstr = "select * from book where callnumber = '" + numberstr + "'";
            myread = mysql.Getmysqlread(sqlstr);
            while(myread.Read())
            {
                Book book = new Book(myread["isbn"].ToString(),myread["callnumber"].ToString(), myread["bookname"].ToString(), myread["author"].ToString(), myread["publisher"].ToString(), myread["position"].ToString(),(int)myread["bookstatus"]);
                books.Add(book);
            }
            return JsonConvert.SerializeObject(books);
        }
    }

    /// <summary>
    /// 桌面式发卡器的相关功能操作
    /// </summary>
    public class DesktopReadManage
    {
        DesktopRead dreader = new DesktopRead();

        /// <summary>
        /// 学生/教师/职工/管理员进入（连接桌面式发卡器——返回布尔值）             
        /// </summary>
        /// <returns></returns>
        public bool Userenter()
        {
            bool flag = false;
            string idstr = null;
            MySqlDataReader myread1 = null;
            MySqlDataReader myread2 = null;
            MySqlDataReader myread3 = null;

            dreader.Connect();

            idstr = dreader.ReadEPC();

            if(idstr.Length>0)
            {
                Mysql mysql = new Mysql();

                if (idstr.StartsWith("U") | idstr.StartsWith("M") | idstr.StartsWith("D") | idstr.StartsWith("T") | idstr.StartsWith("S"))
                {
                    string sqlstr = "select * from user where userid = " + "'" + idstr + "'";
                    myread1 = mysql.Getmysqlread(sqlstr);
                    if (myread1.Read())
                    {
                        string sqlstr1 = "select * from usertag where enteruserid = " + "'" + idstr + "' tagstatus = 1";
                        myread2 = mysql.Getmysqlread(sqlstr1);
                        if(!myread2.Read())
                        {
                            flag = true;

                            string sqlstr2 = "select * from activetag where tagstatus = 0";
                            myread3 = mysql.Getmysqlread(sqlstr2);
                            while (myread3.Read())
                            {
                                string tagnostr = myread3["tagno"].ToString();
                                string timestr = DateTime.Now.ToString();

                                string sqlstr3 = "update activetag set tagstatus = 1 where tagno = " + "'" + tagnostr + "'";
                                mysql.Getmysqlcom(sqlstr3);
                                string sqlstr4 = "insert into usertag (enteruserid,tagno,entertime,tagstatus) value " + "('" + idstr + "','" + tagnostr + "','" + timestr + "',1)";
                                mysql.Getmysqlcom(sqlstr4);
                            }
                        }              
                    }
                }
                else if (idstr.StartsWith("A"))
                {
                    string sqlstr = "select * from libadmin where adminid = " + "'" + idstr + "'";
                    myread1 = mysql.Getmysqlread(sqlstr);
                    if (myread1.Read())
                    {
                        string sqlstr1 = "select * from usertag where enteruserid = " + "'" + idstr + "' tagstatus = 1";
                        myread2 = mysql.Getmysqlread(sqlstr1);
                        if (!myread2.Read())
                        {
                            flag = true;

                            string sqlstr2 = "select * from activetag where tagstatus = 0";
                            myread3 = mysql.Getmysqlread(sqlstr2);
                            while (myread3.Read())
                            {
                                string tagnostr = myread3["tagno"].ToString();
                                string timestr = DateTime.Now.ToString();

                                string sqlstr3 = "update activetag set tagstatus = 1 where tagno = " + "'" + tagnostr + "'";
                                mysql.Getmysqlcom(sqlstr3);
                                string sqlstr4 = "insert into usertag (enteruserid,tagno,entertime,tagstatus) value " + "('" + idstr + "','" + tagnostr + "','" + timestr + "',1)";
                                mysql.Getmysqlcom(sqlstr4);

                            }
                        }           
                    }
                }            
            }
            else
            {
                flag = false;
            }
            dreader.DisConnect();

            return flag;
            
        }

        /// <summary>
        ///  学生/教师/职工/管理员进入（连接桌面式发卡器——返回ID）
        /// </summary>
        /// <returns></returns>
        public string UserenterMessage()
        {
            string ids = null;
            string idstr = null;
            MySqlDataReader myread1 = null;
            MySqlDataReader myread2 = null;
            MySqlDataReader myread3 = null;

            dreader.Connect();

            idstr = dreader.ReadEPC();

            if(idstr.Length>0)
            {
                Mysql mysql = new Mysql();

                if (idstr.StartsWith("U") | idstr.StartsWith("M") | idstr.StartsWith("D") | idstr.StartsWith("T") | idstr.StartsWith("S"))
                {
                    string sqlstr = "select * from user where userid = " + "'" + idstr + "'";
                    myread1 = mysql.Getmysqlread(sqlstr);
                    if (myread1.Read())
                    {
                        string sqlstr1 = "select * from usertag where enteruserid = " + "'" + idstr + "' tagstatus = 1";
                        myread2 = mysql.Getmysqlread(sqlstr1);
                        if (!myread2.Read())
                        {
                            ids = idstr;

                            string sqlstr2 = "select * from activetag where tagstatus = 0";
                            myread3 = mysql.Getmysqlread(sqlstr2);

                            while (myread3.Read())
                            {
                                string tagnostr = myread3["tagno"].ToString();
                                string timestr = DateTime.Now.ToString();

                                string sqlstr3 = "update activetag set tagstatus = 1 where tagno = " + "'" + tagnostr + "'";
                                mysql.Getmysqlcom(sqlstr3);
                                string sqlstr4 = "insert into usertag (enteruserid,tagno,entertime,tagstatus) value " + "('" + idstr + "','" + tagnostr + "','" + timestr + "',1)";
                                mysql.Getmysqlcom(sqlstr4);
                            }
                        }         
                        
                    }
                }
                else if (idstr.StartsWith("A"))
                {
                    string sqlstr = "select * from libadmin where adminid = " + "'" + idstr + "'";
                    myread1 = mysql.Getmysqlread(sqlstr);
                    if (myread1.Read())
                    {
                        string sqlstr1 = "select * from usertag where enteruserid = " + "'" + idstr + "' tagstatus = 1";
                        myread2 = mysql.Getmysqlread(sqlstr1);
                        if (!myread2.Read())
                        {
                            ids = idstr;

                            string sqlstr2 = "select * from activetag where tagstatus = 0";
                            myread3 = mysql.Getmysqlread(sqlstr2);
                            while (myread3.Read())
                            {
                                string tagnostr = myread3["tagno"].ToString();
                                string timestr = DateTime.Now.ToString();

                                string sqlstr3 = "update activetag set tagstatus = 1 where tagno = " + "'" + tagnostr + "'";
                                mysql.Getmysqlcom(sqlstr3);
                                string sqlstr4 = "insert into usertag (enteruserid,tagno,entertime,tagstatus) value " + "('" + idstr + "','" + tagnostr + "','" + timestr + "',1)";
                                mysql.Getmysqlcom(sqlstr4);
                            }
                        }          
                    }
                }         
            }
            else
            {
                ids = null;
            }

            dreader.DisConnect();
            return ids;
        }

       
        /// <summary>
        /// 借书时获取用户id、name（是学生、教师才有返回值，否则为空(未放校园卡或借阅者不是学生或老师)）
        /// </summary>
        /// <returns></returns>
        public string GetUserid()
        {
            string idstr = null;
            MySqlDataReader myread = null;
            User user = new User();       

            dreader.Connect();
            idstr = dreader.ReadEPC();
            if(idstr.Length>0)
            {
                if(idstr.StartsWith("U")|idstr.StartsWith("M")|idstr.StartsWith("D")|idstr.StartsWith("T"))
                {
                    Mysql mysql = new Mysql();
                    string sqlstr = "select * from user where userid = " + "'" + idstr + "'";
                    myread = mysql.Getmysqlread(sqlstr);
                    if(myread.Read())
                    {
                        user = new User(myread["userid"].ToString(), myread["username"].ToString());
                    }
                    else
                    {
                        user = null;
                    }
                }
                else
                {
                    user = null;
                }
                
            }
            else
            {
                user = null;
            }

            dreader.DisConnect();
 
            return JsonConvert.SerializeObject(user);

            //return JsonConvert.SerializeObject(idstr);

        }

            
        /// <summary>
        /// 借书时，获取图书信息
        /// </summary>
        /// <returns></returns>
        public string BorrowBook(string userid)
        {
            string flag = null;
            
            dreader.Connect();

            Book book = new Book();

            string bookidstr = dreader.ReadEPC();   //获取图书id


            if (bookidstr.Length ==0)
            {
                book = new Book(null, null, 0); // 0：借阅失败，请重新扫描（未将需借阅的图书放到借阅设备上）
                flag = JsonConvert.SerializeObject(book);
            }
            else
            {
                Mysql mysql = new Mysql();

                MySqlDataReader myread3 = null;
                string sqlstr = "select * from book where bookid = '"+bookidstr+"'";
                myread3 = mysql.Getmysqlread(sqlstr);
                int status = 0;
                while(myread3.Read())
                {
                    status = Convert.ToInt32(myread3["bookstatus"].ToString());
                }
                
                if(status ==1)
                {
                    book = new Book(null, null, 1); // 1：借阅失败，该书已经被借阅
                    flag = JsonConvert.SerializeObject(book);
                }
                else
                {
                    MySqlDataReader myread1 = null;
                    MySqlDataReader myread2 = null;


                    string sqlstr1 = "select countlimit from user where userid = '" + userid + "'";
                    myread1 = mysql.Getmysqlread(sqlstr1);
                    while (myread1.Read())
                    {
                        int count = Convert.ToInt32(myread1["countlimit"].ToString());
                        if (count > 0)
                        {

                            string sqlstr2 = "update user set countlimit=countlimit-1 where userid = '" + userid + "'";
                            mysql.Getmysqlcom(sqlstr2);
                            string sqlstr3 = "update book set bookstatus = 1 where bookid = " + "'" + bookidstr + "'";
                            mysql.Getmysqlcom(sqlstr3);
                            string sqlstr4 = "insert bookorder (userid,bookid,borrowdate,outdate,orderstatus) value " + "('" + userid + "','" + bookidstr + "','" + DateTime.Now.ToShortDateString().ToString() + "','" + DateTime.Now.Date.AddDays(30).ToShortDateString().ToString() + "',0)";
                            mysql.Getmysqlcom(sqlstr4);
                            string sqlstr5 = "select * from book where bookid = " + "'" + bookidstr + "'";
                            myread2 = mysql.Getmysqlread(sqlstr5);
                            while (myread2.Read())
                            {
                                book = new Book(myread2["callnumber"].ToString(), myread2["bookname"].ToString(), myread2["author"].ToString(), myread2["publisher"].ToString(),2);  //2:借阅成功
                                flag = JsonConvert.SerializeObject(book);
                            }
                        }
                        else
                        {
                            book = new Book(null, null, 3); // 3：借阅失败，已到借阅上限；

                            flag = JsonConvert.SerializeObject(book);
                        }
                    }
                } 
            }

            dreader.DisConnect();
            return flag;

        }
        

      
        /// <summary>
        /// 归还图书
        /// </summary>
        public int ReturnBook()
        {
            int flag = 0;

            dreader.Connect();

            string bookidstr = dreader.ReadEPC();

            if(bookidstr.Length > 0 )
            {
                MySqlDataReader myread = null;
                MySqlDataReader myread1 = null;
                Mysql mysql = new Mysql();
                //查询该图书是否已被归还
                string sqlstr = "select * from book where bookid = '" + bookidstr + "'";
                myread = mysql.Getmysqlread(sqlstr);
                int status = 1;
                while(myread.Read())
                {
                    status = Convert.ToInt32(myread["bookstatus"].ToString());
                }
                if(status!=1)
                {
                    flag = 1;   //1：该图书已被归还
                }
                else
                {
                    //修改book表中的图书馆藏状态bookstatus
                    string sqlstr1 = "update book set bookstatus = 2 where bookid = " + "'" + bookidstr + "'";
                    mysql.Getmysqlcom(sqlstr1);
                    //修改bookorder表中的借阅状态orderstatus
                    string sqlstr2 = "update bookorder set orderstatus = 2,returndate =" + "'" + DateTime.Now.ToShortDateString().ToString() + "'" + "where bookid = " + "'" + bookidstr + "'";
                    mysql.Getmysqlcom(sqlstr2);
                    string sqlstr3 = "select * from bookorder where bookid = '" + bookidstr + "'";
                    myread1 = mysql.Getmysqlread(sqlstr3);
                    while (myread1.Read())
                    {
                        string useridstr = myread1["userid"].ToString();
                        string sqlstr4 = "update user set countlimit=countlimit+1 where userid = '" + useridstr + "'";
                        mysql.Getmysqlcom(sqlstr4);
                        flag = 2;   //2：归还成功
                    }
                }

               
            }
            else
            {                
                flag = 0;  //0：扫描失败，请重新扫描
            }

            dreader.DisConnect();
            return flag;
        }


        /// <summary>
        /// 新书入库时，向图书的无源标签中写入条形码
        /// </summary>
        /// <param name="idstr"></param>
        /// <param name="number"></param>
        /// <param name="namestr"></param>
        /// <param name="authorstr"></param>
        /// <param name="publisherstr"></param>
        /// <param name="positionstr"></param>
        /// <returns></returns>
        public bool NewBook(string idstr)
        {
            bool flag;
            if (dreader.Connect())
            {
                flag = dreader.WriteEPC(idstr);
            }
            else
            {
                flag = false;
            }
            dreader.DisConnect();
            return flag;
        }

        /// <summary>
        /// 坏书清库（直接读取无源标签中书的编号进行清库操作）
        /// </summary>
        /// <returns></returns>
        public bool DeleteBook()
        {
            bool flag;
            string bookidstr = null;
            bookidstr = dreader.ReadEPC();

            if(bookidstr.Length>0)
            {
                Mysql mysql = new Mysql();
                string sqlstr = "delete from book where bookid = " + "'" + bookidstr + "'";

                mysql.Getmysqlcom(sqlstr);
                flag = true;
            }
            else
            {
                flag = false;
            }
                   
            

            dreader.DisConnect();
            return flag;
        }

        /// <summary>
        /// 修改图书时，首先获取当前图书信息
        /// </summary>
        /// <returns></returns>
        public string ReadBook()
        {
            dreader.Connect();
            string bookidstr = dreader.ReadEPC();
            Mysql mysql = new Mysql();
            string sqlstr = "select * from book where bookid = " + "'" + bookidstr + "'";
            MySqlDataReader myread = mysql.Getmysqlread(sqlstr);
            myread.Read();
            Book book = new Book(myread["bookid"].ToString(), myread["callnumber"].ToString(), myread["bookname"].ToString(), myread["author"].ToString(), myread["publisher"].ToString(), myread["position"].ToString());
            
            dreader.DisConnect();
            
            return JsonConvert.SerializeObject(book);

        }
        /// <summary>
        /// 接着修改图书信息、修改数据库
        /// </summary>
        /// <returns></returns>
        public bool EditBook(string bookidstr, string number, string namestr, string authorstr, string publisherstr, string positionstr)
        {
            Mysql mysql = new Mysql();
            string sqlstr = "update book set callnumber = '" + number + "',bookname = '" + namestr + "',author = '" + authorstr + "',publisher = '" + publisherstr + "',position='" + positionstr + "' where bookid = '"+bookidstr+"'";
            bool flag = mysql.Getmysqlcom(sqlstr);

            return flag;
        }

    }

    /// <summary>
    /// 统计记录
    /// </summary>
    public class Statistics
    {
        /// <summary>
        /// 统计在馆人数
        /// </summary>
        /// <returns></returns>
        public int NuminLibrary()
        {
            int count = 0;
            MySqlDataReader myread = null;
            Mysql mysql = new Mysql();
            string sqlstr = "select * from usertag where tagstatus = 1";
            myread = mysql.Getmysqlread(sqlstr);
            if(myread != null)
            {
                while (myread.Read())
                {
                    count++;
                }
            }
            else
            {
                count = 0;
            }
            return count;
        }
    }
 

    

}
