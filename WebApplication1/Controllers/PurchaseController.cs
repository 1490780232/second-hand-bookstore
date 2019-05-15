using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using Newtonsoft.Json;
namespace WebApplication1.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly bookstoreContext _context;

        public PurchaseController(bookstoreContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult OrderQuery()
        {
            var orderList = from p in _context.Book
                                join b in _context.Order 
                                on p.BookId equals b.BookId
                                select new {
                                    OrderId =b.OrderId,
                                    BookId=p.BookId,
                                    BookName=p.BookName,
                                    Press=p.Press,
                                    Author=p.Author,
                                    UserId=b.UserId,
                                    OrderTime=b.OrderTime,
                                    OrderPrice=b.OrderPrice
                                };
  
            string json = JsonConvert.SerializeObject(orderList.ToList());  //序列化
            ViewData["Message"] = "OrderQuery";
            ViewData["Order"] = json;
            return View();
        }
        public string GetData() {
            var orderList = from p in _context.Book
                            join b in _context.Order
                            on p.BookId equals b.BookId
                            select new
                            {
                                OrderId = b.OrderId,
                                BookId = p.BookId,
                                BookName = p.BookName,
                                Press = p.Press,
                                Author = p.Author,
                                UserId = b.UserId,
                                OrderTime = b.OrderTime,
                                OrderPrice = b.OrderPrice
                            };
           // var orderList = _context.Order.ToList();
            string getList = JsonConvert.SerializeObject(orderList);  //序列化
           // return new JsonResult(new { Data = getList });
            return getList;
        }
    }
}