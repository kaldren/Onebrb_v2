using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onebrb.MVC.Utils
{
    public static class DefaultSettings
    {
        // Images
        public const string ImagesFolderName = "images";
        public const string CompanyLogosFolderName = "company-logos";
        public const string NoCompanyLogoFileName = "no_logo.png";

        public static class JobApplication
        {
            public const string Active = "Active";
            public const string Cancelled = "Cancelled";
        }
    }
}
