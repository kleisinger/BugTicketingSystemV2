using BugTicketingSystemV2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BugTicketingSystemV2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            //var CurrentUserName = User.Identity.Name;
            //if(CurrentUserName != null)
            //{
            //    ViewBag.CurrentUser = await _userManager.FindByNameAsync(CurrentUserName);
            //    ViewBag.CurrentRole = await _userManager.GetRolesAsync(ViewBag.CurrentUser);
            //}
            //else
            //{
            //    ViewBag.CurrentUser = new List<string>();
            //    ViewBag.CurrentRole = new List<string>();
            //}
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}