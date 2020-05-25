using Onebrb.MVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Onebrb.MVC.Areas.Manager.Models
{
    public class Job
    {
        public string JobId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public int Applications { get; set; }
        public DateTime DatePosted { get; set; }
        public bool IsDisabled { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public ICollection<ApplicationUserJob> Applicants { get; set; }
    }
}
