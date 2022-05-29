namespace BugTicketingSystemV2.Models
{
    public class TicketHistory
    {
        // properties
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public TicketStatus ticketStatus { get; set; }
        public TicketType ticketType { get; set; }
        public TicketPriority ticketPriority { get; set; }


        // relationships
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public TicketHistory(Ticket ticket, AppUser submitter)
        {
            User = submitter;
            Ticket = ticket;
            Title = ticket.Title;
            Description = ticket.Description;
            CreatedDate = ticket.CreatedDate;
            UpdatedDate = ticket.UpdatedDate;
            ticketStatus = ticket.ticketStatus;
            ticketType = ticket.ticketType;
            ticketPriority = ticket.ticketPriority;
        }
        public TicketHistory()
        {

        }
    }
}
