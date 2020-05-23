using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Onebrb.MVC.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class CompanyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}