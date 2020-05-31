using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onebrb.MVC.Settings
{
    public class GeneralSettings
    {
        public const string Settings = "GeneralSettings";
        public string SiteName { get; set; }
        public string SiteSlogan { get; set; }
        public List<string> Roles { get; set; }
        public string ImagesFolder { get; set; }
    }
}
