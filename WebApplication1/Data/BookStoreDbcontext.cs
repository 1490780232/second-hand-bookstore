using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using System.Data.Entity;

namespace WebApplication1.Data
{
    public class BookStoreDbContext : DbContext
    {
        public BookStoreDbContext() : base("data source=.;initial catalog=bookstore;Integrated Security=SSPI;") //构造函数，指定数据库名称的约定连接
        {
            //Code first会在第一次ef查询的时候会对__MigrationHistory访问，是为了检查数据库和model是否匹配，以保证ef能正常运行
            Database.SetInitializer<BookStoreDbContext>(null);
        }

        //public BookDbContext() : base("Data Source=.;Initial Catalog=Students;Integrated Security=SSPI;") { } 

        //DbSet是一个模版类，<>中代表的是模版类中的对象类型
        public DbSet<Book> book_info { get; set; }
        //public DbSet<Students> Students { get; set; }

        public DbSet<Users> Users { get; set; }//将数据集实体类Books_info与Books_info表关联
        public DbSet<Order> Order { get; set; }//将数据集实体类Borrow_Return与Borrow_Return表关联
        public DbSet<check_book> check_book { get; set; }//将数据集实体类 Readers与 Readers表关联
        public DbSet<bookcase> bookcase { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Book_Statu> book_statu { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //取消复数表名惯例
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
        }
    }
}
