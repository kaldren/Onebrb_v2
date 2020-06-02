using Onebrb.MVC.Areas.Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onebrb.MVC.Areas.Search.ViewModels
{
    public class SearchResultsVM
    {
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string JobId { get; set; }
        public DateTime DatePosted { get; set; }
    }
}
