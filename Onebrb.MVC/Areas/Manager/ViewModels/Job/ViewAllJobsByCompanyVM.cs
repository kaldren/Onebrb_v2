using Onebrb.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onebrb.MVC.Areas.Manager.ViewModels.Job
{
    public class ViewAllJobsByCompanyVM
    {
        public string JobId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Applications { get; set; }
        public DateTime DatePosted { get; set; }
        public bool IsDisabled { get; set; }
        public int CompanyId { get; set; }
        public string ManagerUserName { get; set; }
        public bool IsManager { get; set; }
        public string CompanyName { get; set; }
    }
}
