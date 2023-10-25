using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieStoreMvc.Data;
using MovieStoreMvc.Models;

namespace MovieStoreMvc.Controllers
{
    public class SeatTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SeatTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SeatTypes
        public async Task<IActionResult> Index()
        {
              return View(await _context.SeatType.ToListAsync());
        }

        // GET: SeatTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SeatType == null)
            {
                return NotFound();
            }

            var seatType = await _context.SeatType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seatType == null)
            {
                return NotFound();
            }

            return View(seatType);
        }

        // GET: SeatTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SeatTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price")] SeatType seatType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(seatType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(seatType);
        }

        // GET: SeatTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SeatType == null)
            {
                return NotFound();
            }

            var seatType = await _context.SeatType.FindAsync(id);
            if (seatType == null)
            {
                return NotFound();
            }
            return View(seatType);
        }

        // POST: SeatTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price")] SeatType seatType)
        {
            if (id != seatType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seatType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeatTypeExists(seatType.Id))
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
            return View(seatType);
        }

        // GET: SeatTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SeatType == null)
            {
                return NotFound();
            }

            var seatType = await _context.SeatType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seatType == null)
            {
                return NotFound();
            }

            return View(seatType);
        }

        // POST: SeatTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SeatType == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SeatType'  is null.");
            }
            var seatType = await _context.SeatType.FindAsync(id);
            if (seatType != null)
            {
                _context.SeatType.Remove(seatType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeatTypeExists(int id)
        {
          return _context.SeatType.Any(e => e.Id == id);
        }
    }
}
