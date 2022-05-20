using System;
using BugTicketingSystemV2.Data.DAL;
using BugTicketingSystemV2.Models;

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
			return context.Tickets.ToList();
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

