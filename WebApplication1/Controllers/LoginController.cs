using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        private readonly bookstoreContext db = new bookstoreContext();
        public IActionResult Index()
        {          
            return View();
        }

        public IActionResult User()
        {

            ViewData["Message"] = "User";
            return View();
        }

        public IActionResult Admin()
        {
            ViewData["Message"] = "Admin";
            return View();
        }
    }
}