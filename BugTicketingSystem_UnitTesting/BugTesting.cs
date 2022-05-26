using System;
using System.Collections.Generic;
using BugTicketingSystemV2.Data.BLL;
using BugTicketingSystemV2.Data.DAL;
using BugTicketingSystemV2.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BugTicketingSystem_UnitTesting;

[TestClass]
public class BugTesting
{
    public Mock<IRepository<Ticket>> repoMock;
    public TicketBusinessLogic TicketBll;

    [TestInitialize]
    public void TestInitialise()
    {
        repoMock = new Mock<IRepository<Ticket>>();
        TicketBll = new TicketBusinessLogic(repoMock.Object);

        SubmitterUser Submitter2 = new SubmitterUser
        {
            Email = "submitter2@mitt.ca",
            NormalizedEmail = "SUBMITTER2@MITT.CA",
            UserName = "submitter2@mitt.ca",
            NormalizedUserName = "SUBMITTER2@MITT.CA",
            EmailConfirmed = true,
        };

        AppUser Dev1 = new AppUser
        {
            Email = "dev1@mitt.ca",
            NormalizedEmail = "DEV1@MITT.CA",
            UserName = "dev1@mitt.ca",
            NormalizedUserName = "DEV1@MITT.CA",
            EmailConfirmed = true,
        };

        List<Project> StarterProjects = new List<Project>()
                {
                    new Project { Title = "The First Project"},
                    new Project { Title = "The Second Project" },
                    new Project { Title = "The Third Project" },
                };

        repoMock.Setup(r => r.Get(It.Is<int>(i => i == 1))).Returns(new Ticket
        {
            Title = "Moq Ticket1 Setup",
            Description = "Description for ticket one.",
            CreatedDate = new DateTime(2022, 05, 01),
            ticketStatus = TicketStatus.Assigned,
            ticketType = TicketType.ServiceRequest,
            ticketPriority = TicketPriority.Medium,
            Project = StarterProjects[1],
            SubmitterId = Submitter2.Id,
            UserId = Dev1.Id,
        });

        repoMock.Setup(r => r.Get(It.Is<int>(i => i == 2))).Returns(new Ticket
        {
            Title = "Moq ticket 2 Setup",
            Description = "Description for ticket one.",
            CreatedDate = new DateTime(2022, 05, 01),
            ticketStatus = TicketStatus.Assigned,
            ticketType = TicketType.ServiceRequest,
            ticketPriority = TicketPriority.Medium,
            Project = StarterProjects[1],
            SubmitterId = Submitter2.Id,
            UserId = Dev1.Id,
        });
    }

    [TestMethod]
    public void Get()
    {
        try
        {
            Assert.AreEqual(TicketStatus.Assigned, TicketBll.Get(1).ticketStatus);
        } catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
