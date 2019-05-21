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
    public class CheckBooksController : Controller
    {
        private readonly bookstoreContext _context;

        public CheckBooksController(bookstoreContext context)
        {
            _context = context;
        }

        // GET: CheckBooks
        public async Task<IActionResult> Index()
        {
            return View(await _context.CheckBook.ToListAsync());
        }

        // GET: CheckBooks/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkBook = await _context.CheckBook
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (checkBook == null)
            {
                return NotFound();
            }

            return View(checkBook);
        }

        // GET: CheckBooks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CheckBooks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,CheckStatus,FaileReason,AdminId,CheckTime")] CheckBook checkBook)
        {
            if (ModelState.IsValid)
            {
                _context.Add(checkBook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(checkBook);
        }

        // GET: CheckBooks/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkBook = await _context.CheckBook.FindAsync(id);
            if (checkBook == null)
            {
                return NotFound();
            }
            return View(checkBook);
        }

        // POST: CheckBooks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BookId,CheckStatus,FaileReason,AdminId,CheckTime")] CheckBook checkBook)
        {
            if (id != checkBook.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(checkBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CheckBookExists(checkBook.BookId))
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
            return View(checkBook);
        }

        // GET: CheckBooks/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkBook = await _context.CheckBook
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (checkBook == null)
            {
                return NotFound();
            }

            return View(checkBook);
        }

        // POST: CheckBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var checkBook = await _context.CheckBook.FindAsync(id);
            _context.CheckBook.Remove(checkBook);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CheckBookExists(string id)
        {
            return _context.CheckBook.Any(e => e.BookId == id);
        }
    }
}
