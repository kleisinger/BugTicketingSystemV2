using System;
using BugTicketingSystemV2.Data.DAL;
using BugTicketingSystemV2.Models;
using Microsoft.EntityFrameworkCore;

namespace BugTicketingSystemV2.Data
{
	public class TicketRepository : IRepository<Ticket>
	{
		private BugTicketingSystemV2Context context { get; set; }

		public TicketRepository(BugTicketingSystemV2Context db)
		{
			context = db;
		}

		public TicketRepository()
        {

        }

		public virtual void Add(Ticket ticket)
		{
			context.Tickets.Add(ticket);
		}

		// read
		public virtual Ticket Get(int id)
		{
			Ticket ticket = context.Tickets
				//.Include(t => t.Submitter)
				.Include(t => t.User)
				.Include(p => p.Project).First(a => a.Id == id);
			return ticket;
		}

		public virtual Ticket Get(Func<Ticket, bool> firstFunction)
		{
			return context.Tickets.First(firstFunction);
		}

		public virtual ICollection<Ticket> GetAll()
		{
			var allTickets = context.Tickets
				.Include(t => t.Submitter)
				.Include(t => t.User);
			//.Include(d => d.Submitter).Include(u => u.User
			//);
			return allTickets.ToList();
		}

		public virtual ICollection<Ticket> GetProjectManagerTickets(string id)
        {
			var tickets = context.Tickets.Where(s => s.SubmitterId == id).Where(s => s.ticketStatus != TicketStatus.Resolved).ToList();
			return tickets;
        }


		public virtual ICollection<Ticket> GetList(Func<Ticket, bool> whereFunction)
		{
			return context.Tickets.Where(whereFunction).ToList();
		}


		public virtual void Update(Ticket ticket)
		{
			context.Tickets.Update(ticket);
		}

		public virtual void Remove(Ticket ticket)
		{
			
			context.Tickets.Remove(ticket);
		}

		public virtual void Save()
		{
			context.SaveChanges();
		}
	}
}

