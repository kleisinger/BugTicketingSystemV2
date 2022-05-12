namespace BugTicketingSystemV2.Models
{
    public class TicketNotification
    {
        // properties
        public int Id { get; set; }
        public string Body { get; set; }


        // relationships
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }


        public TicketNotification(string body, Ticket ticket, AppUser submitter)
        {
            Body = body;
            Ticket = ticket;
            User = submitter;
        }

        public TicketNotification()
        {

        }
    }
}
