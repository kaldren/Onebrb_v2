using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Onebrb.MVC.Areas.Search.Controllers
{
    [Area("Search")]
    [Route("[controller]/{action=Index}/{id?}")]
    public class SearchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
