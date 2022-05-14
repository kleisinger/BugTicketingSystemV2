using System;
namespace BugTicketingSystemV2.Data.DAL
{
	public interface IRepository<T> where T : class
	{

		// Create
		T Get(int id);

		T Get(Func<T, bool> firstFunction);

		ICollection<T> GetAll();

		ICollection<T> GetList(Func<T, bool> whereFunction);

		// Update
		void Update(T entity);

		// Delete
		void Remove(T entity);

		void Save();

	}
}

