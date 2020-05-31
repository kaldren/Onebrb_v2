using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onebrb.MVC.Settings
{
    public class JobSettings
    {
        public const string Settings = "JobSettings";
        public ApplicationStatus ApplicationStatus { get; set; }
    }
    public class ApplicationStatus
    {
        public string Active { get; set; }
        public string Cancelled { get; set; }
        public string Completed { get; set; }
        public string InProcess { get; set; }
    }
}
