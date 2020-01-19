﻿using Chat.Data;
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

        public SingleChatService(ApplicationDbContext db, ChatService chatService, AuthenticationStateProvider authenticationStateProvider, UserManager<User> userManager)
        {
            this._db = db;
            this._chatService = chatService;
            this.AuthenticationStateProvider = authenticationStateProvider;
            this._userManager = userManager;
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
        public bool IsExists(List<User> users)
        {
            var chats = _db.GroupChats.Include(gc => gc.ChatUsers).ThenInclude(cu => cu.User).ToList();
            return chats.Any(c => users.All(u => c.ChatUsers.Exists(cu => cu.UserId == u.Id && cu.GroupChatId == c.Id)));
            //return users.All(u => chats.);
        }

        public async Task<User> GetCurrentUser()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var claimUser = authState.User;
            return await _userManager.GetUserAsync(claimUser);
        }
    }
}