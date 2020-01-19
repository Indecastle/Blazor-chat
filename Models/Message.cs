using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Text { get; set; }
        public DateTime When { get; set; }

        public int GroupChatID { get; set; }
        public GroupChat GroupChat { get; set; }

        public Message()
        {
            When = DateTime.Now;
        }
    }
}
