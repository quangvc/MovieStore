using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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
    public class FormatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FormatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Formats
        public async Task<IActionResult> Index()
        {
              return _context.Format != null ? 
                          View(await _context.Format.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Format'  is null.");
        }

        // GET: Formats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Format == null)
            {
                return NotFound();
            }

            var format = await _context.Format
                .FirstOrDefaultAsync(m => m.Id == id);
            if (format == null)
            {
                return NotFound();
            }

            return View(format);
        }

        // GET: Formats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Formats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Format format)
        {
            if (ModelState.IsValid)
            {
                Format ct = await _context.Format.FirstOrDefaultAsync(c => c.Name.Equals(format.Name));
                if (ct != null)
                {
                    return Content("This Format has already been taken.");
                }

                _context.Add(format);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(format);
        }

        // GET: Formats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Format == null)
            {
                return NotFound();
            }

            var format = await _context.Format.FindAsync(id);
            if (format == null)
            {
                return NotFound();
            }
            return View(format);
        }

        // POST: Formats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Format format)
        {
            if (id != format.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Format ct = await _context.Format.FirstOrDefaultAsync(c => c.Id != format.Id && c.Name.Equals(format.Name));
                    if (ct != null)
                    {
                        return Content("This Format has already been taken.");
                    }

                    _context.Update(format);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FormatExists(format.Id))
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
            return View(format);
        }

        // GET: Formats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Format == null)
            {
                return NotFound();
            }

            var format = await _context.Format
                .FirstOrDefaultAsync(m => m.Id == id);
            if (format == null)
            {
                return NotFound();
            }

            return View(format);
        }

        // POST: Formats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Format == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Format'  is null.");
            }
            var format = await _context.Format.FindAsync(id);
            if (format != null)
            {
                _context.Format.Remove(format);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FormatExists(int id)
        {
          return (_context.Format?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
