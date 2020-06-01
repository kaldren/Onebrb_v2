using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Onebrb.MVC.Areas.Message.ViewModels.Message
{
    public class CreateMessageVM
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
        public string Author { get; set; }
        public string Recipient { get; set; }
        public DateTime DateSent { get; set; } = DateTime.UtcNow;
    }
}
