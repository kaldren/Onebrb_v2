using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onebrb.MVC.Settings
{
    public class JobOptions
    {
        public const string JobSettings = "JobSettings";
        public ApplicationStatus JobStatus { get; set; }
    }
    public class ApplicationStatus
    {
        public string Active { get; set; }
        public string Cancelled { get; set; }
        public string Completed { get; set; }
        public string InProcess { get; set; }
    }
}
