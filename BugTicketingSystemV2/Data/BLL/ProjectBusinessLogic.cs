using System;
using System.Linq;
using BugTicketingSystemV2.Data.DAL;
using BugTicketingSystemV2.Models;
using Microsoft.AspNetCore.Identity;

namespace BugTicketingSystemV2.Data.BLL
{
    public class ProjectBusinessLogic
    {
        public ProjectRepository ProjectRepo { get; set; }
        public TicketRepository TicketRepo { get; set; }
        public TicketBusinessLogic TicketBLL { get; set; }
        public UserManager<AppUser> _userManager;

        // CONSTRUCTORS
        public ProjectBusinessLogic(ProjectRepository repo)
        {
            ProjectRepo = repo;
        }

        public ProjectBusinessLogic(ProjectRepository repo, UserManager<AppUser> userManager)

        {
            ProjectRepo = repo;
            _userManager = userManager;
        }

        public ProjectBusinessLogic()
        {
            
        }

        public Project GetProjectById(int id)
        {
            try
            {
                Project project = ProjectRepo.Get(id);
                if(project != null)
                    return ProjectRepo.Get(id);
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
            var AllProjects = ProjectRepo.GetAll();

            return AllProjects;
        }

        public ICollection<Project> GetCurrentProjects(AppUser user)
        {
            var AllProjects = ProjectRepo.GetList(p => p.Users.Contains(user));
            return AllProjects;
        }

        public void CreateProject(string title, string description)
        {
            Project newProject = new Project()
            {
                Title = title,
                Description = description
            };

            ProjectRepo.CreateProject(newProject);
            ProjectRepo.Save();
        }

        public void EditProject(int id, string title, string description)
        {
            var ProjectToEdit = ProjectRepo.Get(id);
            ProjectToEdit.Title = title;
            ProjectToEdit.Description = description;

            ProjectRepo.Update(ProjectToEdit);
            ProjectRepo.Save();
        }

        public async void AssignTicket(string devId, Ticket Ticket)
        {
            if (Ticket != null && devId != null)
            {
                try
                {
                    Ticket.UserId = devId;
                    Ticket.ticketStatus = TicketStatus.Assigned;

                    ProjectRepo.Save();
                }
                catch (Exception ex)
                {
                    throw new Exception("The Developer could not be assigned.");
                }
            }
            else if(Ticket == null)
            {
                throw new Exception("The Ticket could not be found.");
            }
            else if(devId == null)
            {
                throw new Exception("The Developer you chose could not be found.");
            }
        }

        public void AssignProject(int id)
        {

        }

    }
}
