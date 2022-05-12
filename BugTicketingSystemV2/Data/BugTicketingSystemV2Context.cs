using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BugTicketingSystemV2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BugTicketingSystemV2.Data
{
    public class BugTicketingSystemV2Context : IdentityDbContext<AppUser>
    {
        public BugTicketingSystemV2Context (DbContextOptions<BugTicketingSystemV2Context> options)
            : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TicketComment> TicketComments { get; set; }
        public DbSet<TicketNotification> TicketNotifications { get; set; }
        public DbSet<TicketHistory> TicketHistory { get; set; }
        public DbSet<TicketAttachment> TicketAttachments { get; set; }
    }
}
