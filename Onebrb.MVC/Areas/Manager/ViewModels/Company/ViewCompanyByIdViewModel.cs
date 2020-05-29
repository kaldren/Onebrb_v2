using Onebrb.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onebrb.MVC.Areas.Manager.ViewModels.Company
{
    public class ViewCompanyByIdViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public bool IsDisabled { get; set; }
        public string UserName { get; set; }
        public int JobsCount { get; set; }
        public bool IsManager { get; set; }
    }
}
