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
                return View(ticketBll.GetAll().Where(s => s.ticketStatus != TicketStatus.Resolved));
            }
            return View(ticketBll.GetAll());
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

        [HttpGet] 

        public IActionResult ViewTicketAttachments(int ticketId)
        {
            List<TicketAttachment> ticketAttachments = _context.TicketAttachments.Where(ta => ta.TicketId == ticketId).ToList();
            if (ticketAttachments.Count > 0)
                return View(ticketAttachments);

            return RedirectToAction($"Details/{ticketId}");
        }

        [HttpGet]
        public async Task<IActionResult> AddTicketAttachment(int ticketId)
        {
            Ticket ticket = _context.Tickets.First(t => t.Id == ticketId);
            return View(ticket);
        }

        [HttpPost]
        public async Task<IActionResult> AddTicketAttachment(string body, string filePath, string fileUrl, int ticketId)
        {
            string name = User.Identity.Name;
            AppUser user = await _userManager.FindByEmailAsync(name);
            Ticket ticket = _context.Tickets.First(t => t.Id == ticketId);
            TicketAttachment ticketAttachment = new TicketAttachment(body, filePath, fileUrl,ticket, user);
            _context.TicketAttachments.Add(ticketAttachment);
            _context.SaveChanges();

            return View();
        }
    }
}
