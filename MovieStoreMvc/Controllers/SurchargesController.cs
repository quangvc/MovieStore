using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieStoreMvc.Data;
using MovieStoreMvc.Models;

namespace MovieStoreMvc.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class SurchargesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SurchargesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Surcharges
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Surcharge.Include(s => s.format).Include(s => s.room).Include(s => s.seat);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Surcharges/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Surcharge == null)
            {
                return NotFound();
            }

            var surcharge = await _context.Surcharge
                .Include(s => s.format)
                .Include(s => s.room)
                .Include(s => s.seat)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (surcharge == null)
            {
                return NotFound();
            }

            return View(surcharge);
        }

        // GET: Surcharges/Create
        public IActionResult Create()
        {
            ViewData["FormatId"] = new SelectList(_context.Format, "Id", "Name");
            ViewData["RoomId"] = new SelectList(_context.Room, "Id", "Id");
            ViewData["SeatTypeId"] = new SelectList(_context.SeatType, "Id", "Name");
            return View();
        }

        // POST: Surcharges/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Value,FormatId,RoomId,SeatTypeId")] Surcharge surcharge)
        {
            if (ModelState.IsValid)
            {
                _context.Add(surcharge);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FormatId"] = new SelectList(_context.Format, "Id", "Name", surcharge.FormatId);
            ViewData["RoomId"] = new SelectList(_context.Room, "Id", "Id", surcharge.RoomId);
            ViewData["SeatTypeId"] = new SelectList(_context.SeatType, "Id", "Id", surcharge.SeatTypeId);
            return View(surcharge);
        }

        // GET: Surcharges/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Surcharge == null)
            {
                return NotFound();
            }

            var surcharge = await _context.Surcharge.FindAsync(id);
            if (surcharge == null)
            {
                return NotFound();
            }
            ViewData["FormatId"] = new SelectList(_context.Format, "Id", "Name", surcharge.FormatId);
            ViewData["RoomId"] = new SelectList(_context.Room, "Id", "Id", surcharge.RoomId);
            ViewData["SeatTypeId"] = new SelectList(_context.SeatType, "Id", "Id", surcharge.SeatTypeId);
            return View(surcharge);
        }

        // POST: Surcharges/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Value,FormatId,RoomId,SeatTypeId")] Surcharge surcharge)
        {
            if (id != surcharge.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(surcharge);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SurchargeExists(surcharge.Id))
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
            ViewData["FormatId"] = new SelectList(_context.Format, "Id", "Name", surcharge.FormatId);
            ViewData["RoomId"] = new SelectList(_context.Room, "Id", "Id", surcharge.RoomId);
            ViewData["SeatTypeId"] = new SelectList(_context.SeatType, "Id", "Id", surcharge.SeatTypeId);
            return View(surcharge);
        }

        // GET: Surcharges/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Surcharge == null)
            {
                return NotFound();
            }

            var surcharge = await _context.Surcharge
                .Include(s => s.format)
                .Include(s => s.room)
                .Include(s => s.seat)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (surcharge == null)
            {
                return NotFound();
            }

            return View(surcharge);
        }

        // POST: Surcharges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Surcharge == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Surcharge'  is null.");
            }
            var surcharge = await _context.Surcharge.FindAsync(id);
            if (surcharge != null)
            {
                _context.Surcharge.Remove(surcharge);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SurchargeExists(int id)
        {
          return _context.Surcharge.Any(e => e.Id == id);
        }
    }
}
