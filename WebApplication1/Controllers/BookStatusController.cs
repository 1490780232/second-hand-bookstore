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
    public class BookStatusController : Controller
    {
        private readonly bookstoreContext _context;

        public BookStatusController(bookstoreContext context)
        {
            _context = context;
        }

        // GET: BookStatus
        public async Task<IActionResult> Index()
        {
            return View(await _context.BookStatu.ToListAsync());
        }

        // GET: BookStatus/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookStatu = await _context.BookStatu
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (bookStatu == null)
            {
                return NotFound();
            }

            return View(bookStatu);
        }

        // GET: BookStatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BookStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,BookcaseId,BookStatus,CheckStatus,Rfid,SellerId,STime")] BookStatu bookStatu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookStatu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookStatu);
        }

        // GET: BookStatus/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookStatu = await _context.BookStatu.FindAsync(id);
            if (bookStatu == null)
            {
                return NotFound();
            }
            return View(bookStatu);
        }

        // POST: BookStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BookId,BookcaseId,BookStatus,CheckStatus,Rfid,SellerId,STime")] BookStatu bookStatu)
        {
            if (id != bookStatu.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookStatu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookStatuExists(bookStatu.BookId))
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
            return View(bookStatu);
        }

        // GET: BookStatus/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookStatu = await _context.BookStatu
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (bookStatu == null)
            {
                return NotFound();
            }

            return View(bookStatu);
        }

        // POST: BookStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var bookStatu = await _context.BookStatu.FindAsync(id);
            _context.BookStatu.Remove(bookStatu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookStatuExists(string id)
        {
            return _context.BookStatu.Any(e => e.BookId == id);
        }
    }
}
