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
using BugTicketingSystemV2.Data.BLL;
using System.Collections.Specialized;

namespace BugTicketingSystemV2.Controllers
{
    public class TicketsController : Controller
    {
        private readonly BugTicketingSystemV2Context _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ICollection<TicketHistory> TicketHistories { get; set; }
        public ICollection<TicketComment> TicketComments { get; set; }
        public ICollection<TicketAttachment> TicketAttachments { get; set; }
        public ICollection<TicketNotification> TicketNotifications { get; set; }
        public string SubmitterId;
        public string UserId;
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
            //var bugTicketingSystemV2Context = _context.Tickets.Include(t => t.Submitter).Include(t => t.User);
            ViewBag.SUsers = _userManager.Users.ToList();
            return View(repo.GetAll());
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

        public IActionResult Create()
        {

            ViewBag.YourEnumsStatus = new SelectList(Enum.GetValues(typeof(TicketStatus)), TicketStatus.Unassigned);
            ViewBag.YourEnumsType = new SelectList(Enum.GetValues(typeof(TicketType)), TicketType.InformationRequest);
            ViewBag.YourEnumsPriority = new SelectList(Enum.GetValues(typeof(TicketPriority)), TicketPriority.Low);

            // Suggestion: should we add a default enum when creating

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,CreatedDate,ticketStatus,ticketType,ticketPriority")] Ticket ticket)
        {
            string name = User.Identity.Name;
            SubmitterUser user = (SubmitterUser)await _userManager.FindByEmailAsync(name);
            ticket.SubmitterId = user.Id;
            ticket.Submitter = user;
            if(user != null)
            {
                ViewBag.YourEnumsStatus = new SelectList(Enum.GetValues(typeof(TicketStatus)), ticket.ticketStatus);
                ViewBag.YourEnumsType = new SelectList(Enum.GetValues(typeof(TicketType)), ticket.ticketType);
                ViewBag.YourEnumsPriority = new SelectList(Enum.GetValues(typeof(TicketPriority)), ticket.ticketPriority);
                var rand = new Random();
                int num = rand.Next(1, 4);
                ticket.Project = await _context.Projects.FindAsync(num);
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

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
            TicketHistories = ticket.TicketHistories;
            TicketComments = ticket.TicketComments;
            TicketAttachments = ticket.TicketAttachments;
            SubmitterId = ticket.SubmitterId;
            UserId = ticket.UserId;
            TicketNotifications = ticket.TicketNotifications;
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
                ticket.Title = Title;
                ticket.Description = Description;
                ticket.CreatedDate = CreatedDate;
                ticket.UpdatedDate = UpdatedDate;
                ticket.ticketStatus = ticketStatus;
                ticket.ticketType = ticketType;
                ticket.ticketPriority = ticketPriority;
                repo.Update(ticket);
                _context.SaveChangesAsync();
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
                _context.Tickets.Remove(ticket);
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
