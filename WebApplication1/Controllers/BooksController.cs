using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication1.Models;
using ModuleTech;

namespace WebApplication1.Controllers
{
    public class BooksController : Controller
    {
        private readonly bookstoreContext _context;

        public Reader re;
        public bool connect()
        {
            try
            {
                re = Reader.Create("192.168.0.102", ModuleTech.Region.NA, 1);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public ActionResult PublicBook(string book)
        {
            Book book1  = JsonConvert.DeserializeObject<Book>(book);
            bool flag = connect();
            if (flag == false)
            {
                return new JsonResult(new { state = "failed", message = "固定读写器未连接上" });
            }

            try
            {
                string s = book1.BookId;
                TagData epccode = new TagData(s);
                re.WriteTag(null, epccode);
                return new JsonResult(new { state = "true", message = "发布成功" });
            }
            catch
            {
                return new JsonResult(new { state = "failed", message = "未成功写入RFID" });
            }
            
        }

        public BooksController(bookstoreContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> OperateBooks()
        {
            return View(await _context.Book.ToListAsync());
        }
        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await _context.Book.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,BookName,BookIbsn,Author,OriPrice,Press,CurrPrice")] Book book, [Bind("BookId,BookName,BookIbsn,Author,OriPrice,Press,CurrPrice")] Book book2)
        {
            if (ModelState.IsValid)
            {
                book2.BookId += "1";
                _context.Add(book);
                _context.Add(book2);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BookId,BookName,BookIbsn,Author,OriPrice,Press,CurrPrice")] Book book)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var book = await _context.Book.FindAsync(id);
            _context.Book.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(string id)
        {
            return _context.Book.Any(e => e.BookId == id);
        }
    }
}
