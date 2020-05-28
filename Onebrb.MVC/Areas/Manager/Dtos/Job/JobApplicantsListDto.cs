using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onebrb.MVC.Areas.Manager.Dtos.Job
{
    public class JobApplicantsListDto
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ApplicationId { get; set; }
    }
}
