using Onebrb.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onebrb.MVC.Areas.Message.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public string Recipient { get; set; }
        public DateTime DateSent { get; set; }
        public bool IsHiddenForAuthor { get; set; }
        public bool IsHiddenForRecipient { get; set; }

        public Message()
        {
            DateSent = DateTime.UtcNow;
        }
    }
}
