using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class UserCenterController : Controller
    {
        public IActionResult Index()
        {
            return View();
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
        public IActionResult MyOrder()
        {
            return View();
        }
    }
}