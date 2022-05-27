using System;
using System.Linq;
using BugTicketingSystemV2.Data.DAL;
using BugTicketingSystemV2.Models;
using Microsoft.AspNetCore.Identity;

namespace BugTicketingSystemV2.Data.BLL
{
    public class ProjectBusinessLogic
    {
        public ProjectRepository Repo { get; set; }

        // CONSTRUCTORS
        public ProjectBusinessLogic(ProjectRepository repo)
        {
            Repo = repo;
        }

        public ProjectBusinessLogic()
        {
            
        }

        public Project GetProjectById(int id)
        {
            try
            {
                Project project = Repo.Get(id);
                if(project != null)
                    return Repo.Get(id);
                else
                    throw new Exception("Project was not found.");
            }
            catch (Exception ex)
            {
                throw new Exception("Project was not found.");
            }
        }

        public ICollection<Project> GetAllProjects()
        {
            var AllProjects = Repo.GetAll();

            return AllProjects;
        }

        public ICollection<Project> GetCurrentProjects(AppUser user)
        {
            var AllProjects = Repo.GetList(p => p.Users.Contains(user));
                return AllProjects;
        }

        public void CreateProject(string title, string description)
        {
            Project newProject = new Project()
            {
                Title = title,
                Description = description
            };

            Repo.CreateProject(newProject);
            Repo.Save();
        }

        public void EditProject(int id, string title, string description)
        {
            var ProjectToEdit = Repo.Get(id);
            ProjectToEdit.Title = title;
            ProjectToEdit.Description = description;

            Repo.Update(ProjectToEdit);
            Repo.Save();
        }

        public void AssignTicket(int id)
        {

        }

        public void AssignProject(int id)
        {

        }

    }
}
