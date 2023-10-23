using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieStoreMvc.Data;
using MovieStoreMvc.Models;
using MovieStoreMvc.Models.CinemaViewModel;
using Newtonsoft.Json.Linq;

namespace MovieStoreMvc.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MoviesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Movie
                .Include(m => m.manufacturer)
                .Include(m => m.rating)
                .Include(m => m.genres)
                .Include(m => m.formats)
                .Include(m => m.countries)
                .OrderByDescending(m => m.ReleaseDate)
                .AsNoTracking();
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .Include(m => m.manufacturer)
                .Include(m => m.rating)
                .Include(m => m.genres)
                .Include(m => m.formats)
                .Include(m => m.countries)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            // truyền data sang view
            PopulateManufacturersDropDownList();
            PopulateRatingsDropDownList();

            var movie = new Movie();
            movie.genres = new List<Genre>();
            movie.formats = new List<Format>();
            movie.countries = new List<Country>();

            PopulateCheckedGenreData(movie);
            PopulateCheckedFormatData(movie);
            PopulateCheckedCountryData(movie);

            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Poster,Director,Cast,ReleaseDate,EndDate,RunningTime,Language,Trailer," +
            "ManuId,RatingId")] Movie movie, string[] selectedGenres, string[] selectedFormats, string[] selectedCountries)
        {
            if (ModelState.IsValid)
            {
                if (selectedGenres.Length > 0)
                {
                    movie.genres = new List<Genre>();
                    // Load collection with one DB call.
                    _context.Genre.Load();
                }

                // Add selected Courses courses to the new instructor.
                foreach (var genre in selectedGenres)
                {
                    var foundGenre = await _context.Genre.FindAsync(int.Parse(genre));
                    if (foundGenre != null)
                    {
                        movie.genres.Add(foundGenre);
                    }
                    else
                    {
                        return Content("Genre not found!", genre);
                    }
                }

                if (selectedFormats.Length > 0)
                {
                    movie.formats = new List<Format>();
                    // Load collection with one DB call.
                    _context.Format.Load();
                }

                // Add selected Courses courses to the new instructor.
                foreach (var format in selectedFormats)
                {
                    var foundFormat = await _context.Format.FindAsync(int.Parse(format));
                    if (foundFormat != null)
                    {
                        movie.formats.Add(foundFormat);
                    }
                    else
                    {
                        return Content("Format not found!", format);
                    }
                }

                if (selectedCountries.Length > 0)
                {
                    movie.countries = new List<Country>();
                    // Load collection with one DB call.
                    _context.Country.Load();
                }

                // Add selected Courses courses to the new instructor.
                foreach (var country in selectedCountries)
                {
                    var foundCountry = await _context.Country.FindAsync(int.Parse(country));
                    if (foundCountry != null)
                    {
                        movie.countries.Add(foundCountry);
                    }
                    else
                    {
                        return Content("Country not found!", country);
                    }
                }

                if (movie.Poster != null && movie.Poster.Length > 0)
                {
                    string extension = Path.GetExtension(movie.Poster.FileName);
                    string[] exts = new string[] { ".jpg", ".jpeg", ".png" };
                    int pos = Array.IndexOf(exts, extension);

                    if (pos <= -1)
                        return Content(content: "Ảnh không đúng định dạng!");

                    // Generate a unique filename for the poster / tạo tên
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + movie.Poster.FileName;

                    // Set the path where the poster will be saved / thiết lập đường dẫn
                    string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/posters");

                    // Ensure the upload directory exists / đảm bảo đường dẫn tồn tại
                    Directory.CreateDirectory(uploadPath);

                    // Combine the path and filename / kết hợp đường dẫn và file
                    string filePath = Path.Combine(uploadPath, uniqueFileName);

                    // Save the file to the server / lưu file
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await movie.Poster.CopyToAsync(fileStream);
                    }

                    // Set the poster path in the Movie object
                    movie.PosterUrl = uniqueFileName;
                }
                else
                {
                    return Content("The Poster field is required.");
                }

                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["ManuId"] = new SelectList(_context.Manufacturer, "Id", "Name", movie.ManuId);
            //ViewData["RatingId"] = new SelectList(_context.Rating, "Id", "Name", movie.RatingId);

            //trong trường hợp lỗi, trả lại các mục đã chọn
            PopulateManufacturersDropDownList(movie.ManuId);
            PopulateRatingsDropDownList(movie.RatingId);

            PopulateCheckedGenreData(movie);
            PopulateCheckedFormatData(movie);
            PopulateCheckedCountryData(movie);

            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .Include(i => i.genres)
                .Include(i => i.formats)
                .Include(i => i.countries)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            //ViewData["ManuId"] = new SelectList(_context.Manufacturer, "Id", "Name", movie.ManuId);
            //ViewData["RatingId"] = new SelectList(_context.Rating, "Id", "Name", movie.RatingId);
            //ViewData["Genres"] = _context.Genre.AsNoTracking().ToList();
            //ViewData["Formats"] = _context.Format.AsNoTracking().ToList();
            //ViewData["Countries"] = _context.Country.AsNoTracking().ToList();

            PopulateManufacturersDropDownList(movie.ManuId);
            PopulateRatingsDropDownList(movie.RatingId);
            PopulateCheckedGenreData(movie);
            PopulateCheckedFormatData(movie);
            PopulateCheckedCountryData(movie);

            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedGenres, string[] selectedFormats, string[] selectedCountries)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieToUpdate = await _context.Movie
                .Include(i => i.genres)
                .Include(i => i.formats)
                .Include(i => i.countries)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (await TryUpdateModelAsync<Movie>(
            movieToUpdate,
            "",
            i => i.Title, i => i.Description, i => i.Poster, i => i.Director, i => i.Cast, i => i.ReleaseDate, i => i.EndDate, i => i.RunningTime,
            i => i.Language, i => i.Trailer, i => i.ManuId, i => i.RatingId))
            {

                if (movieToUpdate.Poster != null && movieToUpdate.Poster.Length > 0)
                {
                    string extension = Path.GetExtension(movieToUpdate.Poster.FileName);
                    string[] exts = new string[] { ".jpg", ".jpeg", ".png" };
                    int pos = Array.IndexOf(exts, extension);

                    if (pos <= -1)
                        return Content(content: "Ảnh không đúng định dạng!");

                    // Xóa ảnh cũ
                    if (!string.IsNullOrEmpty(movieToUpdate.PosterUrl))
                    {
                        string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/posters", movieToUpdate.PosterUrl);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Generate a unique filename for the poster / tạo tên
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + movieToUpdate.Poster.FileName;

                    // Set the path where the poster will be saved / thiết lập đường dẫn
                    string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/posters");

                    // Ensure the upload directory exists / đảm bảo đường dẫn tồn tại
                    Directory.CreateDirectory(uploadPath);

                    // Combine the path and filename / kết hợp đường dẫn và file
                    string filePath = Path.Combine(uploadPath, uniqueFileName);

                    // Save the file to the server / lưu file
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await movieToUpdate.Poster.CopyToAsync(fileStream);
                    }

                    // Set the poster path in the Movie object
                    movieToUpdate.PosterUrl = uniqueFileName;

                }

                UpdateMovieGenres(selectedGenres, movieToUpdate);
                UpdateMovieFormats(selectedFormats, movieToUpdate);
                UpdateMovieCountries(selectedCountries, movieToUpdate);

                try
                {
                    var result = await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateMovieGenres(selectedGenres, movieToUpdate);
            PopulateCheckedGenreData(movieToUpdate);
            return View(movieToUpdate);

        }

        public async void UpdateMovieGenres(string[] selectedGenres,
                                            Movie movieToUpdate)
        {
            if (selectedGenres == null)
            {
                movieToUpdate.genres = new List<Genre>();
                return;
            }

            var selectedGenresHS = new HashSet<string>(selectedGenres);
            var movieGenres = new HashSet<int>
                (movieToUpdate.genres.Select(c => c.Id));
            foreach (var genre in _context.Genre)
            {
                if (selectedGenresHS.Contains(genre.Id.ToString()))
                {
                    if (!movieGenres.Contains(genre.Id))
                    {
                        movieToUpdate.genres.Add(genre);
                    }
                }
                else
                {
                    if (movieGenres.Contains(genre.Id))
                    {
                        var movieToRemove = movieToUpdate.genres.Single(
                                                        c => c.Id == genre.Id);
                        movieToUpdate.genres.Remove(movieToRemove);
                    }
                }
            }
        }

        public void UpdateMovieFormats(string[] selectedFormats,
                                            Movie movieToUpdate)
        {
            if (selectedFormats == null)
            {
                movieToUpdate.formats = new List<Format>();
                return;
            }

            var selectedFormatsHS = new HashSet<string>(selectedFormats);
            var movieFormats = new HashSet<int>
                (movieToUpdate.formats.Select(c => c.Id));
            foreach (var format in _context.Format)
            {
                if (selectedFormatsHS.Contains(format.Id.ToString()))
                {
                    if (!movieFormats.Contains(format.Id))
                    {
                        movieToUpdate.formats.Add(format);
                    }
                }
                else
                {
                    if (movieFormats.Contains(format.Id))
                    {
                        var movieToRemove = movieToUpdate.formats.Single(
                                                        c => c.Id == format.Id);
                        movieToUpdate.formats.Remove(movieToRemove);
                    }
                }
            }
        }

        public void UpdateMovieCountries(string[] selectedCountries,
                                            Movie movieToUpdate)
        {
            if (selectedCountries == null)
            {
                movieToUpdate.countries = new List<Country>();
                return;
            }

            var selectedCountriesHS = new HashSet<string>(selectedCountries);
            var movieCountries = new HashSet<int>
                (movieToUpdate.countries.Select(c => c.Id));
            foreach (var country in _context.Country)
            {
                if (selectedCountriesHS.Contains(country.Id.ToString()))
                {
                    if (!movieCountries.Contains(country.Id))
                    {
                        movieToUpdate.countries.Add(country);
                    }
                }
                else
                {
                    if (movieCountries.Contains(country.Id))
                    {
                        var movieToRemove = movieToUpdate.countries.Single(
                                                        c => c.Id == country.Id);
                        movieToUpdate.countries.Remove(movieToRemove);
                    }
                }
            }
        }


        private void PopulateManufacturersDropDownList(object selectedManufacturer = null)
        {
            var manufacturersQuery = from d in _context.Manufacturer
                                     orderby d.Name
                                     select d;
            ViewBag.ManuId = new SelectList(manufacturersQuery.AsNoTracking(), "Id", "Name", selectedManufacturer);
        }
        private void PopulateRatingsDropDownList(object selectedRating = null)
        {
            var ratingsQuery = from d in _context.Rating
                               orderby d.Name
                               select d;
            ViewBag.RatingId = new SelectList(ratingsQuery.AsNoTracking(), "Id", "Name", selectedRating);
        }

        private void PopulateCheckedGenreData(Movie movie)
        {
            var allGenres = _context.Genre;
            var movieGenres = new HashSet<int>(movie.genres.Select(c => c.Id));
            var viewModel = new List<MovieGenreData>();
            foreach (var genre in allGenres)
            {
                viewModel.Add(new MovieGenreData
                {
                    GenreId = genre.Id,
                    Name = genre.Name,
                    Checked = movieGenres.Contains(genre.Id)
                });
            }
            ViewData["Genres"] = viewModel;
        }
        private void PopulateCheckedFormatData(Movie movie)
        {
            var allFormats = _context.Format;
            var movieFormats = new HashSet<int>(movie.formats.Select(c => c.Id));
            var viewModel = new List<MovieFormatData>();
            foreach (var format in allFormats)
            {
                viewModel.Add(new MovieFormatData
                {
                    FormatId = format.Id,
                    Name = format.Name,
                    Checked = movieFormats.Contains(format.Id)
                });
            }
            ViewData["Formats"] = viewModel;
        }

        private void PopulateCheckedCountryData(Movie movie)
        {
            var allCountries = _context.Country;
            var movieCountries = new HashSet<int>(movie.countries.Select(c => c.Id));
            var viewModel = new List<MovieCountryData>();
            foreach (var country in allCountries)
            {
                viewModel.Add(new MovieCountryData
                {
                    CountryId = country.Id,
                    Name = country.Name,
                    Checked = movieCountries.Contains(country.Id)
                });
            }
            ViewData["Countries"] = viewModel;
        }






















        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .Include(m => m.manufacturer)
                .Include(m => m.rating)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movie == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Movie'  is null.");
            }
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return (_context.Movie?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
