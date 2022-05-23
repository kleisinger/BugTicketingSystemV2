using System;
using BugTicketingSystemV2.Data.DAL;
using BugTicketingSystemV2.Models;
using Microsoft.EntityFrameworkCore;

namespace BugTicketingSystemV2.Data.BLL
{
	public class TicketBusinessLogic
	{
		public TicketRepository repo;

		public TicketBusinessLogic(TicketRepository repository)
			{
				repo = repository;
			}

			public List<Ticket> GetAll()
			{
				return repo.GetAll().ToList();
			}
		public void AddTicket(Ticket ticket)
        {
			repo.Add(ticket);
			repo.Save();
        }

		public Ticket Get(int id)
        {
			if (id == null)
			{
				throw new ArgumentException("Ticket Id does not exist");
			}
			if(repo.Get(id) == null)
            {
				throw new ArgumentException("Ticket not found");
			}
			return repo.Get(id);
        }

		public void Edit(Ticket ticket, string Title, string Description, DateTime CreatedDate, DateTime UpdatedDate, TicketStatus ticketStatus, TicketType ticketType, TicketPriority ticketPriority)
		{
			if (ticket == null)
			{
				throw new Exception("Ticket not found");
			}
			try
			{
				ticket.Title = Title;
				ticket.Description = Description;
				ticket.CreatedDate = CreatedDate;
				ticket.UpdatedDate = UpdatedDate;
				ticket.ticketStatus = ticketStatus;
				ticket.ticketType = ticketType;
				ticket.ticketPriority = ticketPriority;
				repo.Update(ticket);
				repo.Save();
			}
			catch (DbUpdateConcurrencyException)
			{
					throw;
			}

		}

		public ICollection<Ticket> SubmitterTickets(string submitterId)
		{
			List<Ticket> Tickets = repo.GetSubmitterTickets(submitterId).ToList();
			return Tickets;
		}

		public ICollection<Ticket> DeveloperAssignedTickets(string devID)
		{
			List<Ticket> Tickets = repo.GetDeveloperAssignedTickets(devID).ToList();
			return Tickets;
		}

		public void DeleteTicket(int id)
        {
			if (id == null)
			{
				throw new ArgumentException("Ticket Id does not exist");
			}
			repo.Remove(repo.Get(id));
			repo.Save();
        }
	}
}

