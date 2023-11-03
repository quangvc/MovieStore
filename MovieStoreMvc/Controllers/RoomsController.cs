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
    [Authorize(Roles ="Admin")]
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
            var data = await _context.Room.Include(r => r.cinema).Include(r => r.roomType).Include(r => r.Seats)
                .Select(r => new Room()
                {
                    Id = r.Id,
                    Name = r.Name,
                    cinema = r.cinema,
                    roomType = r.roomType,
                    TotalSeat = r.Seats.Count(),
                })
                .ToListAsync();
            return View(data);
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Room == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .Include(r => r.cinema).Include(r => r.roomType).Include(r => r.Seats)
                .Select(r => new Room()
                {
                    Id = r.Id,
                    Name = r.Name,
                    cinema = r.cinema,
                    roomType = r.roomType,
                    TotalSeat = r.Seats.Count(),
                })
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            ViewData["cinema"] = new SelectList(_context.Cinema, "Id", "Name");
            ViewData["roomType"] = new SelectList(_context.RoomType, "Id", "Name");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CinemaId,RoomTypeId,Name")] Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Add(room);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["cinema"] = new SelectList(_context.Cinema, "Id", "Name", room.cinema);
            ViewData["roomType"] = new SelectList(_context.RoomType, "Id", "Name", room.roomType);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Room == null)
            {
                return NotFound();
            }

            var room = await _context.Room.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            ViewData["cinema"] = new SelectList(_context.Cinema, "Id", "Name", room.cinema);
            ViewData["roomType"] = new SelectList(_context.RoomType, "Id", "Name", room.roomType);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CinemaId,RoomTypeId")] Room room)
        {
            if (id != room.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Id))
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
            ViewData["cinema"] = new SelectList(_context.Cinema, "Id", "Name", room.cinema);
            ViewData["roomType"] = new SelectList(_context.RoomType, "Id", "Name", room.roomType);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Room == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .Include(r => r.cinema).Include(r => r.roomType).Include(r => r.Seats)
                .Select(r => new Room()
                {
                    Id = r.Id,
                    Name = r.Name,
                    cinema = r.cinema,
                    roomType = r.roomType,
                    TotalSeat = r.Seats.Count(),
                })
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Room == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Room'  is null.");
            }
            var room = await _context.Room.FindAsync(id);
            if (room != null)
            {
                _context.Room.Remove(room);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
          return _context.Room.Any(e => e.Id == id);
        }
    }
}
