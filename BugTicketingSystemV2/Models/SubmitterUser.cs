namespace BugTicketingSystemV2.Models
{
    public class SubmitterUser : AppUser
    {
        public SubmitterUser()
        {
            Tickets = new HashSet<Ticket>();
            Comments = new HashSet<TicketComment>();
        }
    }
}
