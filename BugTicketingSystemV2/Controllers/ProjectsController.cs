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

namespace BugTicketingSystemV2.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly BugTicketingSystemV2Context _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //public ProjectRepository _repo;

        public ProjectsController(BugTicketingSystemV2Context context, 
            UserManager<AppUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            //ProjectRepository ProjectRepo
            //_repo = ProjectRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        //public async Task<IActionResult> AllProjects()
        //{
        //    return View(_repo.GetAll());
        //}
    }
}
