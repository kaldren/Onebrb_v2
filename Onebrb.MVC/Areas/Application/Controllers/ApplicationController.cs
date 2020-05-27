using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onebrb.MVC.Areas.Application.Dtos;
using Onebrb.MVC.Data;
using Onebrb.MVC.Models;

namespace Onebrb.MVC.Areas.Application.Controllers
{
    [Area("Application")]
    [Route("[controller]/[action]")]
    public class ApplicationController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{id:minlength(2)}")]
        public IActionResult View(Guid id)
        {
            //var application = _db.Jobs
            //                    .Include(x => x.ApplicationUserJob)
            //                    .ThenInclude(x => x.)

            return View();
        }

        [HttpGet("{id:minlength(2)}")]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var application = await _db.AllApplications
                                    .Include(x => x.ApplicationUser)
                                    .Include(x => x.Job)
                                    .ThenInclude(x => x.Company)
                                    .ThenInclude(x => x.Manager)
                                    .FirstOrDefaultAsync(x => x.ApplicationId == id);

            // This application doesn't exist
            if (application == null)
            {
                return NotFound();
            }

            var job = await _db.Jobs.FirstOrDefaultAsync(x => x.JobId == application.JobId);

            // This job opening doesn't exist
            if (job == null)
            {
                return NotFound();
            }
            // Only the person who created the job opening can cancel applications
            if (application.Job.Company.Manager != currentUser)
            {
                return BadRequest();
            }

            var dto = new ViewApplicationDto
            {
                FirstName = application.ApplicationUser.FirstName,
                LastName = application.ApplicationUser.LastName,
                UserName = application.ApplicationUser.UserName,
            };

            return View(dto);
        }

        [HttpPost("{id:minlength(2)}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var application = await _db.AllApplications
                                    .Include(x => x.Job.Company)
                                    .ThenInclude(x => x.Manager)
                                    .FirstOrDefaultAsync(x => x.ApplicationId == id);
            var job = await _db.Jobs.FirstOrDefaultAsync(x => x.JobId == application.JobId);

            // This application doesn't exist
            if (application == null)
            {
                return NotFound();
            }

            // This job opening doesn't exist
            if (job == null)
            {
                return NotFound();
            }

            // Only the person who created the job opening can cancel applications
            if (application.Job.Company.Manager != currentUser)
            {
                return BadRequest();
            }

            _db.AllApplications.Remove(application);
            await _db.SaveChangesAsync();

            return RedirectToAction("Applicants", "Job", new { id = job.JobId });
        }
    }
}