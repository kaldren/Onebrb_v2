using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onebrb.MVC.Areas.Manager.Dtos.Job;
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

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// View all jobs by company id
        /// </summary>
        /// <param name="id">Company id</param>
        /// <returns>Jobs list</returns>
        /// 
        [HttpGet("[Controller]/View/{id:int}")]
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
        /// View single job offer by id
        /// </summary>
        /// <param name="id">Job id</param>
        /// <returns>The job offer</returns>
        /// 
        [HttpGet("[Controller]/View/{id:alpha}")]
        public new async Task<IActionResult> View(string id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var job = await _db.Jobs
                            .Include(x => x.Company)
                            .ThenInclude(x => x.Manager)
                            .Include(x => x.ApplicationUserJob)
                            .FirstOrDefaultAsync(x => x.JobId == id);

            if (job == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View("ViewOneJob", job);
        }

        /// <summary>
        /// View all applicants
        /// </summary>
        /// <param name="id">Job id</param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Company")]
        [HttpGet("[Controller]/Applicants/{id:alpha}")]
        public async Task<IActionResult> Applicants(string id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var applicants = await _db.Jobs
                                    .Include(x => x.ApplicationUserJob)
                                    .Include(x => x.Company)
                                    .Where(x => x.JobId == id && x.Company.Manager.Id == currentUser.Id)
                                    .FirstOrDefaultAsync();

            if (applicants == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var dto = new List<JobApplicantsListDto>();

            for (int i = 0; i < applicants.ApplicationUserJob.Count; i++)
            {
                var userId = applicants.ApplicationUserJob.ElementAt(i).ApplicationUserId;
                dto.Add(new JobApplicantsListDto
                {
                    UserId = userId,
                    UserName = _userManager.Users.Single(x => x.Id == userId).UserName,
                    FirstName = _userManager.Users.Single(x => x.Id == userId).FirstName,
                    LastName = _userManager.Users.Single(x => x.Id == userId).LastName,
                    ApplicationId = applicants.ApplicationUserJob.ElementAt(i).ApplicationId
                });
            }

            return View(dto);
        }

        [HttpGet("[Controller]/Create/{id:int}")]
        public async Task<IActionResult> Create(int id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var company = await _db.Companies
                                .Include(x => x.Jobs)
                                .FirstOrDefaultAsync(x => x.Id == id && x.Manager == currentUser);

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
        [HttpPost("[Controller]/Create/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm]Job job, [FromRoute] int id)
        {
            if (ModelState.IsValid == false)
            {
                return RedirectToAction(nameof(Index));
            }

            job.CompanyId = id;
            job.JobId = ShortId.Generate(false, false);

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

            return RedirectToAction(nameof(View), new { id });
        }
        
        /// <summary>
        /// Only employee type user allowed to apply for jobs
        /// </summary>
        /// <param name="id">Job id</param>
        /// <returns></returns>
        [Authorize(Roles = "Employee")]
        [HttpPost("{id:alpha}")]
        public async Task<IActionResult> Apply(string id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var job = await _db.Jobs
                            .Include(x => x.Company)
                            .Include(x => x.ApplicationUserJob)
                            .FirstOrDefaultAsync(x => x.JobId == id);

            if (job == null)
            {
                return RedirectToAction(nameof(Index));
            }

            // You can't apply for your own job postings
            if (job.Company.Manager == currentUser)
            {
                return RedirectToAction(nameof(Index));
            }

            var userAlreadyApplied = job.ApplicationUserJob.FirstOrDefault(x => x.ApplicationUser == currentUser);

            // This user already applied for this job
            if (userAlreadyApplied != null)
            {
                return RedirectToAction(nameof(Index));
            }

            job.ApplicationUserJob.Add(new ApplicationUserJob
            {
                ApplicationUser = currentUser,
                ApplicationUserId = currentUser.Id,
                Job = job,
                JobId = job.JobId,
                ApplicationId = ShortId.Generate()
            });

            job.Applications++;

            _db.Jobs.Update(job);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}