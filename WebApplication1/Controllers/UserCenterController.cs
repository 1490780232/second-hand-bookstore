using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    public class UserCenterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly bookstoreContext _context;

        public UserCenterController(bookstoreContext context)
        {
            _context = context;
        }




        public IActionResult PersonalCenter()
        {
            ViewData["Message"] = "PersonalCenter";
            return View();
        }

        public IActionResult UserInform()
        {
            return View();
        }

        public IActionResult CheckInform()
        {
            return View();
        }
        public IActionResult Favorite()
        {
            return View();
        }
        public async Task<IActionResult> MyOrder(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
   
            var order = await _context.Order.Where(m => m.buyerName == id).ToListAsync();
  
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
  
        }
        public async Task<IActionResult> MySell(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Book.Where(m => m.userName == id).ToListAsync();

            if (order == null)
            {
                return NotFound();
            }

            return View(order);

        }
    }
}