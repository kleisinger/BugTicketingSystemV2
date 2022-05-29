using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BugTicketingSystemV2.Data.BLL;
using BugTicketingSystemV2.Data.DAL;
using BugTicketingSystemV2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


namespace BugTicketingSystem_UnitTesting
{
    [TestClass]
    public class Project_Tests
    {
        private ProjectBusinessLogic _projectBLL;
        private ICollection<Project> allProjects;
        private ICollection<Ticket> allTickets;
        private ICollection<Project> pmProjects;


        [TestInitialize]
        public void Initialize()
        {
            // Creating MOCK Repo (based on DAL)
            Mock<ProjectRepository> mockRepo = new Mock<ProjectRepository>();

            // Creating MOCK Users
            AppUser mockAdmin = new AppUser
            {
                Email = "mockAdmin@mitt.ca",
                NormalizedEmail = "MOCKADMIN@MITT.CA",
                UserName = "mockAdmin@mitt.ca",
                NormalizedUserName = "MOCKADMIN@MITT.CA",
                EmailConfirmed = true
            };
            AppUser mockPM = new AppUser
            {
                Email = "mockPM@mitt.ca",
                NormalizedEmail = "MOCKPM@MITT.CA",
                UserName = "mockPM@mitt.ca",
                NormalizedUserName = "MOCKPM@MITT.CA",
                EmailConfirmed = true
            };
            AppUser mockDev = new AppUser
            {
                Email = "mockDev@mitt.ca",
                NormalizedEmail = "MOCKDEV@MITT.CA",
                UserName = "mockDev@mitt.ca",
                NormalizedUserName = "MOCKDEV@MITT.CA",
                EmailConfirmed = true
            };
            AppUser mockSubmitter = new AppUser
            {
                Email = "mockSubmitter@mitt.ca",
                NormalizedEmail = "MOCKSUBMITTER@MITT.CA",
                UserName = "mockSubmitter@mitt.ca",
                NormalizedUserName = "MOCKSUBMITTER@MITT.CA",
                EmailConfirmed = true
            };

            // Creating MOCK Projects - adding a Users List
            Project mockProject1 = new Project { Id = 1, Title = "The First Project", Description = "Description One", Users = new List<AppUser>() };
            Project mockProject2 = new Project { Id = 2, Title = "The Second Project", Description = "Description Two", Users = new List<AppUser>() };
            Project mockProject3 = new Project { Id = 3, Title = "The Third Project", Description = "Description Three", Users = new List<AppUser>() };

            allProjects = new List<Project> { mockProject1, mockProject2, mockProject3 };

            // Adding PMs to Projects
            mockProject1.Users.Add(mockPM);
            mockProject2.Users.Add(mockPM);
            pmProjects = new List<Project> { mockProject1, mockProject2, mockProject3 };

            // Creating MOCK Tickets
            Ticket mockTicket1 = new Ticket
            {
                Title = "Ticket One",
                Description = "Description for ticket one.",
                CreatedDate = new DateTime(2022, 05, 01),
                ticketStatus = TicketStatus.Assigned,
                ticketType = TicketType.ServiceRequest,
                ticketPriority = TicketPriority.Medium,
                Project = mockProject1,
                SubmitterId = mockSubmitter.Id,
                UserId = mockDev.Id,
            };
            Ticket mockTicket2 = new Ticket
            {
                Title = "Ticket Two",
                Description = "Description for ticket two.",
                CreatedDate = new DateTime(2022, 05, 01),
                ticketStatus = TicketStatus.Assigned,
                ticketType = TicketType.ServiceRequest,
                ticketPriority = TicketPriority.Medium,
                Project = mockProject1,
                SubmitterId = mockSubmitter.Id,
                UserId = mockDev.Id,
            };
            Ticket mockTicket3 = new Ticket
            {
                Title = "Ticket Three",
                Description = "Description for ticket three.",
                CreatedDate = new DateTime(2022, 05, 01),
                ticketStatus = TicketStatus.Assigned,
                ticketType = TicketType.ServiceRequest,
                ticketPriority = TicketPriority.Medium,
                Project = mockProject2,
                SubmitterId = mockSubmitter.Id,
                UserId = mockDev.Id,
            };

            allTickets = new List<Ticket> { mockTicket1, mockTicket2, mockTicket3 };

            // Calling "Get" method from DAL
            // We are creating fake responses for our fake DB
            // We are telling the DAL how to behave when the BLL accesses it
            // Called a testing double
            mockRepo.Setup(repo => repo.Get(It.Is<int>(i => i == 1))).Returns(mockProject1);
            mockRepo.Setup(repo => repo.Get(It.Is<int>(i => i == 2))).Returns(mockProject2);
            mockRepo.Setup(repo => repo.Get(It.Is<int>(i => i == 3))).Returns(mockProject3);
            mockRepo.Setup(repo => repo.GetAll()).Returns(allProjects);
            mockRepo.Setup(repo => repo.GetList(It.IsAny<Func<Project, bool>>())).Returns(pmProjects);

            _projectBLL = new ProjectBusinessLogic(mockRepo.Object);
        }


        // GetProjectById
        [TestMethod]
        public void GetProjectByIdAreEqual()
        {
            var realProject = _projectBLL.GetProjectById(1);
            var mockProject = allProjects.ToList()[0];

            Assert.AreEqual(realProject, mockProject);;
        }

        [TestMethod]
        public void GetProjectByIdAreNoEqual()
        {
            var realProject = _projectBLL.GetProjectById(1);
            var mockProject = allProjects.ToList()[2];

            Assert.AreNotEqual(realProject, mockProject); ;
        }

        [TestMethod]
        public void GetProjectByIdThrowsErrorOnNonExistingProjectId()
        {
            Assert.ThrowsException<Exception>(()=> _projectBLL.GetProjectById(10));
        }

        [TestMethod]
        public void GetProjectByIdHasNoIdPassedIn()
        {
            Assert.ThrowsException<Exception>(() => _projectBLL.GetProjectById('x'));
        }



        // GetAllProjects
        [TestMethod]
        public void GetAllProjectsList()
        {
            var AllRealProjects = _projectBLL.GetAllProjects().ToList();
            var AllMockProjects = allProjects.ToList();

            Assert.AreEqual(AllMockProjects[0], AllRealProjects[0]);
        }

        [TestMethod]
        public void GetCurrentProjects()
        {

        }


        [TestMethod]
        public void CreateProject()
        {

        }

        [TestMethod]
        public void EditProject()
        {

        }

        [TestMethod]
        public void AssignTicket()
        {

        }

        [TestMethod]
        public void AssignProject()
        {

        }
    }
}
