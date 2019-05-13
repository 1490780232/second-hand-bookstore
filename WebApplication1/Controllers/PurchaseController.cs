using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class PurchaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult OrderQuery()
        {
            ViewData["Message"] = "OrderQuery";
            return View();
        }
    }
}