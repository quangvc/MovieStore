﻿using System;
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
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Ticket ticket, string ids, int showtimes)
        {
            if (ModelState.IsValid)
            {
                var arrPostitions = ids.Split(',');
                foreach (var seat in arrPostitions)
                {
                    Ticket newTicket = new Ticket();
                    newTicket.Description = ticket.Description;

                    newTicket.ShowtimesId = showtimes;
                    var newShowtimes = await _context.Showtimes
                        .Include(s => s.format)
                        .Include(s => s.movie)
                        .Include(s => s.room)
                        .FirstOrDefaultAsync(s => s.Id == newTicket.ShowtimesId);
                    newTicket.SeatId = await _context.Seat
                        .Where(s => s.RoomId == newShowtimes.RoomId && s.Position.Equals(seat))
                        .Select(s => s.Id)
                        .FirstOrDefaultAsync();
                    var newSeat = await _context.Seat
                        .Include(s => s.room)
                        .ThenInclude(r => r.cinema)
                        .FirstOrDefaultAsync(s => s.Id == newTicket.SeatId);
                    newTicket.CreatedDate = DateTime.Now;
                    newTicket.Name = newShowtimes.format.Name + " " + newShowtimes.movie.Title + " [" + newShowtimes.room.Name + "-" + seat + "]" + "\n" + newShowtimes.StartTime + "\n" + newSeat.room.cinema.Name;

                    var ticketPrice = await _context.Seat
                        .Include(s => s.seatType)
                        .Where(s => s.Id == newTicket.SeatId).Select(s => s.seatType.Price).FirstOrDefaultAsync();

                    newTicket.Price = 0;
                    if (ticketPrice != 0)
                    {
                        newTicket.Price = ticketPrice;
                    }
                    else
                    {
                        newTicket.Price = newShowtimes.Price;
                        var seatTypeSurcharges = await _context.Surcharge.Where(c => c.SeatTypeId == newSeat.SeatTypeId).Select(c => c.Value).FirstOrDefaultAsync();
                        var roomSurcharges = await _context.Surcharge.Where(c => c.RoomId == newShowtimes.RoomId).Select(c => c.Value).FirstOrDefaultAsync();
                        var formatMovieSurcharges = await _context.Surcharge.Where(c => c.FormatId == newShowtimes.FormatId).Select(c => c.Value).FirstOrDefaultAsync();
                        if (seatTypeSurcharges != 0)
                        {
                            newTicket.Price += seatTypeSurcharges;
                        }
                        if (roomSurcharges != 0)
                        {
                            newTicket.Price += roomSurcharges;
                        }
                        if (formatMovieSurcharges != 0)
                        {
                            newTicket.Price += formatMovieSurcharges;
                        }
                    }

                    _context.Add(newTicket);
                }
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
            var seats = _context.Seat.Include(s => s.seatType).Where(s => s.RoomId == RoomId).Select(s => new
            {
                Position = s.Position,
                IsOrder = _context.Ticket.Where(t => t.ShowtimesId == showtimesId && t.SeatId == s.Id).FirstOrDefault(),
                SeatType = s.seatType.Name
            });

            return Json(seats.ToList());
        }

        [HttpGet]
        [Route("Tickets/GetShowtimes")]
        public JsonResult GetTimesByDate(string date, int cinema, int movie)
        {
            CultureInfo culture = new CultureInfo("es-ES");
            DateTime myDate = DateTime.Parse(date, culture);

            var showtimes = _context.Showtimes
                .Include(s => s.room)
                    .ThenInclude(r => r.cinema)
              .Where(s => s.StartTime.Date == myDate && s.StartTime >= DateTime.Now && s.MovieId == movie && s.room.cinema.Id == cinema)
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
        public async Task<JsonResult> GetPrice(int ShowtimesId, string? SeatPositions)
        {
            var totalPrice = 0;
            var Ids = SeatPositions?.Split(',');

            if (Ids == null)
            {
                return Json(totalPrice);
            }

            var newShowtimes = await _context.Showtimes
                .Include(s => s.format)
                .Include(s => s.movie)
                .Include(s => s.room)
                .FirstOrDefaultAsync(s => s.Id == ShowtimesId);

            foreach (var SeatPosition in Ids)
            {

                var SeatId = await _context.Seat.Where(s => s.RoomId == newShowtimes.RoomId && s.Position.Equals(SeatPosition)).Select(s => s.Id).FirstOrDefaultAsync();

                var newSeat = await _context.Seat
                    .Include(s => s.seatType)
                    .FirstOrDefaultAsync(s => s.Id == SeatId);

                var ticketPrice = await _context.Seat
                        .Include(s => s.seatType)
                        .Where(s => s.Id == SeatId).Select(s => s.seatType.Price).FirstOrDefaultAsync();

                var Price = 0;
                if (ticketPrice != 0)
                {
                    Price = ticketPrice;
                }
                else
                {
                    Price = newShowtimes.Price;
                    var seatTypeSurcharges = await _context.Surcharge.Where(c => c.SeatTypeId == newSeat.SeatTypeId).Select(c => c.Value).FirstOrDefaultAsync();
                    var roomSurcharges = await _context.Surcharge.Where(c => c.RoomId == newShowtimes.RoomId).Select(c => c.Value).FirstOrDefaultAsync();
                    var formatMovieSurcharges = await _context.Surcharge.Where(c => c.FormatId == newShowtimes.FormatId).Select(c => c.Value).FirstOrDefaultAsync();
                    if (seatTypeSurcharges != 0)
                    {
                        Price += seatTypeSurcharges;
                    }
                    if (roomSurcharges != 0)
                    {
                        Price += roomSurcharges;
                    }
                    if (formatMovieSurcharges != 0)
                    {
                        Price += formatMovieSurcharges;
                    }
                }

                totalPrice += Price;
            }
            return Json(totalPrice);
        }




    }
}
