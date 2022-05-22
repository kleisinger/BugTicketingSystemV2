using System;
using BugTicketingSystemV2.Data.DAL;
using BugTicketingSystemV2.Models;
using Microsoft.EntityFrameworkCore;

namespace BugTicketingSystemV2.Data
{
	public class TicketRepository : IRepository<Ticket>
	{
		public BugTicketingSystemV2Context context { get; set; }

		public TicketRepository(BugTicketingSystemV2Context db)
		{
			context = db;
		}

		public void Add(Ticket ticket)
		{
			context.Tickets.Add(ticket);
		}

		// read
		public Ticket Get(int id)
		{
			return context.Tickets.First(a => a.Id == id);
		}

		public Ticket Get(Func<Ticket, bool> firstFunction)
		{
			return context.Tickets.First(firstFunction);
		}

		public ICollection<Ticket> GetAll()
		{
			var allTickets = context.Tickets;
			//.Include(d => d.Submitter).Include(u => u.User
			//);
			return allTickets.ToList();
		}

		public ICollection<Ticket> SubmitterTickets(string submitterId)
        {
			List<Ticket> Tickets = context.Tickets.Where(s => s.SubmitterId == submitterId).ToList();
			return Tickets;
        }

		public ICollection<Ticket> DeveloperAssignedTickets(string devID)
		{
			List<Ticket> Tickets = context.Tickets.Where(s => s.UserId == devID).ToList();
			return Tickets;
		}

		//public ICollection<Ticket> ProjectTickets(string projectManagerId)
		//{
		//	List<Ticket> ProjectManagerTickets = new List<Ticket>();
		//	foreach(var project in context.Projects)
  //          {
		//		foreach(var ticket in project.Tickets)
  //              {
		//			if()
  //              }
  //          }
		//}

		public ICollection<Ticket> GetList(Func<Ticket, bool> whereFunction)
		{
			return context.Tickets.Where(whereFunction).ToList();
		}

		public void Create()
        {

        }

		public void Edit(Ticket ticket, string Title, string Description, DateTime CreatedDate, DateTime UpdatedDate, TicketStatus ticketStatus, TicketType ticketType, TicketPriority ticketPriority)
        {
			ticket.Title = Title;
			ticket.Description = Description;
			ticket.CreatedDate = CreatedDate;
			ticket.UpdatedDate = UpdatedDate;
			ticket.ticketStatus = ticketStatus;
			ticket.ticketType = ticketType;
			ticket.ticketPriority = ticketPriority;
			context.Update(ticket);
			context.SaveChangesAsync();
		}

		public void Update(Ticket ticket)
		{
			context.Tickets.Update(ticket);
		}

		public void Remove(Ticket ticket)
		{
			context.Tickets.Remove(ticket);
		}

		public void Save()
		{
			context.SaveChanges();
		}
	}
}

