using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onebrb.MVC.Areas.Manager.Dtos.Company;
using Onebrb.MVC.Areas.Manager.Models;
using Onebrb.MVC.Areas.Manager.ViewModels.Company;
using Onebrb.MVC.Data;
using Onebrb.MVC.Models;

namespace Onebrb.MVC.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Company")]
    [Route("[controller]/[action]/{id?}")]
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
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var companiesList = _db.Companies
                .Include(x => x.Jobs)
                .Include(x =>x.Manager)
                .Where(c => c.Manager == currentUser).ToList();

            var companiesViewModel = new List<ViewCompanyByIdViewModel>();

            for (int i = 0; i < companiesList.Count(); i++)
            {
                companiesViewModel.Add(new ViewCompanyByIdViewModel{
                        Id = companiesList[i].Id,
                        Address = companiesList[i].Address,
                        Description = companiesList[i].Description,
                        IsDisabled = companiesList[i].IsDisabled,
                        JobsCount = companiesList[i].Jobs.Count(),
                        Name = companiesList[i].Name,
                        Url = companiesList[i].Url,
                        UserName = companiesList[i].Manager.UserName,
                        IsManager = (currentUser != null && currentUser.UserName == companiesList[i].Manager.UserName) ? true : false
                });
            }

            return View(companiesViewModel);
        }

        [HttpGet]
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

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var company = await _db.Companies.FirstOrDefaultAsync(x => x.Id == id && x.Manager == currentUser);

            if (company == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Company company)
        {
            if (company == null)
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid == false)
            {
                return RedirectToAction(nameof(Index));
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var dbCompany = await _db.Companies.FirstOrDefaultAsync(x => x.Id == company.Id && x.Manager == currentUser);

            if (dbCompany == null)
            {
                return RedirectToAction(nameof(Index));
            }

            // TODO: Use Automapper
            dbCompany.Name = company.Name;
            dbCompany.Address = company.Address;
            dbCompany.Url = company.Url;
            dbCompany.Description = company.Description;

            _db.Companies.Update(dbCompany);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var company = await _db.Companies.FirstOrDefaultAsync(x => x.Id == id && x.Manager == currentUser);

            if (company == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var company = await _db.Companies.FirstOrDefaultAsync(x => x.Id == id && x.Manager == currentUser);

            if (company != null)
            {
                _db.Companies.Remove(company);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// View company profile
        /// </summary>
        /// <param name="id">Company id</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> View(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var company = await _db.Companies
                                .Include(x => x.Jobs)
                                .Include(x => x.Manager)
                                .FirstOrDefaultAsync(x => x.Id == id);

            // Company doesn't exist
            if (company == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new ViewCompanyByIdViewModel
            {
                Id = company.Id,
                Address = company.Address,
                Description = company.Description,
                IsDisabled = company.IsDisabled,
                JobsCount = company.Jobs.Count(),
                Name = company.Name,
                Url = company.Url,
                UserName = company.Manager.UserName,
                IsManager = (currentUser != null && currentUser.UserName == company.Manager.UserName) ? true : false
            };

            return View(viewModel);
        }

        /// <summary>
        /// Disabled company page for not-manager users
        /// </summary>
        /// <param name="isDisabled"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Disabled(int id)
        {

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Disable(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var company = await _db.Companies.FirstOrDefaultAsync(x => x.Id == id && x.Manager == currentUser);

            if (company == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Disable(int id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var company = await _db.Companies.FirstOrDefaultAsync(x => x.Id == id && x.IsDisabled == false && x.Manager == currentUser);

            if (company == null)
            {
                return RedirectToAction(nameof(Index));
            }

            company.IsDisabled = true;
            _db.Companies.Update(company);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Enable(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var company = await _db.Companies.FirstOrDefaultAsync(x => x.Id == id && x.IsDisabled == true && x.Manager == currentUser);

            if (company == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enable(int id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var company = await _db.Companies.FirstOrDefaultAsync(x => x.Id == id && x.IsDisabled == true && x.Manager == currentUser);

            if (company == null)
            {
                return RedirectToAction(nameof(Index));
            }

            company.IsDisabled = false;
            _db.Companies.Update(company);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}