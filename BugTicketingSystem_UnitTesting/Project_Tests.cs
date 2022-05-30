using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
        private List<Project> allProjects;
        private List<Ticket> allTickets;
        private List<Project> pmProjects;
        private List<AppUser> allUsers;


        [TestInitialize]
        public void Initialize()
        {

            // Creating MOCK Repo (based on DAL)
            Mock<ProjectRepository> mockRepo = new Mock<ProjectRepository>();
            _projectBLL = new ProjectBusinessLogic(mockRepo.Object);


            //// Creating MOCK Users

            AppUser mockUser = new AppUser
            {
                Id = "mockGUID",
                Email = "mockAdmin@mitt.ca",
                NormalizedEmail = "MOCKADMIN@MITT.CA",
                UserName = "mockAdmin@mitt.ca",
                NormalizedUserName = "MOCKADMIN@MITT.CA",
                EmailConfirmed = true
            };
            AppUser mockUser2 = new AppUser
            {
                Id = "mockGUID2",
                Email = "mockPM@mitt.ca",
                NormalizedEmail = "MOCKPM@MITT.CA",
                UserName = "mockPM@mitt.ca",
                NormalizedUserName = "MOCKPM@MITT.CA",
                EmailConfirmed = true
            };

            allUsers = new List<AppUser> { mockUser, mockUser2 };
 

            // Creating MOCK Projects - adding a Users List
            Project mockProject1 = new Project { Id = 1, Title = "The First Project", Description = "Description One", Users = new List<AppUser>() };
            Project mockProject2 = new Project { Id = 2, Title = "The Second Project", Description = "Description Two", Users = new List<AppUser>() };
            Project mockProject3 = new Project { Id = 3, Title = "The Third Project", Description = "Description Three", Users = new List<AppUser>() };

            allProjects = new List<Project> { mockProject1, mockProject2, mockProject3 };

            // Adding PMs to Projects
            mockProject1.Users.Add(mockUser);
            mockProject2.Users.Add(mockUser);
            pmProjects = new List<Project> { mockProject1, mockProject2, mockProject3 };

            // Adding Projects to User
            mockUser.Projects.Add(mockProject1);
            mockUser.Projects.Add(mockProject2);

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
                SubmitterId = mockUser2.Id,
                UserId = mockUser.Id,
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
                SubmitterId = mockUser2.Id,
                UserId = mockUser.Id,
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
                SubmitterId = mockUser2.Id,
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
            mockRepo.Setup(repo => repo.CreateProject(It.IsAny<Project>())).Callback<Project>((project) => allProjects.Add(project));
            mockRepo.Setup(repo => repo.Remove(It.IsAny<Project>())).Callback<Project>((project) => allProjects.Remove(project));

        }

        // TESTING - Arrange - Act - Assert
        // GetProjectById
        [TestMethod]
        public void GetProjectById_Passes_WithCorrectParametersPassed()
        {
            var realProject = _projectBLL.GetProjectById(1);
            var mockProject = allProjects.ToList()[0];

            Assert.AreEqual(realProject, mockProject);
        }

        [TestMethod]
        public void GetProjectById_Fails_WithIncorrectIdPassed()
        {
            var realProject = _projectBLL.GetProjectById(1);
            var mockProject = allProjects.ToList()[2];

            Assert.AreNotEqual(realProject, mockProject); ;
        }

        [TestMethod]
        public void GetProjectById_ThrowsErrorOn_NonExistingProjectId()
        {
            Assert.ThrowsException<Exception>(()=> _projectBLL.GetProjectById(10));
        }

        [TestMethod]
        public void GetProjectById_ThrowsErrorOn_NoIdPassedIn()
        {
            Assert.ThrowsException<Exception>(() => _projectBLL.GetProjectById('x'));
        }


        // GetAllProjects
        [TestMethod]
        public void GetAllProjectsList_Passes_WithCorrectParametersPassed()
        {
            var AllRealProjects = _projectBLL.GetAllProjects().ToList();
            var AllMockProjects = allProjects.ToList();

            CollectionAssert.AreEqual(AllMockProjects, AllRealProjects);
        }

        [TestMethod]
        public void GetCurrentProjects_Passes_WithCorrectParametersPassed()
        {
            //var pm = allUsers[1];

            //var ProjectList = _projectBLL.GetCurrentProjects(pm);

            //List<Project> expectedAssignedProjects = new List<Project> { allProjects[0], allProjects[1] };

            //Assert.AreEqual(expectedAssignedProjects, ProjectList);
        }


        [TestMethod]
        public void CreateProject_Passes_WithCorrectParametersPassed()
        {
           _projectBLL.CreateProject("test Project", "testing new project creation");
           _projectBLL.CreateProject("Another Test", "testing new project creation");

            var result = allProjects[3];
            var result2 = allProjects[4];

            CollectionAssert.Contains(allProjects, result); 
            CollectionAssert.Contains(allProjects, result2);
        }


        [TestMethod]
        public void EditProject_Passes_WithCorrectParametersPassed()
        {
            Project mockProject1 = new Project { Id = 1, Title = "The First Project", Description = "Description One", Users = new List<AppUser>() };

            _projectBLL.EditProject(1, "Changed Title", "Changed Description");
            var editedProject = allProjects[1];

            Assert.AreNotEqual(mockProject1, editedProject);
        }

        [TestMethod]
        public void EditProject_ThrowsErrorOn_WithNonExistingProjectId()
        {
            Assert.ThrowsException<Exception>(() => _projectBLL.EditProject(100, "Changed Title", "Changed Description"));
        }


        [TestMethod]
        public void AssignTicket_Passes_WithCorrectParameters()
        {
            AppUser mockUser = allUsers[0];

            _projectBLL.AssignTicket(mockUser.Id, allTickets[2]);

            var assignedTicket = allTickets[2];

            Assert.AreEqual(assignedTicket.UserId, "mockGUID");
        }

        [TestMethod]
        public void AssignTicket_ThrowsErrorOn_NullUserId()
        {
            AppUser mockUser = new AppUser
            {
                Id = null,
                Email = "mockAdmin@mitt.ca",
                NormalizedEmail = "MOCKADMIN@MITT.CA",
                UserName = "mockAdmin@mitt.ca",
                NormalizedUserName = "MOCKADMIN@MITT.CA",
                EmailConfirmed = true
            };

            Assert.ThrowsException<Exception>(() => _projectBLL.AssignTicket(mockUser.Id, allTickets[2]));
        }

        [TestMethod]
        public void AssignProject_Passes_WithCorrectParameters()
        {
            AppUser mockUser = allUsers[0];

            _projectBLL.AssignProject(mockUser, allProjects[2].Id);
            var assignedProject = allProjects[2];

            CollectionAssert.Contains(assignedProject.Users.ToList(), mockUser);
        }

    }
}
