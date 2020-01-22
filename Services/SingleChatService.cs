using Chat.Data;
using Chat.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Services
{
    public class SingleChatService
    {
        public ApplicationDbContext _db { get; set; }
        public ChatService _chatService { get; set; }
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        public UserManager<User> _userManager { get; set; }

        public List<Message> LocalMessages { get; set; }
        public GroupChat groupChat { get; set; }
        public User user { get; set; }

        public SingleChatService(ApplicationDbContext db, ChatService chatService, AuthenticationStateProvider authenticationStateProvider, UserManager<User> userManager)
        {
            this._db = db;
            this._chatService = chatService;
            this.AuthenticationStateProvider = authenticationStateProvider;
            this._userManager = userManager;
        }

        public async Task<bool> InitChat(int chatId)
        {
            user = await GetCurrentUser();
            groupChat = GetActiveChat(chatId);
            if (groupChat != null && groupChat.ChatUsers.Exists(cu => cu.UserId == user.Id))
            {
                LocalMessages = Message.CopyMessages(groupChat.Messages);
                user = await GetCurrentUser();
                return true;
            }
            return false;
        }

        public GroupChat GetActiveChat(int chatId)
        {
            GroupChat chat = _chatService.groupchats.FirstOrDefault(c => c.Id == chatId);
            if (chat != null)
            {
            }
            else
            {
                chat = _db.GroupChats.Include(g => g.Messages).FirstOrDefault(c => c.Id == chatId);
                if (chat != null)
                    _chatService.groupchats.Add(chat);
            }
            return chat;
        }

        public GroupChat GetChatByUsers(List<User> users)
        {
            var chats = _db.GroupChats.Include(gc => gc.ChatUsers).ThenInclude(cu => cu.User).ToList();
            //return chats.Any(c => users.All(u => c.ChatUsers.Exists(cu => cu.UserId == u.Id && cu.GroupChatId == c.Id)));
            return chats.FirstOrDefault(c => users.Count == c.ChatUsers.Count && users.All(u => c.ChatUsers.Exists(cu => cu.UserId == u.Id && cu.GroupChatId == c.Id)));
            //return users.All(u => chats.);
        }

        public async Task<User> GetCurrentUser()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var claimUser = authState.User;
            return await _userManager.GetUserAsync(claimUser);
        }

        public void SendMessage(string newMessage, User user)
        {
            var message = new Message
            {
                UserName = user.UserName,
                Text = newMessage,
                GroupChatID = groupChat.Id,
                When = DateTime.Now
            };

            groupChat.Messages.Add(message);
            _db.Messages.Add(message);
            _db.SaveChanges();

            if (groupChat.Messages.Count > GroupChat.limitMessage)
            {
                var removelist = groupChat.Messages.Take(groupChat.Messages.Count - GroupChat.limitMessage).ToList();
                removelist.ForEach(m => _db.Entry(m).State = EntityState.Deleted);
                groupChat.Messages.RemoveRange(0, groupChat.Messages.Count - GroupChat.limitMessage);
                _db.SaveChanges();


            }

            groupChat.SendMessage(message);
        }

        public void GotMessage(Message newMessage)
        {
            Message copyNewMessage = new Message(newMessage);
            LocalMessages.Add(copyNewMessage);
            if (LocalMessages.Count > GroupChat.limitMessage)
            {
                LocalMessages.RemoveRange(0, LocalMessages.Count - GroupChat.limitMessage);
            }
        }

        public void RemoveSelectedMessages()
        {
            List<Message> removeMessages = LocalMessages.Where(m => m.Selecting).ToList();

            removeMessages.ForEach(m => _db.Entry(m).State = EntityState.Deleted);
            _db.SaveChanges();
            GroupChat.RemoveMessages(groupChat.Messages, removeMessages);
            groupChat.UpdateChat(removeMessages);
        }

        public void RemovedSelectedMessages(List<Message> removeMessages)
        {
            GroupChat.RemoveMessages(LocalMessages, removeMessages);
            //LocalMessages = Message.CopyMessages(groupChat.Messages);
        }

        public void LogOutFromChat()
        {
            if(groupChat.ChatUsers.Count == 1)
            {
                _chatService.groupchats.Remove(groupChat);
                _db.Entry(groupChat).State = EntityState.Deleted;
                _db.SaveChanges();
            }
            else
            {
                ChatUser chatUser = groupChat.ChatUsers.FirstOrDefault(cu => cu.UserId == user.Id);
                _db.Entry(chatUser).State = EntityState.Deleted;
                _db.SaveChanges();
                groupChat.ChatUsers.Remove(chatUser);
            }
            
        }

        public void ChangeMessage(string textMessage, Message editingMessage)
        {
            var message = groupChat.Messages.FirstOrDefault(m => m.Id == editingMessage.Id);
            message.Text = textMessage;
            _db.Entry(message).State = EntityState.Modified;
            _db.SaveChanges();
            
            groupChat.ChangeMessage(textMessage, editingMessage);

        }

        public void ChangedMessage(string textMessage, Message editingMessage)
        {
            var message = LocalMessages.FirstOrDefault(m => m.Id == editingMessage.Id);
            message.Text = textMessage;
        }

        public void UploadImageMessage(string url)
        {
            var message = new Message
            {
                UserName = user.UserName,
                Text = url,
                GroupChatID = groupChat.Id,
                When = DateTime.Now,
                IsImage = true
            };

            groupChat.Messages.Add(message);
            _db.Messages.Add(message);
            _db.SaveChanges();

            if (groupChat.Messages.Count > GroupChat.limitMessage)
            {
                var removelist = groupChat.Messages.Take(groupChat.Messages.Count - GroupChat.limitMessage).ToList();
                removelist.ForEach(m => _db.Entry(m).State = EntityState.Deleted);
                groupChat.Messages.RemoveRange(0, groupChat.Messages.Count - GroupChat.limitMessage);
                _db.SaveChanges();


            }

            groupChat.SendMessage(message);
        }
    }
}
