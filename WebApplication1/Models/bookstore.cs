using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity;


namespace WebApplication1.Models
{
    //<summary>
    // 定义一个实体类Students
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string bookID { get; set; }
        public string bookName { get; set; }
        public string bookIBSN { get; set; }
        public string author { get; set; }
        public int oriPrice { get; set; }

        public string press { get; set; }//出版社
        public int currPrice { get; set; }   
    }



    public class Book_Statu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string bookID { get; set; }
        public string BookcaseID { get; set; }
        public int bookStatus { get; set; }
        public int checkStatus { get; set; }

        public string RFID { get; set; }
        public string sellerID { get; set; }
        public DateTime sTime { get; set; }
    }


    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string userID  { get; set; }
        public string userName { get; set; }
        public string AlipayAccount { get; set; }
        public string contactInfo { get; set; }
        public string mail { get; set; }
        
    }

    public class check_book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string bookID { get; set; }
        public int checkStatus { get; set; }
        public string faileReason { get; set; }
        public DateTime checkTime { get; set; }
        public int adminID { get; set; }
    }

    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string orderID { get; set; }
        public string bookID { get; set; }
        public string userID { get; set; }
        public DateTime orderTime { get; set; }
        public int rderPrice { get; set; }
        public int orderStatus { get; set; }
    }

    public class bookcase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string bookcaseID { get; set; }
        public string categorye { get; set; }
        public string RFID { get; set; }

    }



    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string adminID { get; set; }
        public string adminName { get; set; }
        public string phoneNumt { get; set; }
        public string mail { get; set; }}

    }

    //public class bookDbContext : DbContext
    //{

    //    public bookDbContext() : base("data source=.;initial catalog=bookstore;Integrated Security=SSPI;") //构造函数，指定数据库名称的约定连接
    //    {
    //        //Code first会在第一次ef查询的时候会对__MigrationHistory访问，是为了检查数据库和model是否匹配，以保证ef能正常运行
    //        Database.SetInitializer<bookDbContext>(null);
    //    } 

    //    //public BookDbContext() : base("Data Source=.;Initial Catalog=Students;Integrated Security=SSPI;") { } 

    //    //DbSet是一个模版类，<>中代表的是模版类中的对象类型
    //    public DbSet<Book> book_info { get; set; }
    ////public DbSet<Students> Students { get; set; }

    //    public DbSet<Users> Users { get; set; }//将数据集实体类Books_info与Books_info表关联
    //    public DbSet<Order> Order { get; set; }//将数据集实体类Borrow_Return与Borrow_Return表关联
    //    public DbSet<check_book> check_book { get; set; }//将数据集实体类 Readers与 Readers表关联
    //    public DbSet<bookcase> bookcase { get; set; }
    //    public DbSet<Admin> Admin { get; set; }
    //    public DbSet<Book_Statu> book_statu { get; set; }
    //    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //    {
    //        //取消复数表名惯例
    //        modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
    //    }
    //}
