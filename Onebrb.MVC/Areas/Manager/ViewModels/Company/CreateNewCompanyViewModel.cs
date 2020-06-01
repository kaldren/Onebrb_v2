using Microsoft.AspNetCore.Http;
using Onebrb.MVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Onebrb.MVC.Areas.Manager.ViewModels.Company
{
    public class CreateNewCompanyViewModel
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
        [Display(Name = "Company logo")]
        public IFormFile CompanyLogoImage { get; set; }
    }
}
