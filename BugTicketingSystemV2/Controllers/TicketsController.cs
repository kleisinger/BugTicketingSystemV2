using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTicketingSystemV2.Data;
using BugTicketingSystemV2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace BugTicketingSystemV2.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly BugTicketingSystemV2Context _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public TicketRepository repo;

        public TicketsController(BugTicketingSystemV2Context context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            repo = new TicketRepository(_context);
        }

        public async Task<IActionResult> Index()
        {
            string name = User.Identity.Name;
            AppUser user = await _userManager.FindByEmailAsync(name);
            Task<bool> IsSubmitter = _userManager.IsInRoleAsync(user, "Submitter");
            Task<bool> IsDeveloperUser = _userManager.IsInRoleAsync(user, "Developer");
            Task<bool> IsProjectManager = _userManager.IsInRoleAsync(user, "Project Manager");
            //ViewBag.SUsers = await _userManager.Users.ToListAsync();
            if (await IsSubmitter)
            {
                return View(repo.SubmitterTickets(user.Id));
            } else if(await IsDeveloperUser)
            {
                return View(repo.DeveloperAssignedTickets(user.Id));
            }
            return View(repo.GetAll());
        }

        [Authorize(Roles = "Submitter")]
        public async Task<IActionResult> SubmitterTickets()
        {
            string name = User.Identity.Name;
            SubmitterUser user = (SubmitterUser)await _userManager.FindByEmailAsync(name);
            if(user == null)
            {
                return NotFound();
            }
            ViewBag.SUsers = _userManager.Users.ToList();

            return View(repo.SubmitterTickets(user.Id));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Submitter)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        [Authorize(Roles = "Submitter")]
        public IActionResult Create()
        {

            ViewBag.YourEnumsStatus = new SelectList(Enum.GetValues(typeof(TicketStatus)), TicketStatus.Unassigned);
            ViewBag.YourEnumsType = new SelectList(Enum.GetValues(typeof(TicketType)), TicketType.InformationRequest);
            ViewBag.YourEnumsPriority = new SelectList(Enum.GetValues(typeof(TicketPriority)), TicketPriority.Low);
            //Lis
            //ViewBag.YourProjects = new SelectList(_context.Projects, "Pick a project");
            ViewBag.YourProjects = _context.Projects.ToList();
            // Suggestion: should we add a default enum when creating
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
                //var rand = new Random();
                //int num = rand.Next(1, 4);
                ticket.Project = project;
                _context.Tickets.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        [Authorize(Roles="Developer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewBag.YourEnumsStatus = new SelectList(Enum.GetValues(typeof(TicketStatus)), ticket.ticketStatus);
            ViewBag.YourEnumsType = new SelectList(Enum.GetValues(typeof(TicketType)), ticket.ticketType);
            ViewBag.YourEnumsPriority = new SelectList(Enum.GetValues(typeof(TicketPriority)), ticket.ticketPriority);
            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string Title, string Description, DateTime CreatedDate, DateTime UpdatedDate, TicketStatus ticketStatus, TicketType ticketType, TicketPriority ticketPriority)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }
            
            ViewBag.YourEnumsStatus = new SelectList(Enum.GetValues(typeof(TicketStatus)), ticket.ticketStatus);
            ViewBag.YourEnumsType = new SelectList(Enum.GetValues(typeof(TicketType)), ticket.ticketType);
            ViewBag.YourEnumsPriority = new SelectList(Enum.GetValues(typeof(TicketPriority)), ticket.ticketPriority);
            if (ModelState.IsValid)
            {
                try
                {
                    repo.Edit(ticket, Title, Description, CreatedDate, UpdatedDate, ticketStatus, ticketType, ticketPriority);
            }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        [Authorize(Roles ="Project Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Submitter)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'BugTicketingSystemV2Context.Ticket'  is null.");
            }
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                repo.Remove(ticket);
                repo.Save();
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
          return (_context.Tickets?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
