namespace BugTicketingSystemV2.Models
{
    public class TicketComment
    {
        // properties
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }

        // relationships
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }


        public TicketComment(string body, Ticket ticket, AppUser submitter)
        {
            Body = body;
            CreatedDate = DateTime.Now;
            Ticket = ticket;
            User = submitter;
        }

        public TicketComment()
        {

        }
    }
}
}
