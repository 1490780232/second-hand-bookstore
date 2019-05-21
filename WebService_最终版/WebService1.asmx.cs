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
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod(Description = "返回字符串")]
        public string HelloWorld2()
        {
            return "知识不一定用得上，能力却终身受益！";
        }
        [WebMethod(Description = "带参数的字符串函数")]
        public string Show(string name)
        {
            if (name == null || name == "")
            {
                return "互联网是个小妖精！";
            } else {
                return name + "是个小妖精！";
            }
        }
        [WebMethod(Description = "相加")]
        public double Add(double num1, double num2)
        {
            return num1 + num2;
        }
        [WebMethod(Description = "相减")]
        public double Sub(double num1, double num2)
        {
            return num1 - num2;
        }
        [WebMethod(Description = "相乘")]
        public double Mul(double num1, double num2)
        {
            return num1* num2;
        }
        [WebMethod(Description = "相除")]
        public double Div(double num1, double num2)
        {
            if (num2 != 0)
                return num1 / num2;
            else
                return 0;
        }
        [WebMethod(Description = "返回数据集合")]
        public DataSet GetDataSet(string command)
        {
            SqlConnection sqlCon1 = new SqlConnection(@"Database = Students; Server = LENOVO-PC; Integrated Security = SSPI");
            DataSet ds = new DataSet();
            using (SqlDataAdapter dapt = new SqlDataAdapter(command, sqlCon1))
            {
                dapt.Fill(ds, "Books");
            }
            return ds;
        }
        [WebMethod(Description = "登录")]
        public DataSet Login(string name,string pwd)
        {
            SqlConnection sqlCon1 = new SqlConnection(@"Database = Students; Server = LENOVO-PC; Integrated Security = SSPI");
            //SqlConnection sqlCon1 = new SqlConnection(@"Data Source =.\SQLEXPRESS; Initial
  //Catalog = Students; User ID = sa; Password = 123");
            DataSet ds = new DataSet();
            var command = "select Name,Sex,Age,Discipline from Students where Name='"+@name+"' and Pwd='"+@pwd+"'";
            using (SqlDataAdapter dapt = new SqlDataAdapter(command, sqlCon1))
            {
                dapt.Fill(ds, "Books");
            }
            return ds;
        }
    }
}
