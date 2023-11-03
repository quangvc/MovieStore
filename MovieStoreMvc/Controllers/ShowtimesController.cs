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
    public class ShowtimesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShowtimesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Showtimes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Showtimes.Include(s => s.format).Include(s => s.movie).Include(s => s.room)
                .Select(s => new Showtimes()
                {
                    Id = s.Id,
                    StartTime = s.StartTime,
                    Price = s.Price,
                    movie = s.movie,
                    room = s.room,
                    format = s.format,
                    TicketSold = _context.Ticket.Count(t => t.ShowtimesId == s.Id),
                    TotalPrice = _context.Ticket.Where(t => t.ShowtimesId == s.Id).Select(t => t.Price).Sum(),
                });
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Showtimes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Showtimes == null)
            {
                return NotFound();
            }

            var showtimes = await _context.Showtimes
                .Include(s => s.format)
                .Include(s => s.movie)
                .Include(s => s.room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (showtimes == null)
            {
                return NotFound();
            }

            return View(showtimes);
        }

        // GET: Showtimes/Create
        public IActionResult Create()
        {
            ViewData["FormatId"] = new SelectList(_context.Format, "Id", "Name");
            ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Title");
            ViewData["RoomId"] = new SelectList(_context.Room, "Id", "Name");
            return View();
        }

        // POST: Showtimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MovieId,RoomId,FormatId,StartTime,Price")] Showtimes showtimes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(showtimes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FormatId"] = new SelectList(_context.Format, "Id", "Name", showtimes.FormatId);
            ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Title", showtimes.MovieId);
            ViewData["RoomId"] = new SelectList(_context.Room, "Id", "Id", showtimes.RoomId);
            return View(showtimes);
        }

        // GET: Showtimes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Showtimes == null)
            {
                return NotFound();
            }

            var showtimes = await _context.Showtimes.FindAsync(id);
            if (showtimes == null)
            {
                return NotFound();
            }
            ViewData["FormatId"] = new SelectList(_context.Format, "Id", "Name", showtimes.FormatId);
            ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Title", showtimes.MovieId);
            ViewData["RoomId"] = new SelectList(_context.Room, "Id", "Name", showtimes.RoomId);
            return View(showtimes);
        }

        // POST: Showtimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MovieId,RoomId,FormatId,StartTime,Price")] Showtimes showtimes)
        {
            if (id != showtimes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(showtimes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShowtimesExists(showtimes.Id))
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
            ViewData["FormatId"] = new SelectList(_context.Format, "Id", "Name", showtimes.FormatId);
            ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Title", showtimes.MovieId);
            ViewData["RoomId"] = new SelectList(_context.Room, "Id", "Id", showtimes.RoomId);
            return View(showtimes);
        }

        // GET: Showtimes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Showtimes == null)
            {
                return NotFound();
            }

            var showtimes = await _context.Showtimes
                .Include(s => s.format)
                .Include(s => s.movie)
                .Include(s => s.room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (showtimes == null)
            {
                return NotFound();
            }

            return View(showtimes);
        }

        // POST: Showtimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Showtimes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Showtimes'  is null.");
            }
            var showtimes = await _context.Showtimes.FindAsync(id);
            if (showtimes != null)
            {
                _context.Showtimes.Remove(showtimes);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShowtimesExists(int id)
        {
          return _context.Showtimes.Any(e => e.Id == id);
        }
    }
}
