using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onebrb.MVC.Data;
using Onebrb.MVC.Models;

namespace Onebrb.MVC.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
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
        /// View individual job by id
        /// </summary>
        /// <param name="id">Job id</param>
        /// <returns>Job</returns>
        public async Task<IActionResult> View(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var job = await _db.Jobs.FirstOrDefaultAsync(x => x.Id == id);

            if (job == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(job);
        }
    }
}