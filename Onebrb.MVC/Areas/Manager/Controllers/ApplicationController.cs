﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onebrb.MVC.Areas.Manager.Dtos.Application;
using Onebrb.MVC.Data;
using Onebrb.MVC.Models;

namespace Onebrb.MVC.Areas.Manager.Controllers
{
    [Area("Manager")]
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

        [HttpGet]
        public IActionResult View(Guid id)
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> Delete(string id)
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
                Id = application.ApplicationId,
                FirstName = application.ApplicationUser.FirstName,
                LastName = application.ApplicationUser.LastName,
                UserName = application.ApplicationUser.UserName,
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteApplication(string id)
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