using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieStoreMvc.Constants;
using MovieStoreMvc.Data;
using MovieStoreMvc.Data.Migrations;
using MovieStoreMvc.Models;
using NuGet.Protocol;
using System.Security.Claims;

namespace MovieStoreMvc.Controllers
{
    //[Authorize(Roles ="Admin")]
    public class AppUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public AppUsersController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //GET: AppUser
        public async Task<IActionResult> Index()
        {
            ViewData["users"] = (
              from u in _context.Users
              select new
              {
                  Id = u.Id,
                  Email = u.Email,
                  Name = u.Name,
                  PhoneNumber = u.PhoneNumber,
                  Role =
                  string.Join(", ", (
                      from ur in _context.UserRoles
                      join r in _context.Roles on ur.RoleId equals r.Id
                      where ur.UserId == u.Id
                      select r.Name).ToArray())
              });

            return View();
        }


        // GET: AppUser/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            var role = await _userManager.GetRolesAsync(user);
            ViewData["Role"] = string.Join("", role.ToList());

            return View(user);
        }

        // GET: AppUser/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || _context.Showtimes == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var Role = (from ur in _context.UserRoles
                       join r in _context.Roles on ur.RoleId equals r.Id
                       where ur.UserId == id
                       select r.Name).ToList();
            ViewData["Roles"] = new SelectList(_context.Roles, "Name", "Name", string.Join("", Role));
            return View(user);
        }

        // POST: AppUser/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string? id, string? Role)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);

            if (Role == "Admin")
            {
                await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            }
            else if (Role == "Employee")
            {
                await _userManager.AddToRoleAsync(user, Roles.Employee.ToString());
            }
            else
            {
                await _userManager.AddToRoleAsync(user, Roles.User.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: AppUser/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            var role = await _userManager.GetRolesAsync(user);
            ViewData["Role"] = string.Join("", role.ToList());

            return View(user);
        }

        // POST: AppUser/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string? id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
