using BugTicketingSystemV2.Data;
using BugTicketingSystemV2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

    public class SeedData
{
    public async static Task Initialize(IServiceProvider serviceProvider)
    {
        var context = new BugTicketingSystemV2Context(serviceProvider.GetRequiredService<DbContextOptions<BugTicketingSystemV2Context>>());
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        var passwordHasher = new PasswordHasher<AppUser>();

        // Create Roles
        if (!context.Roles.Any())
        {
            List<string> initialRoles = new List<string>()
                {
                    "Admin",
                    "Project Manager",
                    "Developer",
                    "Submitter"
                };

            foreach (string role in initialRoles)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Create Users
        if (!context.Projects.Any())
        {
            AppUser Admin1 = new AppUser
            {
                Email = "admin@mitt.ca",
                NormalizedEmail = "ADMIN@MITT.CA",
                UserName = "admin@mitt.ca",
                NormalizedUserName = "ADMIN@MITT.CA",
                EmailConfirmed = true
            };
            var hashedPassword = passwordHasher.HashPassword(Admin1, "Password!1");
            Admin1.PasswordHash = hashedPassword;
            await userManager.CreateAsync(Admin1);
            await userManager.AddToRoleAsync(Admin1, "Admin");

            AppUser PM1 = new AppUser
            {
                Email = "pm1@mitt.ca",
                NormalizedEmail = "PM1@MITT.CA",
                UserName = "pm1@mitt.ca",
                NormalizedUserName = "PM1@MITT.CA",
                EmailConfirmed = true,
            };
            var hashed1Password = passwordHasher.HashPassword(PM1, "Password!1");
            PM1.PasswordHash = hashed1Password;
            await userManager.CreateAsync(PM1);
            await userManager.AddToRoleAsync(PM1, "Project Manager");

            AppUser PM2 = new AppUser
            {
                Email = "pm2@mitt.ca",
                NormalizedEmail = "PM2@MITT.CA",
                UserName = "pm2@mitt.ca",
                NormalizedUserName = "PM2@MITT.CA",
                EmailConfirmed = true,
            };
            var hashed2Password = passwordHasher.HashPassword(PM2, "Password!1");
            PM2.PasswordHash = hashed2Password;
            await userManager.CreateAsync(PM2);
            await userManager.AddToRoleAsync(PM2, "Project Manager");

            AppUser Dev1 = new AppUser
            {
                Email = "dev1@mitt.ca",
                NormalizedEmail = "DEV1@MITT.CA",
                UserName = "dev1@mitt.ca",
                NormalizedUserName = "DEV1@MITT.CA",
                EmailConfirmed = true,
            };
            var hashed3Password = passwordHasher.HashPassword(Dev1, "Password!1");
            Dev1.PasswordHash = hashed3Password;
            await userManager.CreateAsync(Dev1);
            await userManager.AddToRoleAsync(Dev1, "Developer");

            AppUser Dev2 = new AppUser
            {
                Email = "dev2@mitt.ca",
                NormalizedEmail = "DEV2@MITT.CA",
                UserName = "dev2@mitt.ca",
                NormalizedUserName = "DEV2@MITT.CA",
                EmailConfirmed = true,
            };
            var hashed4Password = passwordHasher.HashPassword(Dev2, "Password!1");
            Dev2.PasswordHash = hashed4Password;
            await userManager.CreateAsync(Dev2);
            await userManager.AddToRoleAsync(Dev2, "Developer");

            SubmitterUser Submitter1 = new SubmitterUser
            {
                Email = "submitter1@mitt.ca",
                NormalizedEmail = "SUBMITTER1@MITT.CA",
                UserName = "submitter1@mitt.ca",
                NormalizedUserName = "SUBMITTER1@MITT.CA",
                EmailConfirmed = true,
            };

            var hashed5Password = passwordHasher.HashPassword(Submitter1, "Password!1");
            Submitter1.PasswordHash = hashed5Password;
            await userManager.CreateAsync(Submitter1);
            await userManager.AddToRoleAsync(Submitter1, "Submitter");

            SubmitterUser Submitter2 = new SubmitterUser
            {
                Email = "submitter2@mitt.ca",
                NormalizedEmail = "SUBMITTER2@MITT.CA",
                UserName = "submitter2@mitt.ca",
                NormalizedUserName = "SUBMITTER2@MITT.CA",
                EmailConfirmed = true,
            };

            var hashed6Password = passwordHasher.HashPassword(Submitter2, "Password!1");
            Submitter2.PasswordHash = hashed6Password;
            await userManager.CreateAsync(Submitter2);
            await userManager.AddToRoleAsync(Submitter2, "Submitter");


            // Create Projects
            List<Project> StarterProjects = new List<Project>()
                {
                    new Project("First Project", "description"),
                    new Project("Second Project", "description"),
                    new Project("Third Project", "description"),
                };

            context.Projects.AddRange(StarterProjects);

            //Create Tickets
            List<Ticket> StarterTickets = new List<Ticket>()
                {
                        new Ticket
                        {
                            Title = "Ticket One",
                            Description = "Description for ticket one.",
                            CreatedDate = new DateTime(2022, 05, 01),
                            ticketStatus = TicketStatus.Assigned,
                            ticketType = TicketType.ServiceRequest,
                            ticketPriority = TicketPriority.Medium,
                            Project = StarterProjects[1],
                            SubmitterId = Submitter1.Id,
                            UserId = Dev1.Id,
                        },

                        new Ticket
                        {
                            Title = "Ticket Two",
                            Description = "Description for ticket two.",
                            CreatedDate = new DateTime(2022, 05, 03),
                            ticketStatus = TicketStatus.Unassigned,
                            ticketType = TicketType.InformationRequest,
                            ticketPriority = TicketPriority.Low,
                            Project = StarterProjects[0],
                            SubmitterId = Submitter1.Id,
                            UserId = null,
                        },

                        new Ticket
                        {
                            Title = "Ticket Three",
                            Description = "Description for ticket three.",
                            CreatedDate = new DateTime(2022, 05, 02),
                            ticketStatus = TicketStatus.Resolved,
                            ticketType = TicketType.ServiceRequest,
                            ticketPriority = TicketPriority.Low,
                            Project = StarterProjects[1],
                            SubmitterId = Submitter2.Id,
                            UserId = Dev1.Id,
                        },
                        new Ticket
                        {
                            Title = "Ticket Four",
                            Description = "Description for ticket four.",
                            CreatedDate = new DateTime(2022, 05, 01),
                            ticketStatus = TicketStatus.Assigned,
                            ticketType = TicketType.ServiceRequest,
                            ticketPriority = TicketPriority.Medium,
                            Project = StarterProjects[1],
                            SubmitterId = Submitter1.Id,
                            UserId = Dev2.Id,
                        },
                        new Ticket
                        {
                            Title = "Ticket Five",
                            Description = "Description for ticket five.",
                            CreatedDate = new DateTime(2022, 05, 01),
                            ticketStatus = TicketStatus.Assigned,
                            ticketType = TicketType.Incident,
                            ticketPriority = TicketPriority.High,
                            Project = StarterProjects[2],
                            SubmitterId = Submitter2.Id,
                            UserId = Dev2.Id,
                        },
                        new Ticket
                        {
                            Title = "Ticket Six",
                            Description = "Description for ticket six.",
                            CreatedDate = new DateTime(2022, 05, 01),
                            ticketStatus = TicketStatus.Unassigned,
                            ticketType = TicketType.ServiceRequest,
                            ticketPriority = TicketPriority.Medium,
                            Project = StarterProjects[2],
                            SubmitterId = Submitter1.Id,
                            UserId = null,
                        },
                };

            context.Tickets.AddRange(StarterTickets);

            // Create Comments
            List<TicketComment> StarterComments = new List<TicketComment>()
                    {
                        new TicketComment
                        {
                            Body = "This is the first comment from a dev",
                            CreatedDate = new DateTime(2022, 05, 06),
                            TicketId = StarterTickets[0].Id,
                            Ticket = StarterTickets[0],
                            UserId = Dev1.Id,
                        },
                        new TicketComment
                        {
                            Body = "This is the first comment from a submitter",
                            CreatedDate = new DateTime(2022, 05, 05),
                            TicketId = StarterTickets[0].Id,
                            Ticket = StarterTickets[0],
                            UserId = Submitter1.Id,
                        },

                        new TicketComment
                        {
                            Body = "This is the second comment from a dev",
                            CreatedDate = new DateTime(2022, 05, 03),
                            TicketId = StarterTickets[1].Id,
                            Ticket = StarterTickets[1],
                            UserId = Dev2.Id,
                        },

                        new TicketComment
                        {
                            Body = "This is the second comment from a submitter",
                            CreatedDate = new DateTime(2022, 05, 05),
                            TicketId = StarterTickets[1].Id,
                            Ticket = StarterTickets[1],
                            UserId = Submitter2.Id,
                        },
                    };
            context.TicketComments.AddRange(StarterComments);

            // Create Attachments
            List<TicketAttachment> StarterAttachments = new List<TicketAttachment>()
                    {
                        new TicketAttachment
                        {
                            Body = "This is the first attachment from a dev",
                            CreatedDate = new DateTime(2022, 05, 10),
                            FilePath = "fake>test>path",
                            FileUrl = "www.fakeurl.com",
                            TicketId = StarterTickets[1].Id,
                            Ticket = StarterTickets[1],
                            UserId = Dev1.Id,
                        },

                        new TicketAttachment
                        {
                            Body = "This is the first attachment from a Submitter",
                            CreatedDate = new DateTime(2022, 05, 10),
                            FilePath = "fake>test>path",
                            FileUrl = "www.fakeurl.com",
                            TicketId = StarterTickets[1].Id,
                            Ticket = StarterTickets[1],
                            UserId = Submitter1.Id,
                        },

                        new TicketAttachment
                        {
                            Body = "This is the second attachment from a dev",
                            CreatedDate = new DateTime(2022, 05, 10),
                            FilePath = "fake>test>path",
                            FileUrl = "www.fakeurl.com",
                            TicketId = StarterTickets[2].Id,
                            Ticket = StarterTickets[2],
                            UserId = Dev2.Id,
                        },

                        new TicketAttachment
                        {
                            Body = "This is the second attachment from a submitter",
                            CreatedDate = new DateTime(2022, 05, 10),
                            FilePath = "fake>test>path",
                            FileUrl = "www.fakeurl.com",
                            TicketId = StarterTickets[2].Id,
                            Ticket = StarterTickets[2],
                            UserId = Submitter2.Id,
                        },
                    };

            context.TicketAttachments.AddRange(StarterAttachments);

            // Create Notifications
            List<TicketNotification> StarterNotifications = new List<TicketNotification>()
                    {
                        new TicketNotification
                        {
                            Body = "Testing with a notification",
                            TicketId = StarterTickets[1].Id,
                            Ticket = StarterTickets[1],
                            UserId = Dev1.Id,
                        },

                        new TicketNotification
                        {
                            Body = "Testing with a second notification",
                            TicketId = StarterTickets[1].Id,
                            Ticket = StarterTickets[1],
                            UserId = Dev1.Id,
                        },

                        new TicketNotification
                        {
                            Body = "Testing with a notification",
                            TicketId = StarterTickets[1].Id,
                            Ticket = StarterTickets[1],
                            UserId = Dev2.Id,
                        },
                    };
            context.TicketNotifications.AddRange(StarterNotifications);
        }

        context.SaveChanges();
    }
}
