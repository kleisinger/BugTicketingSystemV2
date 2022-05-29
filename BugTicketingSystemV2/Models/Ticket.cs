using System.ComponentModel.DataAnnotations.Schema;

namespace BugTicketingSystemV2.Models
{
    public class Ticket
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
        public Project Project { get; set; }

        [ForeignKey("Submitter")]
        public string SubmitterId { get; set; }
        public SubmitterUser Submitter { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public AppUser? User { get; set; }

        public ICollection<TicketHistory> TicketHistories { get; set; }
        public ICollection<TicketComment> TicketComments { get; set; }
        public ICollection<TicketAttachment> TicketAttachments { get; set; }
        public ICollection<TicketNotification> TicketNotifications { get; set; }

        public Ticket(string title, string description, Project project, SubmitterUser submitter)
        {
            Title = title;
            Description = description;
            CreatedDate = DateTime.Now;
            Project = project;
            Submitter = submitter;
            TicketHistories = new HashSet<TicketHistory>();
            TicketComments = new HashSet<TicketComment>();
            TicketAttachments = new HashSet<TicketAttachment>();
            TicketNotifications = new HashSet<TicketNotification>();
        }

        public Ticket()
        {
        }
    }

    public enum TicketType
    {
        Incident,
        ServiceRequest,
        InformationRequest
    }
    public enum TicketStatus
    {
        Unassigned,
        Assigned,
        InProgress,
        Resolved
    }
    public enum TicketPriority
    {
        High,
        Medium,
        Low
    }

}
