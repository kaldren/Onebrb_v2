﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Onebrb.MVC.Areas.Manager.ViewModels.Company
{
    public class EditCompanyVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public bool IsDisabled { get; set; }
        [Display(Name = "Company logo")]
        public IFormFile CompanyLogoImage { get; set; }
    }
}