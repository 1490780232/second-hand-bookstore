using ReaderManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebService1
{
    /// <summary>
    /// WebService1 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        ModuleReadManage mread = new ModuleReadManage();
        DesktopReadManage dread = new DesktopReadManage();
        Statistics statistic = new Statistics();

        [WebMethod(Description = "断开与固定式读写器的连接")]
        public void DisConnect()
        {
            mread.MouduledisConnect();
            HttpRequest Request = HttpContext.Current.Request;
            string callback = Request["callback"];
            HttpResponse Response = HttpContext.Current.Response;

            Response.Write(callback + "(true)");
            Response.End();
        }

        [WebMethod(Description = "盘存(连接192.168.0.103四根小天线——返回条形码、书名、当前位置、正确位置)")]
        public void  BookInventory()
        {
            HttpRequest Request = HttpContext.Current.Request;
            string callback = Request["callback"];
            HttpResponse Response = HttpContext.Current.Response;
            string query2 = mread.Inventory();
            Response.Write(callback + "(" + query2 + ")");
            Response.End();
           

        }
        [WebMethod(Description = "定时盘点图书，修改图书馆藏状态")]
        public void TimedBookInventory()
        {
            mread.TimedInventory();
        }
        [WebMethod(Description = "用户进入（连接192.168.0.101大天线——使用线程、无返回值）")]
        public void Enter()
        {
            mread.UserEnter();
            
            HttpRequest Request = HttpContext.Current.Request;
            string callback = Request["callback"];
            HttpResponse Response = HttpContext.Current.Response;
            
            Response.Write(callback + "(true)");
            Response.End();
        }
        [WebMethod(Description = "用户进入（连接桌面式发卡器——返回布尔值）")]
        public void Enterbool()
        {
            HttpRequest Request = HttpContext.Current.Request;
            string callback = Request["callback"];
            HttpResponse Response = HttpContext.Current.Response;
            string penter = dread.Userenter().ToString();
            Response.Write(callback + "("+penter+")");
            Response.End();
            
        }
        [WebMethod(Description = "用户进入（连接桌面式发卡器——返回ID）")]
        public string EnterMessage()
        {
            return dread.UserenterMessage();
        }
        [WebMethod(Description = "用户离开(连接192.168.0.102大天线——使用线程、无返回值)")]
        public void Leave()
        {
            mread.UserOut();
            HttpRequest Request = HttpContext.Current.Request;
            string callback = Request["callback"];
            HttpResponse Response = HttpContext.Current.Response;
            //string pleave = "true";
            Response.Write(callback + "(true)");
            Response.End();
            
        }
         [WebMethod(Description = "用户离开(连接192.168.0.102大天线——返回ID)")]
        public string LeaveMessage()
        {
            return mread.UseroutMessage();
        }
        [WebMethod(Description = "图书查询（根据关键词及查询语句）")]
        public string Query(int keynum,string searchstr)
        {
            return mread.QueryBook1(keynum,searchstr);
        }
        [WebMethod(Description = "图书查询（根据图书索书号具体查询多本索书号相同的图书详细信息）")]
        public string Querybook(string numberstr)
        {
            return mread.QueryBook2(numberstr);
        }
        /*[WebMethod(Description = "图书借阅-获取用户信息（是学生、教师才有返回值，否则为空(未放校园卡或借阅者不是'学生或老师')）")]
        public string Getuserid()
        {
            return dread.GetUserid();
        }
        [WebMethod(Description = "图书借阅-获取图书信息")]
        public string Borrow(string useridstr)
        {
            return dread.BorrowBook(useridstr);
        }*/
        [WebMethod(Description = "图书借阅-获取用户信息（是学生、教师才有返回值，否则为空）")]
        public void Getuserid()
        {
            HttpRequest Request = HttpContext.Current.Request;
            string callback = Request["callback"];
            HttpResponse Response = HttpContext.Current.Response;
            string query3 = dread.GetUserid();
            Response.Write(callback + "(" + query3 + ")");
            Response.End();
        }
        /*public string Getuserid()
        {
            return dread.GetUserid();
        }*/
        [WebMethod(Description = "图书借阅-获取图书信息")]
        public void Borrow(string useridstr)
        {
            HttpRequest Request = HttpContext.Current.Request;
            string callback = Request["callback"];
            HttpResponse Response = HttpContext.Current.Response;
            string borrowb = dread.BorrowBook(useridstr);
            //string borrowb = useridstr;
            Response.Write(callback + "(" + borrowb + ")");
            //Response.Write(callback +"("+borrowb+")");
            Response.End();
        }
        [WebMethod(Description = "图书归还（返回布尔值）")]
        public void Return()
        {
            HttpRequest Request = HttpContext.Current.Request;
            string callback = Request["callback"];
            HttpResponse Response = HttpContext.Current.Response;
            string returnb = "";
            if(dread.ReturnBook()==1){
                returnb = "1";
            }
            else if (dread.ReturnBook() == 0) {
                returnb="0";
            }
            else
            {
                returnb = "2";
            }
            //string borrowb = useridstr;
            Response.Write(callback + "(" + returnb + ")");
            //Response.Write(callback +"("+borrowb+")");
            Response.End();
           }
        [WebMethod(Description = "新书入库")]
        public void Warehousing(string idstr)
        {
            dread.NewBook(idstr);
            HttpRequest Request = HttpContext.Current.Request;
            string callback = Request["callback"];
            HttpResponse Response = HttpContext.Current.Response;

            Response.Write(callback + "(true)");
            Response.End();
        }
        [WebMethod(Description = "坏书清库（直接读取无源标签中书的编号进行清库操作）")]
        public void Delete()
        {
            HttpRequest Request = HttpContext.Current.Request;
            string callback = Request["callback"];
            HttpResponse Response = HttpContext.Current.Response;
            string deleteb = "";
            if (dread.DeleteBook() == true)
            {
                deleteb = "1";
            }
            else
            {
                deleteb = "0";
            }
            //string borrowb = useridstr;
            Response.Write(callback + "(" + deleteb + ")");
            //Response.Write(callback +"("+borrowb+")");
            Response.End();
            
        }
        [WebMethod(Description = "统计当前在馆人数")]
        public void NumberIn()
        {
            HttpRequest Request = HttpContext.Current.Request;
            string callback = Request["callback"];
            HttpResponse Response = HttpContext.Current.Response;
            string numberin = statistic.NuminLibrary().ToString();
            //string borrowb = useridstr;
            Response.Write(callback + "(" + numberin + ")");
            //Response.Write(callback +"("+borrowb+")");
            Response.End();
            //return statistic.NuminLibrary();
        }
        /*
        [WebMethod(Description = "图书修改-获得需修改图书的信息")]
        public string EditOne()
        {
            return dread.ReadBook();
        }
        [WebMethod(Description = "图书修改-修改图书信息、修改数据库")]
        public bool EditTwo(string bookidstr, string number, string namestr, string authorstr, string publisherstr, string positionstr)
        {
            return dread.EditBook(bookidstr, number, namestr, authorstr, publisherstr, positionstr);
        }
        [WebMethod(Description = "连接192.168.0.102大天线")]
        public bool MouduleConnect()
        {
            return mread.MouduleConnect2();
        }
         */
    }
}
