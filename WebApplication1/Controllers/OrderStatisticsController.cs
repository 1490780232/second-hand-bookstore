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
    public class OrderStatisticsController : Controller
    {
        private readonly bookstoreContext _context;

        public OrderStatisticsController(bookstoreContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> ViewOrderStatistics()
        {
            return View(await _context.OrderStatistics.ToListAsync());
        }
        // GET: OrderStatistics
        public async Task<IActionResult> Index()
        {
            return View(await _context.OrderStatistics.ToListAsync());
        }

        // GET: OrderStatistics/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderStatistics = await _context.OrderStatistics
                .FirstOrDefaultAsync(m => m.time == id);
            if (orderStatistics == null)
            {
                return NotFound();
            }

            return View(orderStatistics);
        }

        // GET: OrderStatistics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrderStatistics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("time,BookNum,TotalValue")] OrderStatistics orderStatistics)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderStatistics);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orderStatistics);
        }

        // GET: OrderStatistics/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderStatistics = await _context.OrderStatistics.FindAsync(id);
            if (orderStatistics == null)
            {
                return NotFound();
            }
            return View(orderStatistics);
        }

        // POST: OrderStatistics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("time,BookNum,TotalValue")] OrderStatistics orderStatistics)
        {
            if (id != orderStatistics.time)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderStatistics);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderStatisticsExists(orderStatistics.time))
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
            return View(orderStatistics);
        }

        // GET: OrderStatistics/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderStatistics = await _context.OrderStatistics
                .FirstOrDefaultAsync(m => m.time == id);
            if (orderStatistics == null)
            {
                return NotFound();
            }

            return View(orderStatistics);
        }

        // POST: OrderStatistics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var orderStatistics = await _context.OrderStatistics.FindAsync(id);
            _context.OrderStatistics.Remove(orderStatistics);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderStatisticsExists(string id)
        {
            return _context.OrderStatistics.Any(e => e.time == id);
        }
    }
}
