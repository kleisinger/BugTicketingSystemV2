namespace BugTicketingSystemV2.Models
{
    public class TicketAttachment
    {
        // properties
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? FilePath { get; set; }
        public string? FileUrl { get; set; }

        // relationships
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }


        public TicketAttachment(string body, string? filePath, string? fileUrl, Ticket ticket, AppUser submitter)
        {
            Body = body;
            FilePath = filePath;
            FileUrl = fileUrl;
            CreatedDate = DateTime.Now;
            Ticket = ticket;
            User = submitter;
        }

        public TicketAttachment()
        {

        }
    }
}
