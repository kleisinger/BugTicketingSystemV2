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
        public ProjectRepository _projectRepo;
        public TicketRepository _ticketRepo;
        public ProjectBusinessLogic _projectBLL;
        public TicketBusinessLogic _ticketBLL;

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
        public async Task<IActionResult> AllProjects()
        {
            var CurrentUserName = User.Identity.Name;
            ViewBag.CurrentUser = await _userManager.FindByNameAsync(CurrentUserName);
            ViewBag.CurrentRole = await _userManager.GetRolesAsync(ViewBag.CurrentUser);

            return View(_projectBLL.GetAllProjects());
        }

        // GET
        public async Task<IActionResult> CurrentProjects(int id)
        {
            var CurrentUserName = User.Identity.Name;
            AppUser CurrentUser = await _userManager.FindByNameAsync(CurrentUserName);

            return View(_projectBLL.GetCurrentProjects(CurrentUser));
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
        public async Task<IActionResult> AddProject(string title, string description)
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
        public async Task<IActionResult> EditProject(int id, string title, string description)
        {
            _projectBLL.EditProject(id, title, description);

            return RedirectToAction("ProjectDetails", new{ id = id });
        }



        // GET
        // Still in progress
        // Submitter in the GET in the TICKET DAL is preventing this from working rn
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
        public async Task<IActionResult> AssignTicket(string devId, int ticketId)
        {
            Ticket Ticket = _ticketBLL.Get(ticketId);
            AppUser Dev = await _userManager.FindByIdAsync(devId);
            var ProjectId = Ticket.Project.Id;

            Ticket.UserId = devId;
            Ticket.User = Dev;
            Ticket.ticketStatus = TicketStatus.Assigned;
            _context.SaveChanges();

            return RedirectToAction("ProjectDetails", new { id = ProjectId });
        }

        // GET
        // Needs DAL / BLL Refactoring
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
            Project project = _projectBLL.GetProjectById(projectId);
            AppUser PM = await _userManager.FindByIdAsync(pmId);

            project.Users.Add(PM);
            _context.SaveChanges();

            return RedirectToAction("ProjectDetails", new { id = projectId });
        }



    }
}
