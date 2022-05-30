using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTicketingSystemV2.Data;
using BugTicketingSystemV2.Models;
using Microsoft.AspNetCore.Identity;
using BugTicketingSystemV2.Data.DAL;
using Microsoft.AspNetCore.Authorization;
using BugTicketingSystemV2.Data.BLL;

namespace BugTicketingSystemV2.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly BugTicketingSystemV2Context _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ProjectRepository _projectRepo;
        private readonly TicketRepository _ticketRepo;
        private readonly ProjectBusinessLogic _projectBLL;
        private readonly TicketBusinessLogic _ticketBLL;

        public ProjectsController(BugTicketingSystemV2Context context, 
            UserManager<AppUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _projectRepo = new ProjectRepository(_context);
            _ticketRepo = new TicketRepository(_context);
            _projectBLL = new ProjectBusinessLogic(_projectRepo);
            _ticketBLL = new TicketBusinessLogic(_ticketRepo);
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET
        [Authorize(Roles = "Admin, Project Manager")]
        public IActionResult AllProjects(int? SelectedPage, int? SelectedProjectsPerPage)
        {
            List<Project> AllProjectsList = _projectBLL.GetAllProjects().OrderByDescending(p => p.Id).ToList();

            int Page = 0;
            int ProjectsPerPage = 5;
            int ProjectsCount = AllProjectsList.Count();
            
            if (SelectedPage != null)
                Page = (int)SelectedPage;

            if (SelectedProjectsPerPage != null)
                ProjectsPerPage = (int)SelectedProjectsPerPage;

            ViewData["SelectedPage"] = Page;
            ViewData["SelectedProjectsPerPage"] = ProjectsPerPage;
            ViewData["ProjectsCount"] = ProjectsCount;

            int PagesCount = ProjectsCount / ProjectsPerPage;
            ViewData["PagesCount"] = PagesCount;
            int PageStart = ProjectsPerPage * Page;

            List<Project> ProjectsToShow = new List<Project>();
            for (int i = PageStart; i < PageStart + ProjectsPerPage && i < ProjectsCount; i++)
            {
                ProjectsToShow.Add(AllProjectsList[i]);
            }

            return View(ProjectsToShow);
        }


        // GET
        public async Task<IActionResult> CurrentProjects(int id)
        {
            var CurrentUserName = User.Identity.Name;
            AppUser CurrentUser = await _userManager.FindByNameAsync(CurrentUserName);
            var CurrentProjects = _projectBLL.GetCurrentProjects(CurrentUser);

            return View(CurrentProjects);
        }

        // GET
        public async Task<IActionResult> ProjectDetails(int id, int? AllId)
        {
            var CurrentUserName = User.Identity.Name;
            ViewBag.CurrentUser = await _userManager.FindByNameAsync(CurrentUserName);
            ViewBag.CurrentRole = await _userManager.GetRolesAsync(ViewBag.CurrentUser);
            if(AllId != null)
            {
                return View(_projectBLL.GetProjectById(id));
            }
            return View(_projectBLL.GetProjectById(id));
        }

        // GET
        [Authorize(Roles = "Admin, Project Manager")]
        public async Task<IActionResult> AddProject()
        {
            var CurrentUserName = User.Identity.Name;
            AppUser CurrentUser = await _userManager.FindByNameAsync(CurrentUserName);

            return View();
        }

        [HttpPost]
        public IActionResult AddProject(string title, string description)
        {
            _projectBLL.CreateProject(title, description);

            return RedirectToAction("AllProjects");
        }



        // GET
        [Authorize(Roles = "Admin, Project Manager")]
        public async Task<IActionResult> EditProject(int id)
        {
            var CurrentUserName = User.Identity.Name;
            AppUser CurrentUser = await _userManager.FindByNameAsync(CurrentUserName);

            return View(_projectBLL.GetProjectById(id));
        }

        [HttpPost]
        public IActionResult EditProject(int id, string? title, string? description)
        {
            _projectBLL.EditProject(id, title, description);

            return RedirectToAction("ProjectDetails", new{ id = id });
        }



        // GET
        [Authorize(Roles = "Admin, Project Manager")]
        public async Task<IActionResult> AssignTicket(int id)
        {
            List<AppUser> developers = new List<AppUser>();
            IdentityRole developerRole = await _roleManager.FindByNameAsync("Developer");
            foreach(var user in _context.Users.ToList())
            {
                IdentityRole role = await _roleManager.FindByIdAsync(developerRole.Id);
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    developers.Add(user);
                };
            }

            ViewBag.Developers = new SelectList(developers, "Id", "UserName");
            ViewBag.Ticket = _context.Tickets.First(t => t.Id == id);
            return View();
        }

        [HttpPost]
        public IActionResult AssignTicket(string devId, int ticketId)
        {
            Ticket Ticket = _ticketBLL.Get(ticketId);
            var ProjectId = Ticket.Project.Id;

            _projectBLL.AssignTicket(devId, Ticket);

            return RedirectToAction("ProjectDetails", new { id = ProjectId });
        }



        // GET
        [Authorize(Roles = "Admin, Project Manager")]
        public async Task<IActionResult> AssignProject(int id)
        {
            List<AppUser> PMs = new List<AppUser>();
            IdentityRole PMRole = await _roleManager.FindByNameAsync("Project Manager");
            foreach (var user in _context.Users.ToList())
            {
                IdentityRole role = await _roleManager.FindByIdAsync(PMRole.Id);
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    PMs.Add(user);
                };
            }

            ViewBag.PMs = new SelectList(PMs, "Id", "UserName");
            ViewBag.Project = _context.Projects.First(t => t.Id == id);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AssignProject(string pmId, int projectId)
        {
            AppUser pm = await _userManager.FindByIdAsync(pmId);

            _projectBLL.AssignProject(pm, projectId);

            return RedirectToAction("ProjectDetails", new { id = projectId });
        }

    }
}
