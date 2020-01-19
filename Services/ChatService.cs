using Chat.Data;
using Chat.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Chat.Services
{
    public class ChatService
    {
        public IServiceProvider Services { get; }
        public List<GroupChat> groupchats { get; set; }
        public ChatService(IServiceProvider services)
        {
            Services = services;

            using (var scope = Services.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                groupchats = (from ff in _db.GroupChats.Include(g => g.Messages).Include(g => g.ChatUsers).ThenInclude(cu => cu.User)
                              select ff).ToList();
            }
        }
    }
}

