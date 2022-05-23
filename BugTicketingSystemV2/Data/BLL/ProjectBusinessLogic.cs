using System;
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
                return Repo.Get(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Project was not found.");
            }
        }

        public ICollection<Project> GetAllProjects()
        {
            try
            {
                return Repo.GetAll();
            }
            catch(Exception ex)
            {
                throw new Exception("No projects were found.");
            }
        }

        public ICollection<Project> GetCurrentProjects(AppUser user)
        {
            try
            {
                return Repo.GetList(p => p.Users.Contains(user));
            }
            catch (Exception ex)
            {
                throw new Exception("No projects were found.");
            }
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



    }
}
