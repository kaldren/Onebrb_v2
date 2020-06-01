using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onebrb.MVC.Areas.Message.ViewModels.Message
{
    public class ViewMessageVM
    {
        public Guid Id { get; set; }
        public string Recipient { get; set; }
        public string Author { get; set; }
        public DateTime DateSent { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
