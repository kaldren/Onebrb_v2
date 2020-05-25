using Microsoft.AspNetCore.Identity;
using Onebrb.MVC.Areas.Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onebrb.MVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<ApplicationUserJob> ApplicationUserJob { get; set; }
    }
}
