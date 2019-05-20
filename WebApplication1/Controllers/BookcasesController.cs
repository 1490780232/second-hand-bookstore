using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BookcasesController : Controller
    {
        private readonly bookstoreContext _context;

        public BookcasesController(bookstoreContext context)
        {
            _context = context;
        }

        // GET: Bookcases
        public async Task<IActionResult> Index()
        {
            return View(await _context.Bookcase.ToListAsync());
        }

        // GET: Bookcases/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookcase = await _context.Bookcase
                .FirstOrDefaultAsync(m => m.BookcaseId == id);
            if (bookcase == null)
            {
                return NotFound();
            }

            return View(bookcase);
        }

        // GET: Bookcases/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bookcases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookcaseId,Category,Rfid")] Bookcase bookcase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookcase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookcase);
        }

        // GET: Bookcases/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookcase = await _context.Bookcase.FindAsync(id);
            if (bookcase == null)
            {
                return NotFound();
            }
            return View(bookcase);
        }

        // POST: Bookcases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BookcaseId,Category,Rfid")] Bookcase bookcase)
        {
            if (id != bookcase.BookcaseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookcase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookcaseExists(bookcase.BookcaseId))
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
            return View(bookcase);
        }

        // GET: Bookcases/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookcase = await _context.Bookcase
                .FirstOrDefaultAsync(m => m.BookcaseId == id);
            if (bookcase == null)
            {
                return NotFound();
            }

            return View(bookcase);
        }

        // POST: Bookcases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var bookcase = await _context.Bookcase.FindAsync(id);
            _context.Bookcase.Remove(bookcase);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookcaseExists(string id)
        {
            return _context.Bookcase.Any(e => e.BookcaseId == id);
        }
    }
}
