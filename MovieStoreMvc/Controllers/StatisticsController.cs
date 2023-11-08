using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieStoreMvc.Data;
using System;

namespace MovieStoreMvc.Controllers
{
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
                    Date = t.Key.Date,
                    Revenue = t.Sum(s => s.Price)
                });

            return Json(data.ToList());
        }



    }
}
