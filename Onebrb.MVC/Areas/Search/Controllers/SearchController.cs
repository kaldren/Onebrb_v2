using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onebrb.MVC.Areas.Search.ViewModels;
using Onebrb.MVC.Data;

namespace Onebrb.MVC.Areas.Search.Controllers
{
    [Area("Search")]
    [Route("[controller]/{action=Index}/{id?}")]
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public SearchController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Phrase(string text)
        {
            var results = await _db.Jobs
                                    .Include(x => x.Company)
                                    .Where(x => x.Description.Contains(text) || x.Title.Contains(text)).ToListAsync();

            var resultVm = _mapper.Map <List<SearchResultsVM>>(results);

            return View(resultVm);
        }
    }
}
