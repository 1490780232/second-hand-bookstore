using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BookQueryController : Controller
    {
        private readonly bookstoreContext _context;

        public BookQueryController(bookstoreContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Condition()
        {
            ViewData["Message"] = "Condition";
            return View();
        }
        public IActionResult Result(string bookname)
        {
            //var StudentsQuery = from b in db.book_info
            //                    where b.bookName.Contains(s)
            //                    select b;

            //List<Book> StudentsList = StudentsQuery.ToList();
            ViewData["Message"] = "Result";
            return View();
        }
        public IActionResult ResultDetail(string id)
        {
            ViewData["Message"] = "ResultDetail";
            var orderList = from p in _context.Book
                            join b in _context.BookStatu
                            on p.BookId equals b.BookId
                            join person in _context.Users
                            on b.SellerId equals person.UserId
                            where p.BookId.Contains(id)
                            select new
                            {
                                BookId = p.BookId,
                                BookCase=b.BookcaseId,
                                BookName = p.BookName,
                                SalerId=b.SellerId,
                                SalerName=person.UserName,
                                SalerContact=person.ContactInfo,
                                BookIbsn = p.BookIbsn,
                                Author = p.Author,
                                Press = p.Press,
                                OriPrice = p.OriPrice,
                                CurrPrice = p.CurrPrice
                            };
            // var orderList = _context.Order.ToList();
            string getList = JsonConvert.SerializeObject(orderList);  //序列化
                                                                      // return new JsonResult(new { Data = getList });
            return View();
        }

        public string GetData(string bookname)
        {
            var orderList = from p in _context.Book
                            where p.BookName.Contains(bookname)
                            select new
                            {
                                BookId = p.BookId,
                                BookName = p.BookName,
                                BookIbsn = p.BookIbsn,
                                Author = p.Author,
                                Press = p.Press,
                                OriPrice = p.OriPrice,
                                CurrPrice = p.CurrPrice
                            };
            string getList = JsonConvert.SerializeObject(orderList);  //序列化
            // ViewData["data"] = getList;                                                  // return new JsonResult(new { Data = getList });
            return getList;
        }
        public IActionResult GetBooks(string category)
        {
            var orderList = from p in _context.Book
                            where p.category==category
                            select new
                            {
                                BookId = p.BookId,
                                BookName = p.BookName,
                                BookIbsn = p.BookIbsn,
                                Author = p.Author,
                                Press = p.Press,
                                OriPrice = p.OriPrice,
                                CurrPrice = p.CurrPrice,                                
                            };
            string getList = JsonConvert.SerializeObject(orderList);  //序列化
            // ViewData["data"] = getList;                                                 
            // return new JsonResult(new { state:"success",message = getList });
            return new JsonResult(new { state = "success", suggest_book= getList,});
            // return getList;
        }
    }
}