using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onebrb.MVC.Settings
{
    public class CompanyLogoOptions
    {
        public const string CompanyLogoSettings = "CompanyLogos";
        public string CompanyLogosFolderPath { get; set; }
        public string NoCompanyLogoFileName { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
    }
}
