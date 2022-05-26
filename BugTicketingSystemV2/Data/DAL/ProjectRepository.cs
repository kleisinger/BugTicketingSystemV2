using System;
using BugTicketingSystemV2.Data.DAL;
using BugTicketingSystemV2.Models;
using Microsoft.EntityFrameworkCore;

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
        public virtual void CreateProject(Project project)
        {
            _db.Projects.Add(project);
        }

        // READ
        public virtual Project Get(int id)
        {
            var Projects = _db.Projects.Include(p => p.Users)
                                       .Include(p => p.Tickets).ThenInclude(t => t.User);

            return Projects.First(p => p.Id == id);
        }

        public virtual Project Get(Func<Project, bool> firstFunction)
        {
            var Projects = _db.Projects.Include(p => p.Users)
                                       .Include(p => p.Tickets).ThenInclude(t => t.User);

            return Projects.First(firstFunction);
        }

        public virtual ICollection<Project> GetAll()
        {
            var Projects = _db.Projects;

            return Projects.ToList();
        }

        public virtual ICollection<Project> GetList(Func<Project, bool> whereFunction)
        {
            var Projects = _db.Projects.Include(p => p.Users)
                                       .Include(p => p.Tickets).ThenInclude(t => t.User);

            return Projects.Where(whereFunction).ToList();
        }

        // UPDATE
        public virtual void Update(Project project)
        {
            _db.Projects.Update(project);
        }

        // DELETE
        public virtual void Remove(Project project)
        {
            _db.Projects.Remove(project);
        }

        // SAVE
        public virtual void Save()
        {
            _db.SaveChanges();
        }
    }
}
