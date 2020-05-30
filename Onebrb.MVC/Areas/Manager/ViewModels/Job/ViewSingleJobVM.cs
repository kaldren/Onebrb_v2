using Microsoft.Extensions.Options;
using Onebrb.MVC.Models;
using Onebrb.MVC.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Onebrb.MVC.Areas.Manager.ViewModels.Job
{
    public class ViewSingleJobVM
    {
        private readonly IOptions<CompanyLogoOptions> _companyLogoOptions;

        public string JobId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Applications { get; set; }
        public DateTime DatePosted { get; set; }
        public bool IsDisabled { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public bool IsManager { get; set; }
        public bool AlreadyApplied { get; set; }
        public string CompanyLogoFullPath { get; set; }
    }
}
