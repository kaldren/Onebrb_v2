using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onebrb.MVC.Areas.Manager.Models;
using Onebrb.MVC.Data;
using Onebrb.MVC.Models;
using shortid;

namespace Onebrb.MVC.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class JobController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index(int id)
        {
            //var job = 

            return View();
        }

        /// <summary>
        /// View all jobs by company id
        /// </summary>
        /// <param name="id">Company id</param>
        /// <returns>Jobs list</returns>
        public async Task<IActionResult> View(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var jobs = await _db.Jobs
                                .Where(x => x.CompanyId == id)
                                .Include(x => x.Company)
                                .ToListAsync();

            if (jobs == null || jobs.Count == 0)
            {
                return RedirectToAction(nameof(Create), new { id });
            }

            return View(jobs);
        }

        /// <summary>
        /// View all jobs by company id
        /// </summary>
        /// <param name="id">Company id</param>
        /// <returns>Jobs</returns>
        public async Task<IActionResult> ViewAll(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var jobs = await _db.Jobs.Where(x => x.CompanyId == id).ToListAsync();

            if (jobs == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(jobs);
        }

        [HttpGet]
        public async Task<IActionResult> Create([FromRoute]int id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var company = await _db.Companies.FirstOrDefaultAsync(x => x.Id == id && x.Manager == currentUser);

            if (company == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(new Job { Company = company, CompanyId = company.Id });
        }

        /// <summary>
        /// Create new job post
        /// </summary>
        /// <param name="job">The Job</param>
        /// <param name="id">Company id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]Job job, [FromRoute] int id)
        {
            job.CompanyId = id;
            job.JobId = ShortId.Generate();

            if (ModelState.IsValid == false)
            {
                return RedirectToAction(nameof(Index));
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var company = await _db.Companies.FirstOrDefaultAsync(x => x.Id == job.CompanyId && x.Manager == currentUser);

            if (company == null)
            {
                return RedirectToAction(nameof(Index));
            }

            job.DatePosted = DateTime.UtcNow;
            job.Company = company;

            await _db.Jobs.AddAsync(job);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}