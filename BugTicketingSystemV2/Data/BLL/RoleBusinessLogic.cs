using System;
using BugTicketingSystemV2.Data.DAL;
using BugTicketingSystemV2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BugTicketingSystemV2.Data.BLL
{
	public class RoleBusinessLogic
	{
		private RoleManager<IdentityRole> roleManager;
		private UserManager<AppUser> userManager;

		//public RoleController(RoleManager<IdentityRole> roleMgr, UserManager<AppUser> userMrg)
		//{
		//	roleManager = roleMgr;
		//	userManager = userMrg;
		//}


	}
		
}

