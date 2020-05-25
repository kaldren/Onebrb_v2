using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onebrb.MVC.Areas.Profile.Dtos;
using Onebrb.MVC.Data;
using Onebrb.MVC.Models;

namespace Onebrb.MVC.Areas.Profile.Controllers
{
    [Area("Profile")]
    [Route("[controller]/[action]")]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{username:minlength(2)}")]
        public new async Task<IActionResult> View(string username)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.UserName == username);

            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var dto = new ViewProfileDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName
            };

            return View(dto);
        }
    }
}