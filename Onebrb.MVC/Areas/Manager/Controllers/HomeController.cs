using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Onebrb.MVC.Data;
using Onebrb.MVC.Models;

namespace Onebrb.MVC.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Company")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Check if the user has any companies created
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var hasCompanies = _db.Companies.Where(c => c.Manager == user).ToList();

            return View(hasCompanies);
        }

        [HttpPost]
        public IActionResult Create()
        {
            return View();
        }
    }
}