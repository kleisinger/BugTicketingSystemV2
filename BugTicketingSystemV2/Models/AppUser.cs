using Microsoft.AspNetCore.Identity;

namespace BugTicketingSystemV2.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<Ticket> Tickets { get; set; }
        public ICollection<TicketComment> Comments { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<TicketNotification> Notifications { get; set; }


        public AppUser()
        {
            Tickets = new HashSet<Ticket>();
            Comments = new HashSet<TicketComment>();
            Projects = new HashSet<Project>();
            Notifications = new HashSet<TicketNotification>();
        }

    }
}
