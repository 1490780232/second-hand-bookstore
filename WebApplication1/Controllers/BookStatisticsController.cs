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
    public class BookStatisticsController : Controller
    {
        private readonly bookstoreContext _context;

        public BookStatisticsController(bookstoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ViewBookStatistics()
        {
            return View(await _context.BookStatistics.ToListAsync());
        }

        // GET: BookStatistics
        public async Task<IActionResult> Index()
        {
            return View(await _context.BookStatistics.ToListAsync());
        }

        // GET: BookStatistics/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookStatistics = await _context.BookStatistics
                .FirstOrDefaultAsync(m => m.BookCategory == id);
            if (bookStatistics == null)
            {
                return NotFound();
            }

            return View(bookStatistics);
        }

        // GET: BookStatistics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BookStatistics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookCategory,BookNum,TotalValue")] BookStatistics bookStatistics)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookStatistics);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookStatistics);
        }

        // GET: BookStatistics/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookStatistics = await _context.BookStatistics.FindAsync(id);
            if (bookStatistics == null)
            {
                return NotFound();
            }
            return View(bookStatistics);
        }

        // POST: BookStatistics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BookCategory,BookNum,TotalValue")] BookStatistics bookStatistics)
        {
            if (id != bookStatistics.BookCategory)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookStatistics);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookStatisticsExists(bookStatistics.BookCategory))
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
            return View(bookStatistics);
        }

        // GET: BookStatistics/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookStatistics = await _context.BookStatistics
                .FirstOrDefaultAsync(m => m.BookCategory == id);
            if (bookStatistics == null)
            {
                return NotFound();
            }

            return View(bookStatistics);
        }

        // POST: BookStatistics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var bookStatistics = await _context.BookStatistics.FindAsync(id);
            _context.BookStatistics.Remove(bookStatistics);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookStatisticsExists(string id)
        {
            return _context.BookStatistics.Any(e => e.BookCategory == id);
        }
    }
}
