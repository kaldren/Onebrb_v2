using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onebrb.MVC.Areas.Message.Models;
using Onebrb.MVC.Areas.Message.ViewModels.Message;
using Onebrb.MVC.Data;
using Onebrb.MVC.Models;

namespace Onebrb.MVC.Areas.Message.Controllers
{
    [Area("Message")]
    [Route("[controller]/{action=Index}/{id?}")]
    [Authorize]
    public class MessageController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public MessageController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _db = db;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var messagesList = await _db.Messages.Where(x => x.Recipient == currentUser.UserName && !x.IsHiddenForRecipient).ToListAsync();

            var vm = _mapper.Map<List<ViewMessageVM>>(messagesList);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var recipient = await _db.Users.FirstOrDefaultAsync(x => x.UserName == id);

            if (currentUser == null || recipient == null)
            {
                return BadRequest();
            }

            var vm = new CreateMessageVM
            {
                Recipient = recipient.UserName,
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMessageVM vm)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var recipient = await _db.Users.FirstOrDefaultAsync(x => x.UserName == vm.Recipient);
            
            if (currentUser == null || recipient == null)
            {
                return BadRequest();
            }

            var message = _mapper.Map<Models.Message>(vm);

            message.Author = currentUser.UserName;

            await _db.Messages.AddAsync(message);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            // Only remove messages that are created by the author and are not already hidden (deleted)
            var message = await _db.Messages.FirstOrDefaultAsync(x => x.Id == id);

            if (message == null)
            {
                return BadRequest();
            }

            if (message.Author == currentUser.UserName)
            {
                // Remove the message for the author's perspective
                message.IsHiddenForAuthor = true;
            }
            else if (message.Recipient == currentUser.UserName)
            {
                // Remove the message for the recipient's perspective
                message.IsHiddenForRecipient = true;
            }

            _db.Messages.Update(message);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
