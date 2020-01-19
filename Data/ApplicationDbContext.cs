using System;
using System.Collections.Generic;
using System.Text;
using Chat.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chat.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ChatUser>()
            .HasKey(t => new { t.UserId, t.GroupChatId });

            builder.Entity<Message>()
                .HasOne<GroupChat>(a => a.GroupChat)
                .WithMany(d => d.Messages)
                .HasForeignKey(d => d.GroupChatID);

            builder.Entity<ChatUser>()
               .HasOne<User>(a => a.User)
               .WithMany(d => d.ChatUsers)
               .HasForeignKey(d => d.UserId);
            builder.Entity<ChatUser>()
               .HasOne<GroupChat>(a => a.GroupChat)
               .WithMany(d => d.ChatUsers)
               .HasForeignKey(d => d.GroupChatId);
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<GroupChat> GroupChats { get; set; }
    }
}
