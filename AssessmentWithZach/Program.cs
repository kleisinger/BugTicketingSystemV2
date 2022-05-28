// See https://aka.ms/new-console-template for more information
using System.ComponentModel.DataAnnotations.Schema;
using BugTicketingSystemV2.Models;


ITicket NewTicket = new BugReport();
NewTicket = new AddOnBackLogReissue(NewTicket);

public abstract class ITicket
{
    protected int Id { get; set; }
    protected string Title { get; set; }
    protected string Description { get; set; }
    protected DateTime CreatedDate { get; set; }
    protected DateTime? UpdatedDate { get; set; }

    protected TicketStatus ticketStatus { get; set; }
    protected TicketType ticketType { get; set; }
    protected TicketPriority ticketPriority { get; set; }
    protected int _responseDeadline { get; set; }
    public virtual int ResponseDeadline()
    {
        return _responseDeadline;
    }
    protected int? _breachDeadline { get; set; }
    public virtual int? BreachedDeadline()
    {
        return _breachDeadline;
    }

    // relationships
    protected Project Project { get; set; }

    [ForeignKey("Submitter")]
    protected string SubmitterId { get; set; }
    SubmitterUser Submitter { get; set; }

    [ForeignKey("User")]
    protected string? UserId { get; set; }
    protected AppUser? User { get; set; }

    protected ICollection<TicketHistory> TicketHistories { get; set; }
    protected ICollection<TicketComment> TicketComments { get; set; }
    protected ICollection<TicketAttachment> TicketAttachments { get; set; }
    protected ICollection<TicketNotification> TicketNotifications { get; set; }
}

public class BugReport : ITicket
{
    public string ErrorCodes { get; set; }
    public string ErrorLogs { get; set; }
    
    public BugReport()
    {
        if(ticketPriority == TicketPriority.High)
        {
            _responseDeadline = 1 * 2;
            _breachDeadline = _responseDeadline + 24; 
        } else if (ticketPriority == TicketPriority.Medium)
        {
            _responseDeadline = 2 * 2;
            _breachDeadline = _responseDeadline + 48;
        }
        else
        {
            _responseDeadline = 3 * 2;
            _breachDeadline = _responseDeadline + 72;
        }
    }
}

public class ServiceRequest : ITicket
{
    public List<Enum> ServiceTypes { get; set; }
}

public abstract class AddOnBugReport : ITicket
{
    public ITicket Ticket;
    public abstract override int ResponseDeadline();
    public abstract override int? BreachedDeadline();
}

public class AddOnWhiteGloveMerchant : AddOnBugReport
{
    public AddOnWhiteGloveMerchant(ITicket ticket)
    {
        Ticket = ticket;
    }

    public override int? BreachedDeadline()
    {
        return (int)(Ticket.BreachedDeadline());
    }

    public override int ResponseDeadline()
    {
        return (int)(Ticket.ResponseDeadline() * 0.8);
    }
}

public class AddOnBackLogReissue : AddOnBugReport
{
    private ITicket newTicket;

    //public AddOnBackLogReissue(BugReport ticket)
    //{
    //    Ticket = ticket;
    //}

    public AddOnBackLogReissue(ITicket newTicket)
    {
        this.newTicket = newTicket;
    }

    public override int? BreachedDeadline()
    {
        return Ticket.BreachedDeadline() + 100;
    }

    public override int ResponseDeadline()
    {
        return Ticket.ResponseDeadline();
    }
}