using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onebrb.MVC.Settings
{
    public class GeneralOptions
    {
        public const string GeneralSettings = "GeneralSettings";
        public List<string> Roles { get; set; }
        public string ImagesFolder { get; set; }
    }
}
