using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class UsersController : Controller
    {
        private readonly bookstoreContext _context;

        public UsersController(bookstoreContext context)
        {
            _context = context;
        }

        public ActionResult Check(string username, string password)
        {
            foreach (var item in _context.Users.ToList())
            {
                if (username == item.UserName && password == item.Password)
                {
                    return new JsonResult(new { state = "success", message = "登录成功,正在跳转...",username=username,userType="users" });
                }
            }
            return new JsonResult(new { state = "failed", message = "登录失败，请检查您的账户与密码是否正确" });
        }

        public async Task<IActionResult> OperateUsers()
        {
            return View(await _context.Users.ToListAsync());
        }
 
        public async Task<IActionResult> Position()
        {
            return View();
        }
        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        public IActionResult getInfo(string username)
        {
            var users = from p in _context.Users
                            where p.UserName == username
                            select new
                            {
                                UserId = p.UserId,
                                UserName = p.UserName,
                                AlipayAccount = p.AlipayAccount,
                                ContactInfo = p.ContactInfo,
                                Mail = p.Mail,
                                Password = p.Password,
                                Sex = p.sex
                            };
            string getUser = JsonConvert.SerializeObject(users);  //序列化
            // ViewData["data"] = getList;                                                  // return new JsonResult(new { Data = getList });
            return new JsonResult(new { state = "success", user = getUser, });
        }

        public IActionResult Update(string user)
        {
            Users my_user = JsonConvert.DeserializeObject<Users>(user);
            _context.Update(my_user);
            if (_context.SaveChanges() > 0)
                return new JsonResult(new { state = "success", message = "提交成功" });
            else
                return new JsonResult(new { state = "faild", message = "提交失败，请重新提交" });
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserName,AlipayAccount,ContactInfo,Mail")] Users users)
        {
            if (ModelState.IsValid)
            {
                _context.Add(users);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(users);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( [Bind("UserId,UserName,AlipayAccount,ContactInfo,Mail")] Users users)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(OperateUsers));
            }
            return View(users);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var users = await _context.Users.FindAsync(id);
            _context.Users.Remove(users);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(string id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
