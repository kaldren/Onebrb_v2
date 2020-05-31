using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Onebrb.MVC.Areas.Manager.Dtos.Job;
using Onebrb.MVC.Areas.Manager.Models;
using Onebrb.MVC.Areas.Manager.ViewModels.Job;
using Onebrb.MVC.Data;
using Onebrb.MVC.Models;
using Onebrb.MVC.Settings;
using shortid;

namespace Onebrb.MVC.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Route("[controller]/[action]/{id?}")]
    [Authorize]
    public class JobController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly JobSettings _jobSettings;


        public JobController(ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IOptions<JobSettings> jobSettings)
        {
            _db = db;
            _userManager = userManager;
            _mapper = mapper;
            _jobSettings = jobSettings.Value;
        }
        /// <summary>
        /// Main page
        /// </summary>
        /// <returns></returns>
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
        [HttpGet]
        [ActionName("Company")]
        [AllowAnonymous]
        public async Task<IActionResult> ViewAllJobs(int id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var jobs = await _db.Jobs
                                .Where(x => x.CompanyId == id)
                                .Include(x => x.Company)
                                .ToListAsync();

            if (jobs == null || jobs.Count == 0)
            {
                return RedirectToAction(nameof(Create), new { id });
            }

            var viewModel = _mapper.Map<List<Job>, List<ViewAllJobsByCompanyVM>>(jobs);

            if (currentUser != null && viewModel != null)
            {
                if (currentUser.UserName == viewModel[0].ManagerUserName)
                {
                    viewModel = viewModel.Select(x => { x.IsManager = true; return x; }).ToList();
                }
            }

            return View(viewModel);
        }

        /// <summary>
        /// View single job offer by id
        /// </summary>
        /// <param name="id">Job id</param>
        /// <returns>The job offer</returns>
        [HttpGet]
        [ActionName("View")]
        [AllowAnonymous]
        public async Task<IActionResult> ViewSingleJob(string id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var job = await _db.Jobs
                            .Include(x => x.Company)
                            .ThenInclude(x => x.Manager)
                            .Include(x => x.ApplicationUserJob)
                            .FirstOrDefaultAsync(x => x.JobId == id);

            if (job == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = _mapper.Map<ViewSingleJobVM>(job);

            viewModel.IsManager = (currentUser != null && currentUser.UserName == job.Company.Manager.UserName) ? true : false;
            var hasApplied = job.ApplicationUserJob
                                .FirstOrDefault(x => x.ApplicationUserId == User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (hasApplied != null)
            {
                viewModel.AlreadyApplied = true;
            }

            return View("ViewSingleJob", viewModel);
        }

        /// <summary>
        /// View all applicants
        /// </summary>
        /// <param name="id">Job id</param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Company")]
        [HttpGet]
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

            // Select only the active applications
            var activeApplicants = new List<ApplicationUserJob>();
            activeApplicants = applicants.ApplicationUserJob.Where(x => x.Status == _jobSettings.ApplicationStatus.Active).ToList();
            applicants.ApplicationUserJob = activeApplicants;

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
                    Status = applicants.ApplicationUserJob.ElementAt(i).Status,
                    ApplicationId = applicants.ApplicationUserJob.ElementAt(i).ApplicationId
                });
            }

            return View(dto);
        }

        /// <summary>
        /// Create job post page. Only companies can open it.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Company")]
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm]Job job, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
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
        [HttpPost]
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

            var userAlreadyApplied = job.ApplicationUserJob
                                        .FirstOrDefault(x => x.ApplicationUser == currentUser);

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
                ApplicationId = ShortId.Generate(),
                Status = _jobSettings.ApplicationStatus.Active
            });

            job.Applications++;

            _db.Jobs.Update(job);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> Edit(string id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var job = await _db.Jobs.FirstOrDefaultAsync(x => x.JobId == id && x.Company.Manager == currentUser);

            if (job == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<EditJobOfferVM>(job);

            return View(viewModel);
        }
    }
}