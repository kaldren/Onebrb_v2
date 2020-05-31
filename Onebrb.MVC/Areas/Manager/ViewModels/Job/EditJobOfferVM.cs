using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onebrb.MVC.Areas.Manager.ViewModels.Job
{
    public class EditJobOfferVM
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DatePosted { get; set; }
        public bool IsDisabled { get; set; }
    }
}
