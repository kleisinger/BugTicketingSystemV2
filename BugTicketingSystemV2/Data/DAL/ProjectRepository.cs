using System;
using BugTicketingSystemV2.Data.DAL;
using BugTicketingSystemV2.Models;

namespace BugTicketingSystemV2.Data.DAL
{
    public class ProjectRepository : IRepository<Project>
    {
        private BugTicketingSystemV2Context _db { get; set; }

        // Constructors
        public ProjectRepository(BugTicketingSystemV2Context db)
        {
            _db = db;
        }

        public ProjectRepository()
        {

        }

        
        // CRUD
        // CREATE
        public void CreateProject(Project project)
        {
            _db.Projects.Add(project);
        }

        // READ
        public virtual Project Get(int id)
        {
            return _db.Projects.Find(id);
        }

        public Project Get(Func<Project, bool> firstFunction)
        {
            return _db.Projects.First(firstFunction);
        }

        public virtual ICollection<Project> GetAll()
        {
            return _db.Projects.ToList();
        }

        public ICollection<Project> GetList(Func<Project, bool> whereFunction)
        {
            return _db.Projects.Where(whereFunction).ToList();
        }

        // UPDATE
        public void Update(Project project)
        {
            _db.Projects.Update(project);
        }

        // DELETE
        public void Remove(Project project)
        {
            _db.Projects.Remove(project);
        }

        // SAVE
        public void Save()
        {
            _db.SaveChanges();
        }


    }
}
