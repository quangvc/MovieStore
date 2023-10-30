using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
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
            ViewData["Cinema"] = new SelectList(_context.Cinema.Select(s => new
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

                lstDate, "Value", "Text", 1
                );
            ViewData["Movie"] = new SelectList(_context.Movie, "Id", "Title");

            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,CreatedDate")] Ticket ticket, string seat, string date, string time)
        {
            if (ModelState.IsValid)
            {
                CultureInfo culture = new CultureInfo("es-ES");
                DateTime myDate = DateTime.Parse(date, culture);

                var myTime = DateTime.ParseExact(time, "H:mm", null, System.Globalization.DateTimeStyles.None);

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

                ticket.SeatId = await _context.Seat.Where(s => s.RoomId == newShowtimes.RoomId && s.Position.Equals(seat)).Select(s => s.Id).FirstOrDefaultAsync();

                ticket.CreatedDate = DateTime.Now;

                //var ticketPrice = await _context.Seat
                //    .Include (s => s.seatType)
                //    .Where(s => s.Id == ticket.SeatId).Select(s => s.seatType.Price).FirstOrDefaultAsync();
                //if (ticketPrice != null && ticketPrice != 0)
                //{
                //    ticket.Price = ticketPrice;
                //} else
                //{
                //    ticket.Price = newShowtimes.Price;
                //    var seatTypeSurcharges = await _context.Surcharge.Where(c => c.SeatTypeId == ticket.seat.SeatTypeId).Select(c => c.Value).FirstOrDefaultAsync();
                //    var roomSurcharges = await _context.Surcharge.Where(c => c.RoomId == ticket.showtimes.RoomId).Select(c => c.Value).FirstOrDefaultAsync();
                //    var formatMovieSurcharges = await _context.Surcharge.Where(c => c.FormatId == ticket.showtimes.FormatId).Select(c => c.Value).FirstOrDefaultAsync();
                //    if (seatTypeSurcharges != null)
                //    {
                //        ticket.Price += seatTypeSurcharges;
                //    } 
                //    if (roomSurcharges != null)
                //    {
                //        ticket.Price += roomSurcharges;
                //    }
                //    if (formatMovieSurcharges != null)
                //    {
                //        ticket.Price += formatMovieSurcharges;
                //    }
                //}

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
            ViewData["SeatId"] = new SelectList(_context.Seat, "Id", "Id", ticket.SeatId);
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
        public JsonResult GetSeats(int? showtimesId)
        {
            var RoomId = _context.Showtimes.Where(e => e.Id == showtimesId).Select(e => e.RoomId).FirstOrDefault();
            var seats = _context.Seat.Where(s => s.RoomId == RoomId).Select(s => s.Position);

            return Json(seats.ToList());
        }

        [HttpGet]
        [Route("Tickets/GetShowtimes")]
        public JsonResult GetTimesByDate(string date)
        {
            CultureInfo culture = new CultureInfo("es-ES");
            DateTime myDate = DateTime.Parse(date, culture);

            var showtimes = _context.Showtimes
              .Where(s => s.StartTime.Date == myDate && s.StartTime >= DateTime.Now)
              .OrderBy(s => s.FormatId)
              .Select(s => new
              {
                  s.Id,
                  showtimes = s.format.Name + " - " + s.StartTime.ToString("H:mm")
              })
              .ToList();

            return Json(showtimes);
        }

        [HttpGet]
        [Route("Tickets/GetPrice")]
        public async Task<JsonResult> GetPrice(int ShowtimesId, string SeatPosition)
        {
            //Ticket ticket = new Ticket();

            var newShowtimes = await _context.Showtimes
                .Include(s => s.format)
                .Include(s => s.movie)
                .Include(s => s.room)
                .FirstOrDefaultAsync(s => s.Id == ShowtimesId);

            var SeatId = await _context.Seat.Where(s => s.RoomId == newShowtimes.RoomId && s.Position.Equals(SeatPosition)).Select(s => s.Id).FirstOrDefaultAsync();

            var newSeat = await _context.Seat
                .Include(s => s.seatType)
                //.Include(s => s.room)
                //    .ThenInclude(r => r.cinema)
                .FirstOrDefaultAsync(s => s.Id == SeatId);

            var ticketPrice = await _context.Seat
                    .Include(s => s.seatType)
                    .Where(s => s.Id == SeatId).Select(s => s.seatType.Price).FirstOrDefaultAsync();

            var Price = 0;
            if (ticketPrice != null && ticketPrice != 0)
            {
                Price = ticketPrice;
            }
            else
            {
                Price = newShowtimes.Price;
                var seatTypeSurcharges = await _context.Surcharge.Where(c => c.SeatTypeId == newSeat.SeatTypeId).Select(c => c.Value).FirstOrDefaultAsync();
                var roomSurcharges = await _context.Surcharge.Where(c => c.RoomId == newShowtimes.RoomId).Select(c => c.Value).FirstOrDefaultAsync();
                var formatMovieSurcharges = await _context.Surcharge.Where(c => c.FormatId == newShowtimes.FormatId).Select(c => c.Value).FirstOrDefaultAsync();
                if (seatTypeSurcharges != null)
                {
                    Price += seatTypeSurcharges;
                }
                if (roomSurcharges != null)
                {
                    Price += roomSurcharges;
                }
                if (formatMovieSurcharges != null)
                {
                    Price += formatMovieSurcharges;
                }
            }
            return Json(Price);
        }




    }
}
