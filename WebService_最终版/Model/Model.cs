using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity;

namespace Model
{
    //用户(X学生、教师、职工)
    public class User
    {
        public string  userid { get; set; }     //学号
        public string username { get; set; }        //姓名
        public string sex { get; set; }     //性别
        public int  usertype { get; set; }     //类型：'0：学生；1：教师；2：职工'  
        public User(string idstr, string namestr)
        {
            userid = idstr;
            username = namestr;
        }
    }

    //管理员
    public class LibAdmin
    {
        public string adminid { get; set; }
        public string adminname { get; set; }
        public string password { get; set; }

    }

    //有源标签
    public class ActiveTag
    {
        public string tagno { get; set; }
        public int tagstatus { get; set; }      //0:未使用；1：使用中
    }

    //无源标签
    public class PassiveTag
    {
        public string tagid { get; set; }
    }

    //用户-有源标签
    public class UserTag
    {
        public string userid { get; set; }
        public string tagno { get; set; }
        public string time { get; set; }
        public int tagstatus { get; set; }   //1：在用；2：归还
    }

    public class SeatOrder
    {
        public string orderid { get; set; }
        public string userid { get; set; }
        public string seatid { get; set; }
        public string ordertime { get; set; }
        public int locstatus { get; set; }   //座位状态：0：离开；1：预约；2：临时离开；3：使用中

    }

    //图书
    public class Book
    {
        public string bookid { get; set; }      //条形码
        public string callnumber { get; set; }  //索书号
        public string bookname { get; set; }        //题名
        public string author { get; set; }       //作者
        public string publisher { get; set; }   //出版社
        public string  position { get; set; }       //馆藏位置，等同于Bookshelf中的Name
        public int bookstatus { get; set; }     //馆藏状态：0:在架上；1：被借阅
        public Book(string number,string namestr,int statusstr)
        {
            callnumber = number;
            bookname = namestr;
            bookstatus = statusstr;
        }
        public Book(string idstr, string number, string namestr, string authorstr, string publisherstr,string positionstr,int statusstr)
        {
            bookid = idstr;
            callnumber = number;
            bookname = namestr;
            author = authorstr;
            publisher = publisherstr;
            position = positionstr;
            bookstatus = statusstr;
        }
    }

    public class Bookshelf
    {
        public string shelfid { get; set; }      //书架编号,和天线id相同
        public string shelfname { get; set; }    //书架名，即位置
     
    }

    public class BookOrder
    {
        public string orderid { get; set; }
        public string userid { get; set; }
        public string bookid { get; set; }
        public string borrowdate { get; set; }  //借阅时间
        public string returndate { get; set; }  //归还时间
        public string outdate { get; set; }     //超期时间(比如用于续借)
        public int orderstatus { get; set; }     //0:借阅中；1：超期；2：已归还
    }

    public class Mysql
    {
        /// <summary>
        /// 建立MySql数据库连接
        /// </summary>
        /// <returns></returns>
        public MySqlConnection Getmysqlcon()
        {
            string myconnstr = "Host=localhost;Database=library;Username='root';Password=''";
            //建立连接
            MySqlConnection myconn = new MySqlConnection(myconnstr);

            return myconn;
        }

        /// <summary>
        /// 执行MySqlCommand命令
        /// </summary>
        /// <param name="sqlstr"></param>
        public void Getmysqlcom(string sqlstr)
        {
            //建立连接
            MySqlConnection myconn = this.Getmysqlcon();
            myconn.Open();
            MySqlCommand mycom = new MySqlCommand(sqlstr, myconn);
            mycom.ExecuteNonQuery();
            mycom.Dispose();
            myconn.Close();
            myconn.Dispose();
        }

        /// <summary>
        /// 根据sqlstr查询数据
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public MySqlDataReader Getmysqlread(string sqlstr)
        {
            MySqlCommand mycom = null;
            MySqlDataReader myread = null;
            //建立连接
            MySqlConnection myconn = this.Getmysqlcon();
            //设置查询命令
            mycom = new MySqlCommand(sqlstr, myconn);

            try
            {
                //打开连接
                myconn.Open();
                //执行查询，并将结果返回读取器
                myread = mycom.ExecuteReader(CommandBehavior.CloseConnection);              
            }
            catch(Exception ex) { }

            return myread;         
        }      

    }

    /*
    //强类型连接 
    public class DbContext : System.Data.Entity.DbContext
    {

        public DbContext() : base(@"Database=library;Server= ;Integrated Security=SSPI") { }
        public DbSet<Students> student { get; set; }  //将数据集实体类与表关联
    }*/

}
