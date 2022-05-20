using System;
using BugTicketingSystemV2.Data.DAL;
using BugTicketingSystemV2.Models;

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

    }
}
