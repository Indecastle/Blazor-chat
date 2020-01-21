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
        public const int limitMessage = 10;

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Message> Messages { get; set; }
        public virtual List<ChatUser> ChatUsers { get; set; }

        public GroupChat()
        {
            ChatUsers = new List<ChatUser>();
            Messages = new List<Message>();
        }

        public GroupChat(string name) : this()
        {
            this.Name = name;
        }

        public event Action<Message> Sent;
        public event Action<List<Message>> Updated;
        public event Action<string, Message> ChangedMessage;

        public void SendMessage(Message message)
        {
            Sent?.Invoke(message);
        }

        public void UpdateChat(List<Message> removeMessages)
        {
            Updated?.Invoke(removeMessages);
        }

        public void ChangeMessage(string textMessage, Message editingMessage)
        {
            ChangedMessage.Invoke(textMessage, editingMessage);
        }

        static public void RemoveMessages(List<Message> messages, List<Message> removedMessages)
        {
            removedMessages.ForEach(m => messages.RemoveAt(messages.FindIndex(m2 => m.Id == m2.Id)));
        }
    }
}
