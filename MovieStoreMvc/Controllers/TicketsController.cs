using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MovieStoreMvc.Data;
using MovieStoreMvc.Data.Migrations;
using MovieStoreMvc.Models;
using static System.Net.Mime.MediaTypeNames;

namespace MovieStoreMvc.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Ticket.Include(t => t.seat).Include(t => t.showtimes);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ticket == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.seat)
                .Include(t => t.showtimes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["Room"] = new SelectList(_context.Room.Select(s => new
            {
                Id = s.Id,
                Name = s.Name
            }).ToList(), "Id", "Name");

            List<SelectListItem> lstDate = new List<SelectListItem>();
            for (int i = 0; i < 7; i++)
            {
                lstDate.Add(new SelectListItem
                {
                    Text = DateTime.Today.AddDays(i).ToString("dd/MM/yyyy"),
                    Value = DateTime.Today.AddDays(i).ToString("dd/MM/yyyy")
                });                
            }

            ViewData["Date"] = new SelectList(
                //_context.Showtimes.Where(s => s.StartTime > DateTime.Now)
                //.Select(s => new
                //{
                //    Date = s.StartTime.ToString("dd/MM/yyyy")
                //}).ToList().Distinct(), "Date", "Date");

                lstDate, "Value", "Text", 1
                );

            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,CreatedDate")] Ticket ticket, string seat, int room, string date, string time)
        {
            if (ModelState.IsValid)
            {
                CultureInfo culture = new CultureInfo("es-ES");
                DateTime myDate = DateTime.Parse(date, culture);

                var myTime = DateTime.ParseExact(time, "H:mm", null, System.Globalization.DateTimeStyles.None);

                ticket.SeatId = await _context.Seat.Where(s => s.RoomId == room && s.Position.Equals(seat)).Select(s => s.Id).FirstOrDefaultAsync();

                ticket.ShowtimesId = await _context.Showtimes.Where(s => s.StartTime.Day == myDate.Day && s.StartTime.Month == myDate.Month && s.StartTime.Year == myDate.Year
                && s.StartTime.Hour == myTime.Hour && s.StartTime.Minute == myTime.Minute).Select(s => s.Id).FirstOrDefaultAsync();

                var newSeat = await _context.Seat
                    .Include(s => s.room)
                        .ThenInclude(r => r.cinema)
                    .FirstOrDefaultAsync(s => s.Id == ticket.SeatId);

                var newShowtimes = await _context.Showtimes
                    .Include(s => s.format)
                    .Include(s => s.movie)
                    .Include(s => s.room)
                    .FirstOrDefaultAsync(s => s.Id == ticket.ShowtimesId);

                ticket.CreatedDate = DateTime.Now;
                ticket.Name = newShowtimes.format.Name + " " + newShowtimes.movie.Title + " [" + newShowtimes.room.Name + "-" + seat + "]" + "\n" + newShowtimes.StartTime + "\n" + newSeat.room.cinema.Name;
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SeatId"] = new SelectList(_context.Seat, "Id", "Id", ticket.SeatId);
            ViewData["ShowtimesId"] = new SelectList(_context.Showtimes, "Id", "Id", ticket.ShowtimesId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ticket == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["SeatId"] = new SelectList(_context.Seat, "Id", "Position", ticket.SeatId);
            ViewData["ShowtimesId"] = new SelectList(_context.Showtimes, "Id", "StartTime", ticket.ShowtimesId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SeatId,ShowtimesId,Name,Description,Price,CreatedDate,UpdatedDate")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            ViewData["SeatId"] = new SelectList(_context.Seat, "Id", "Id", ticket.SeatId);
            ViewData["ShowtimesId"] = new SelectList(_context.Showtimes, "Id", "Id", ticket.ShowtimesId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ticket == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.seat)
                .Include(t => t.showtimes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ticket == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Ticket'  is null.");
            }
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket != null)
            {
                _context.Ticket.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("Tickets/GetSeats")]
        public JsonResult GetSeatByRoom(int? RoomId)
        {
            var seats = _context.Seat.Where(s => s.RoomId == RoomId).Select(s => s.Position);

            return Json(seats.ToList());
        }

        [HttpGet]
        [Route("Tickets/GetTimes")]
        public JsonResult GetTimesByDate(string date)
        {
            CultureInfo culture = new CultureInfo("es-ES");
            DateTime myDate = DateTime.Parse(date, culture);

            var times = _context.Showtimes
            .Where(s => s.StartTime.Day == myDate.Day && s.StartTime.Month == myDate.Month && s.StartTime.Year == myDate.Year)
            .Select(s => s.StartTime.ToString("H:mm"));

            return Json(times.ToList());
        }




    }
}
