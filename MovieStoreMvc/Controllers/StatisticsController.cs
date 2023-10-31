using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieStoreMvc.Data;

namespace MovieStoreMvc.Controllers
{
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
        public JsonResult GetStatisticAsync(int? startDate)
        {
            var data = _context.Movie.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Revenue = _context.Ticket.Include(t => t.showtimes).Where(t => t.showtimes.MovieId == x.Id).Select(t => t.Price).Sum(),
            });

            return Json(data.ToList());
        }









    }
}
