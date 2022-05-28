using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTicketingSystemV2.Data;
using BugTicketingSystemV2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using BugTicketingSystemV2.Data.BLL;

namespace BugTicketingSystemV2.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly BugTicketingSystemV2Context _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public TicketRepository repo;
        public TicketBusinessLogic ticketBll;

        public TicketsController(BugTicketingSystemV2Context context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            repo = new TicketRepository(_context);
            ticketBll = new TicketBusinessLogic(repo);
        }

        public async Task<IActionResult> Index(int? ResId)
        {
            string name = User.Identity.Name;
            AppUser user = await _userManager.FindByEmailAsync(name);
            bool IsSubmitter = await _userManager.IsInRoleAsync(user, "Submitter");
            bool IsDeveloperUser = await _userManager.IsInRoleAsync(user, "Developer");
            bool IsProjectManager = await _userManager.IsInRoleAsync(user, "Project Manager");
            if (IsSubmitter)
            {
                ViewBag.user = "Submitter";
                return View(ticketBll.SubmitterTickets(user.Id));
            } else if(IsDeveloperUser)
            {
                ViewBag.user = "Developer";
                return View(ticketBll.DeveloperAssignedTickets(user.Id));
            } else if (IsProjectManager)
            {
                ViewBag.user = "Project Manager";
                if(ResId != null)
                {
                    return View(ticketBll.GetAll().Where(s => s.Project.Users.Contains(user)));
                }
                return View(ticketBll.GetAll());
            }
            return View(ticketBll.GetAll());
        }

        [Authorize(Roles = "Project Manager")]
        public async Task<IActionResult> ViewAll(int? id)
        {
            if(id != null)
            {

            }
            return RedirectToAction("Index",ticketBll.GetAll());
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(ticketBll.Get(id));
        }

        [Authorize(Roles = "Submitter")]
        public IActionResult Create(int? Pid)
        {

            ViewBag.YourEnumsStatus = new SelectList(Enum.GetValues(typeof(TicketStatus)), TicketStatus.Unassigned);
            ViewBag.YourEnumsType = new SelectList(Enum.GetValues(typeof(TicketType)), TicketType.InformationRequest);
            ViewBag.YourEnumsPriority = new SelectList(Enum.GetValues(typeof(TicketPriority)), TicketPriority.Low);
            ViewBag.YourProjects = _context.Projects.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int projectId, [Bind("Id,Title,Description,CreatedDate,ticketStatus,ticketType,ticketPriority")] Ticket ticket)
        {
            string name = User.Identity.Name;
            SubmitterUser user = (SubmitterUser)await _userManager.FindByEmailAsync(name);
            ticket.SubmitterId = user.Id;
            ticket.Submitter = user;
            Project project = _context.Projects.FirstOrDefault(x => x.Id == projectId);
            if(user != null)
            {
                ViewBag.YourEnumsStatus = new SelectList(Enum.GetValues(typeof(TicketStatus)), ticket.ticketStatus);
                ViewBag.YourEnumsType = new SelectList(Enum.GetValues(typeof(TicketType)), ticket.ticketType);
                ViewBag.YourEnumsPriority = new SelectList(Enum.GetValues(typeof(TicketPriority)), ticket.ticketPriority);
                ViewBag.YourProjects = new SelectList(_context.Projects, "Pick a project");
                ticket.Project = project;
                ticketBll.AddTicket(ticket);
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        [Authorize(Roles="Developer")]
        public async Task<IActionResult> Edit(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewBag.YourEnumsStatus = new SelectList(Enum.GetValues(typeof(TicketStatus)), ticket.ticketStatus);
            ViewBag.YourEnumsType = new SelectList(Enum.GetValues(typeof(TicketType)), ticket.ticketType);
            ViewBag.YourEnumsPriority = new SelectList(Enum.GetValues(typeof(TicketPriority)), ticket.ticketPriority);
            return View(ticketBll.Get(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string Title, string Description, DateTime CreatedDate, DateTime UpdatedDate, TicketStatus ticketStatus, TicketType ticketType, TicketPriority ticketPriority)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            ViewBag.YourEnumsStatus = new SelectList(Enum.GetValues(typeof(TicketStatus)), ticket.ticketStatus);
            ViewBag.YourEnumsType = new SelectList(Enum.GetValues(typeof(TicketType)), ticket.ticketType);
            ViewBag.YourEnumsPriority = new SelectList(Enum.GetValues(typeof(TicketPriority)), ticket.ticketPriority);
            if (ModelState.IsValid)
            {
                ticketBll.Edit(ticket, Title, Description, CreatedDate, UpdatedDate, ticketStatus, ticketType, ticketPriority);
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        [Authorize(Roles = "Developer, Project Manager")]
        public async Task<IActionResult> MarkAsResolved(int id)
        {
            ticketBll.MarkTicketAsResolved(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddComment(int id)
        {
            Ticket ticket = ticketBll.Get(id);
            if(ticket != null)
            {
                ViewBag.TicketTitle = ticket.Title;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int id, string Body, DateTime createdDate)
        {
            string mail = User.Identity.Name;
            AppUser user = await _userManager.FindByNameAsync(mail);
            ticketBll.AddComment(id, Body, createdDate, user);
            //return RedirectToAction("Details", new { id = id });
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ViewComments(int id)
        {
            string mail = User.Identity.Name;
            AppUser user = await _userManager.FindByNameAsync(mail);
            Ticket ticket = _context.Tickets.Include(t => t.TicketComments).First(i => i.Id == id);
            ViewBag.SUsers = _context.Users.ToList();
            if(ticket.TicketComments.Count() == 0)
            {
                return RedirectToAction("Details", ticket.Id);
            }
            return View(ticket);
        }

        [Authorize(Roles ="Project Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            return View(ticketBll.Get(id));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ticketBll.DeleteTicket(id);
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
          return (_context.Tickets?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
