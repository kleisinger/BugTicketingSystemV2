using System;
using BugTicketingSystemV2.Data.DAL;
using BugTicketingSystemV2.Models;

namespace BugTicketingSystemV2.Data.BLL
{
	public class TicketBusinessLogic
	{

		IRepository<Ticket> repo;

		public TicketBusinessLogic(IRepository<Ticket> repository)
			{
				repo = repository;
			}

			public List<Ticket> GetAll()
			{
				return repo.GetAll().ToList();
			}
	}
}

