using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieStoreMvc.Data;
using MovieStoreMvc.Models;
using System;

namespace MovieStoreMvc.Controllers
{
    public class MovieSeatCoverage
    {
        public string Movie { get; set; }
        public int? TotalTickets { get; set; }
        public int TotalSeats { get; set; }
        public List<int> CountSeats { get; set; }
        public double Ratio { get; set; }
    }
    [Authorize(Roles = "Admin, Employee")]
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index2()
        {
            return View();
        }

        public IActionResult test()
        {
            return View();
        }

        public IActionResult test2()
        {
            return View();
        }

        [HttpGet]
        [Route("Statistics/RevenueByMovie")]
        public JsonResult GetStatisticAsync()
        {
            var data = _context.Movie.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Revenue = _context.Ticket.Include(t => t.showtimes).Where(t => t.showtimes.MovieId == x.Id).Select(t => t.Price).Sum(),
            });

            return Json(data.ToList());
        }

        [HttpGet]
        [Route("Statistics/RevenueByYear")]
        public JsonResult RevenueByYearAsync()
        {
            var data = _context.Ticket.Include(t => t.showtimes).GroupBy(t => t.CreatedDate.Year).Select(t => new
            {
                Year = t.Key,
                Revenue = t.Sum(s => s.Price)
            });

            return Json(data.ToList());
        }

        [HttpGet]
        [Route("Statistics/FromDateToDate")]
        public JsonResult RevenueFromDateToDateAsync(DateTime startDate, DateTime endDate)
        {
            
            var data = _context.Ticket.Include(t => t.showtimes)
                .Where(t => t.showtimes.StartTime.Date >= startDate && t.showtimes.StartTime.Date <= endDate)
                .GroupBy(t => t.CreatedDate.Date).Select(t => new
                {
                    Date = t.Key.ToString("dd-MM-yyyy"),
                    Revenue = t.Sum(s => s.Price)
                });

            return Json(data.ToList());
        }

        [HttpGet]
        [Route("Statistics/SeatCoverageRatio")]
        public JsonResult SeatCoverageRatio()
        {
            var data = _context.Movie
                .Select(m => new MovieSeatCoverage
                {
                    Movie = m.Title,
                    TotalTickets = _context.Ticket.Include(t => t.showtimes).Where(t => t.showtimes.MovieId == m.Id).Count(),
                    CountSeats = _context.Showtimes.Include(s => s.movie).Where(s => s.MovieId == m.Id)
                        .Select(s => s.room.Seats.Count).ToList(),
                }).ToList();

            data.ForEach(t =>
            {
                t.TotalSeats = t.CountSeats.Sum();
                t.Ratio = t.TotalSeats == 0 ? 0 : (double)t.TotalTickets * 100 / t.TotalSeats;
            });

            return Json(data);
        }

        [HttpGet]
        [Route("Statistics/SeatCoverageRatioByCinema")]
        public JsonResult SeatCoverageRatioByCinema()
        {
            var data = _context.Cinema
                .Select(c => new
                {
                    Cinema = c.Name,
                    Movies = _context.Movie.Select(m => new MovieSeatCoverage
                    {
                        Movie = m.Title,
                        TotalTickets = _context.Ticket.Include(t => t.showtimes)
                                                        .Include(t => t.seat)
                                                            .ThenInclude(s => s.room)
                                                            .ThenInclude(r => r.cinema)
                                                        .Where(t => t.showtimes.MovieId == m.Id && t.seat.room.CinemaId == c.Id).Count(),
                        CountSeats = _context.Showtimes.Include(s => s.movie)
                                                            .Include(s => s.room)
                                                                .ThenInclude(r => r.cinema)
                                                            .Where(s => s.MovieId == m.Id && s.room.CinemaId == c.Id)
                                    .Select(s => s.room.Seats.Count).ToList(),
                    }).ToList(),
                }).ToList();

            data.ForEach(t =>
            {
                t.Movies.ForEach(m =>
                {
                    m.TotalSeats = m.CountSeats.Sum();
                    m.Ratio = m.TotalSeats == 0 ? 0 : (double)m.TotalTickets * 100 / m.TotalSeats;
                });
            });

            return Json(data);
        }



    }
}
