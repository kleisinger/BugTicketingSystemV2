namespace BugTicketingSystemV2.Models
{
    public class Project
    {
        // properties
        public int Id { get; set; }
        public string Title { get; set; }

        // relationships
        public ICollection<Ticket> Tickets { get; set; }
        public ICollection<AppUser> Users { get; set; }


        public Project(string title)
        {
            Title = title;
            Tickets = new HashSet<Ticket>();
            Users = new HashSet<AppUser>();
        }
        public Project()
        {

        }
    }
}
