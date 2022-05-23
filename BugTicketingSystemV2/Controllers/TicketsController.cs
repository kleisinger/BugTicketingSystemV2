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

        public async Task<IActionResult> Index()
        {
            string name = User.Identity.Name;
            AppUser user = await _userManager.FindByEmailAsync(name);
            Task<bool> IsSubmitter = _userManager.IsInRoleAsync(user, "Submitter");
            Task<bool> IsDeveloperUser = _userManager.IsInRoleAsync(user, "Developer");
            Task<bool> IsProjectManager = _userManager.IsInRoleAsync(user, "Project Manager");
            if (await IsSubmitter)
            {
                return View(ticketBll.SubmitterTickets(user.Id));
            } else if(await IsDeveloperUser)
            {
                return View(ticketBll.DeveloperAssignedTickets(user.Id));
            }
            return View(ticketBll.GetAll());
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(ticketBll.Get(id));
        }

        [Authorize(Roles = "Submitter")]
        public IActionResult Create()
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

        [Authorize(Roles = "Developer")]
        public async Task<IActionResult> MarkAsResolved(int id)
        {
            ticketBll.MarkTicketAsResolved(id);
            return RedirectToAction(nameof(Index));
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
