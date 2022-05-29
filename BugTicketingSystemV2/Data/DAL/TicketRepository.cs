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
			Ticket ticket = context.Tickets
				//.Include(t => t.Submitter)
				.Include(t => t.User)
				.Include(p => p.Project).First(a => a.Id == id);
			return ticket;
		}

		public Ticket Get(Func<Ticket, bool> firstFunction)
		{
			return context.Tickets.First(firstFunction);
		}

		public ICollection<Ticket> GetAll()
		{
			var allTickets = context.Tickets
				.Include(t => t.Submitter)
				.Include(t => t.User);
			//.Include(d => d.Submitter).Include(u => u.User
			//);
			return allTickets.ToList();
		}

		public ICollection<Ticket> GetSubmitterTickets(string id)
        {
			var tickets = context.Tickets.Where(s => s.SubmitterId == id).Where(s => s.ticketStatus != TicketStatus.Resolved).ToList();
			return tickets;
        }

		public ICollection<Ticket> GetProjectManagerTickets(string id)
        {
			var tickets = context.Tickets.Where(s => s.SubmitterId == id).Where(s => s.ticketStatus != TicketStatus.Resolved).ToList();
			return tickets;
        }

		public ICollection<Ticket> GetDeveloperAssignedTickets(string id)
		{
			var tickets = context.Tickets.Where(s => s.UserId == id).Where(s => s.ticketStatus != TicketStatus.Resolved).ToList();
			return tickets;
		}


		public ICollection<Ticket> GetList(Func<Ticket, bool> whereFunction)
		{
			return context.Tickets.Where(whereFunction).ToList();
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

