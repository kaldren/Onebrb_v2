using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onebrb.MVC.Settings
{
    public class CompanyOptions
    {
        public const string CompanySettings = "CompanySettings";
        public string CompanyLogoFolderPath { get; set; }
        public string NoCompanyLogoFileName { get; set; }
        public int LogoImageWidth { get; set; }
        public int LogoImageHeight { get; set; }
    }
}
