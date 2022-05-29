using System;
using System.Collections.Generic;
using System.Linq;
using BugTicketingSystemV2.Data;
using BugTicketingSystemV2.Data.BLL;
using BugTicketingSystemV2.Data.DAL;
using BugTicketingSystemV2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BugTicketingSystem_UnitTesting;

[TestClass]
public class BugTesting
{
    public TicketBusinessLogic TicketBll;
    public List<Ticket> AllTickets;
    private RoleManager<IdentityRole> roleManager;
    public AppUser Dev1;

    [TestInitialize]
    public void TestInitialise()
    {
        Mock<TicketRepository> repoMock = new Mock<TicketRepository>();

        SubmitterUser Submitter2 = new SubmitterUser
        {
            Email = "submitter2@mitt.ca",
            NormalizedEmail = "SUBMITTER2@MITT.CA",
            UserName = "submitter2@mitt.ca",
            NormalizedUserName = "SUBMITTER2@MITT.CA",
            EmailConfirmed = true,
        };
        //roleManager.s
        SubmitterUser Submitter3 = new SubmitterUser
        {
            Email = "submitter2@mitt.ca",
            NormalizedEmail = "SUBMITTER2@MITT.CA",
            UserName = "submitter2@mitt.ca",
            NormalizedUserName = "SUBMITTER2@MITT.CA",
            EmailConfirmed = true,
        };

        Dev1 = new AppUser
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

        Ticket Ticket1 = new Ticket
        {
            Id = 1,
            Title = "Moq Ticket1 Setup",
            Description = "Description for ticket one.",
            CreatedDate = new DateTime(2022, 05, 01),
            ticketStatus = TicketStatus.Assigned,
            ticketType = TicketType.ServiceRequest,
            ticketPriority = TicketPriority.Medium,
            Project = StarterProjects[1],
            SubmitterId = Submitter2.Id,
            UserId = Dev1.Id,
        };

        Ticket Ticket2 = new Ticket
        {
            Id = 2,
            Title = "Moq ticket 2 Setup",
            Description = "Description for ticket two.",
            CreatedDate = new DateTime(2022, 05, 01),
            ticketStatus = TicketStatus.Assigned,
            ticketType = TicketType.ServiceRequest,
            ticketPriority = TicketPriority.Medium,
            Project = StarterProjects[2],
            SubmitterId = Submitter2.Id,
            UserId = Dev1.Id,
        };

        AllTickets = new List<Ticket> { Ticket1, Ticket2 };
        repoMock.Setup(r => r.Get(It.Is<int>(i => i == 1))).Returns(Ticket1);
        repoMock.Setup(r => r.Get(It.Is<int>(i => i == 2))).Returns(Ticket2);
        repoMock.Setup(r => r.GetAll()).Returns(AllTickets);
        TicketBll = new TicketBusinessLogic(repoMock.Object);

    }

    [TestMethod]
    public void GetTicket()
    {
        Assert.ThrowsException<Exception>(() =>
        {
            TicketBll.Get(13);
        });

    }

    [TestMethod]
    public void GetTicketWithInvalidNumber()
    {
        Assert.ThrowsException<Exception>(() =>
        {
            TicketBll.Get('c');
        });

    }

    [TestMethod]
    public void GetAllTickets()
    {
           Assert.AreEqual(2, TicketBll.GetAll().Count());
    }

    [TestMethod]
    public void EditTicket()
    {
            // unit testing with repo mock object but ticket repo is used in the ticket bll
                TicketBll.Edit(TicketBll.Get(2), "Ticket Edit", "nada", DateTime.Now, DateTime.Now, TicketStatus.Assigned, TicketType.Incident, TicketPriority.High);
                Assert.AreEqual("Ticket Edit", TicketBll.Get(2).Title);
    }

    [TestMethod]
    public void AddCommentThrowsExceptionWithNullValues()
    {
        Ticket ticket = TicketBll.Get(1);
        Assert.ThrowsException<Exception>(() =>
        {
            TicketBll.AddComment(1, "", DateTime.Now, Dev1);
        });
        //string comment = "";
        
    }

}
