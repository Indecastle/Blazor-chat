using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public bool IsImage { get; set; }
        [NotMapped]
        public bool Selecting { get; set; }

        public Message()
        {
            When = DateTime.Now;
        }
        public Message(Message message)
        {
            this.Id = message.Id;
            this.UserName = message.UserName;
            this.Text = message.Text;
            this.When = message.When;
            this.Selecting = message.Selecting;
            this.GroupChatID = message.GroupChatID;
            this.GroupChat = message.GroupChat;
        }

        public static List<Message> CopyMessages(List<Message> messages)
        {
            return messages.Select(m => new Message(m)).ToList();
        }
    }
}
