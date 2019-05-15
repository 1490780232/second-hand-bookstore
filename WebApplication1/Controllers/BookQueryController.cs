using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class BookQueryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Condition()
        {
            ViewData["Message"] = "Condition";
            return View();
        }
        public IActionResult Result()
        {
            ViewData["Message"] = "Result";
            return View();
        }
        public IActionResult ResultDetail()
        {
            ViewData["Message"] = "ResultDetail";
            return View();
        }
    }
}