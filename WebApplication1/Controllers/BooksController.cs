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
                re = Reader.Create("192.168.0.103", ModuleTech.Region.NA, 4);
                int[] ants = new int[] { 1 };
                SimpleReadPlan plan = new SimpleReadPlan(TagProtocol.GEN2, ants);
                re.ParamSet("ReadPlan", plan);
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ActionResult Position()
        {

            Random random = new Random();
            int num = random.Next(30, 300);
            int num2 = random.Next(30, 300);
            return new JsonResult(new { x = num, y = num2 });

        }

        public ActionResult PublicBook(string book)
        {
            Book book1=null;
            int a=_context.Book.Count()+1;

            string id = Convert.ToString(a);


            book1 = JsonConvert.DeserializeObject<Book>(book);
            book1.BookId = "00" + id;


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
                _context.Add(book1);
                 _context.SaveChanges();
                re.Disconnect();
                string getBook = JsonConvert.SerializeObject(book1);

                return new JsonResult(new { state = "success", message = getBook, new_book = getBook});

            }
            catch
            {
                return new JsonResult(new { state = "failed", message = "未成功写入RFID" });
            }

        }

        public ActionResult  BuyBooks()
        {
            string id = null;
            bool flag = connect();
            if (flag == false)
            {
                return new JsonResult(new { state = "failed", message = "固定读写器未连接上" });
            }

            List<string> tags = new List<string>();
            try
            {
                string str;
                TagReadData[] tagda = re.Read(200);
                if (tagda.Length > 0)
                {
                    str = tagda[0].Tag.ToString();
                    id = str.Substring(4);//.EPCString;
                }

            }
            catch
            {
                return new JsonResult(new { state = "failed", message = "未读出数据" });
            }
           
            var book = from p in _context.Book where p.BookId.Contains(id) select p ;
            if (book == null)
            {
                return  new JsonResult(new { state = "failed", message = "RFID信息错误" });
            }
            string book1 = JsonConvert.SerializeObject(book.First<Book>());
            return new JsonResult(new { state = "success", message = book1 });
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
        public async Task<IActionResult> Create([Bind("BookId,BookName,BookIbsn,Author,OriPrice,Press,CurrPrice")] Book book)
        {
            if (ModelState.IsValid)
            {

                _context.Add(book);

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
