using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Chat.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Models
{
    public class GroupChat
    {
        const int limitMessage = 10;

        [Key]
        public int Id { get; set; }
        public virtual List<Message> Messages { get; set; }
        public virtual List<ChatUser> ChatUsers { get; set; }
        [NotMapped]
        public int Count { get; set; } = 0;

        public GroupChat()
        {
            ChatUsers = new List<ChatUser>();
            Messages = new List<Message>();
        }

        public delegate void SendChatHandler(Message newMessage);
        public event SendChatHandler Notify;

        public void SendMessage(string newMessage, User user, ApplicationDbContext _db)
        {
            var message = new Message
            {
                UserName = user.UserName,
                Text = newMessage,
                GroupChatID = Id,
                When = DateTime.Now
            };
            Messages.Add(message);
            _db.Messages.Add(message);
            _db.SaveChanges();
            if (Messages.Count > limitMessage)
            {
                var removelist = Messages.Take(Messages.Count - limitMessage).ToList();
                removelist.ForEach(m => _db.Entry(m).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);
                Messages.RemoveRange(0, Messages.Count - limitMessage);
                _db.SaveChanges();
            }
            
            Notify?.Invoke(message);
        }
    }
}
