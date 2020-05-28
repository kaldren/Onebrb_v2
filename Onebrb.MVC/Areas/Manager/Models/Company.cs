using Onebrb.MVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Onebrb.MVC.Areas.Manager.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public string Url { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsDisabled { get; set; }
        public virtual ApplicationUser Manager { get; set; }
        public ICollection<Job> Jobs { get; set; }
    }
}
