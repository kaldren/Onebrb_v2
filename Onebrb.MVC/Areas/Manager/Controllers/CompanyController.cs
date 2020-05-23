using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Onebrb.MVC.Areas.Manager.Models;
using Onebrb.MVC.Data;
using Onebrb.MVC.Models;

namespace Onebrb.MVC.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Company")]
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompanyController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Check if the user has any companies created
            var user = await _userManager.GetUserAsync(HttpContext.User);
            List<Company> companiesList = new List<Company>();

            companiesList = _db.Companies.Where(c => c.Manager == user).ToList();

            return View(companiesList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Company company)
        {
            if (ModelState.IsValid)
            {
                // Add company manager
                var manager = await _userManager.GetUserAsync(HttpContext.User);
                company.Manager = manager;
                await _db.Companies.AddAsync(company);
                await _db.SaveChangesAsync();
            }

            return View(nameof(Index));
        }
    }
}