using Model;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;

namespace DAL
{
    public class bookAction
    {
        //添加记录
        public static bool InsertBook(Book bo1)
        {
            using (bookDbContext db = new bookDbContext())
            {
                db.book_info.Add(bo1);
                return db.SaveChanges() > 0;  //保存数据
            }
        }

        //修改记录(book中应包含主键ID值)
        public static bool UpdateBook(Book bo1)
        {
            using (bookDbContext db = new bookDbContext())
            {
                db.book_info.Attach(bo1);
                db.Entry(bo1).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
        }

        //删除记录(book中应包含主键ID值)
        public static bool DeleteBook(Book b0)
        {
            using (bookDbContext db = new bookDbContext())
            {
                db.book_info.Attach(b0);
                db.book_info.Remove(b0);
                return db.SaveChanges() > 0;
            }
        }


        //返回所有记录
        public static List<Book> GetAllBook()
        {
            using (bookDbContext db = new bookDbContext())
            {
                var StudentsQuery = (from b in db.book_info select b).ToList();
                return StudentsQuery;
            }
        }

        //返回查询记录
        public static List<Book> GetAllBook(string s)
        {
            using (bookDbContext db = new bookDbContext())
            {
                var StudentsQuery = from b in db.book_info
                                 where b.bookName.Contains(s)
                                 select b;

                List<Book> StudentsList = StudentsQuery.ToList();

                return StudentsList;
            }
        }

        //bookstatu的查询
        public static bool InsertBookStatu(Book_Statu bo1)
        {
            using (bookDbContext db = new bookDbContext())
            {
                db.book_statu.Add(bo1);
                return db.SaveChanges() > 0;  //保存数据
            }
        }

        //修改记录(book应包含主键ID值)
        public static bool UpdateBook_Statu(Book_Statu bo1)
        {
            using (bookDbContext db = new bookDbContext())
            {
                db.book_statu.Attach(bo1);
                db.Entry(bo1).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
        }

        //删除记录(book中应包含主键ID值)
        public static bool DeleteBook_Statu(Book_Statu b0)
        {
            using (bookDbContext db = new bookDbContext())
            {
                db.book_statu.Attach(b0);
                db.book_statu.Remove(b0);
                return db.SaveChanges() > 0;
            }
        }


        //返回所有记录
        public static List<Book_Statu> GetAllBook_Statu()
        {
            using (bookDbContext db = new bookDbContext())
            {
                var StudentsQuery = (from b in db.book_statu select b).ToList();
                return StudentsQuery;
            }
        }

        //查询接口。。。。未完待续


        public static bool InsertAdmin(Admin bo1)
        {
            using (bookDbContext db = new bookDbContext())
            {
                db.Admin.Add(bo1);
                return db.SaveChanges() > 0;  //保存数据
            }
        }

        //修改记录(book中应包含主键ID值)
        public static bool UpdateAdmin(Admin bo1)
        {
            using (bookDbContext db = new bookDbContext())
            {
                db.Admin.Attach(bo1);
                db.Entry(bo1).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
        }

        //删除记录(book中应包含主键ID值)
        public static bool DeleteAdmin(Admin b0)
        {
            using (bookDbContext db = new bookDbContext())
            {
                db.Admin.Attach(b0);
                db.Admin.Remove(b0);
                return db.SaveChanges() > 0;
            }
        }


        //返回所有记录
        public static List<Admin> GetAllAdmin()
        {
            using (bookDbContext db = new bookDbContext())
            {
                var StudentsQuery = (from b in db.Admin select b).ToList();
                return StudentsQuery;
            }
        }

        //返回查询记录



        public static bool Insertbookcase(bookcase bo1)
        {
            using (bookDbContext db = new bookDbContext())
            {
                db.bookcase.Add(bo1);
                return db.SaveChanges() > 0;  //保存数据
            }
        }

        //修改记录(book中应包含主键ID值)
        public static bool Updatebookcase(bookcase bo1)
        {
            using (bookDbContext db = new bookDbContext())
            {
                db.bookcase.Attach(bo1);
                db.Entry(bo1).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
        }

        //删除记录(book中应包含主键ID值)
        public static bool Deletebookcase(bookcase b0)
        {
            using (bookDbContext db = new bookDbContext())
            {
                db.bookcase.Attach(b0);
                db.bookcase.Remove(b0);
                return db.SaveChanges() > 0;
            }
        }


        //返回所有记录
        public static List<bookcase> GetAllbookcase()
        {
            using (bookDbContext db = new bookDbContext())
            {
                var StudentsQuery = (from b in db.bookcase select b).ToList();
                return StudentsQuery;
            }
        }


        public static bool InsertOrder(Order bo1)
        {
            using (bookDbContext db = new bookDbContext())
            {
                db.Order.Add(bo1);
                return db.SaveChanges() > 0;  //保存数据
            }
        }

        //修改记录(book中应包含主键ID值)
        public static bool UpdateOrder(Order bo1)
        {
            using (bookDbContext db = new bookDbContext())
            {
                db.Order.Attach(bo1);
                db.Entry(bo1).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
        }

        //删除记录(book中应包含主键ID值)
        public static bool DeleteOrder(Order b0)
        {
            using (bookDbContext db = new bookDbContext())
            {
                db.Order.Attach(b0);
                db.Order.Remove(b0);
                return db.SaveChanges() > 0;
            }
        }


        //返回所有记录
        public static List<Order> GetAllOrder(string user)//查看userID的订单
        {
            using (bookDbContext db = new bookDbContext())
            {
                var StudentsQuery = (from b in db.Order where b.userID==user select b).ToList();
                return StudentsQuery;
            }
        }



        public static bool InsertUsers(Users bo1)
        {
            using (bookDbContext db = new bookDbContext())
            {
                db.Users.Add(bo1);
                return db.SaveChanges() > 0;  //保存数据
            }
        }

        //修改记录(book中应包含主键ID值)
        public static bool UpdateOUsers(Users bo1)
        {
            using (bookDbContext db = new bookDbContext())
            {
                db.Users.Attach(bo1);
                db.Entry(bo1).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
        }

        //删除记录(book中应包含主键ID值)
        public static bool DeleteUsers(Users b0)
        {
            using (bookDbContext db = new bookDbContext())
            {
                db.Users.Attach(b0);
                db.Users.Remove(b0);
                return db.SaveChanges() > 0;
            }
        }


        //返回所有记录
        public static List<Users> GetAllUsers()//查看userID的订单
        {
            using (bookDbContext db = new bookDbContext())
            {
                var StudentsQuery = (from b in db.Users  select b).ToList();
                return StudentsQuery;
            }
        }



    }



}
