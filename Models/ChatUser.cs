using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Models
{
    public class ChatUser
    {
        public string UserId { get; set; }
        public User User { get; set; }


        public int GroupChatId { get; set; }
        public GroupChat GroupChat { get; set; }
    }
}
