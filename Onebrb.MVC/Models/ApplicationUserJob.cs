using Onebrb.MVC.Areas.Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onebrb.MVC.Models
{
    public class ApplicationUserJob
    {
        public string JobId { get; set; }
        public Job Job { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationId { get; set; }
        public string Status { get; set; }
    }
}
