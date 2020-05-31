using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onebrb.MVC.Settings
{
    public class CompanySettings
    {
        public const string Settings = "CompanySettings";
        public string LogosFolder { get; set; }
        public string NoCompanyLogoFileName { get; set; }
        public int LogoImageWidth { get; set; }
        public int LogoImageHeight { get; set; }
    }
}
