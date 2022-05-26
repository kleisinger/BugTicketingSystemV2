using System;
using System.Collections.Generic;
using BugTicketingSystemV2.Data.BLL;
using BugTicketingSystemV2.Data.DAL;
using BugTicketingSystemV2.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


namespace BugTicketingSystem_UnitTesting
{
    [TestClass]
    public class Project_Tests
    {
        private ProjectBusinessLogic _projectBLL;
        private List<Project> allProjects;


        [TestInitialize]
        public void Initialize()
        {
            // Create Mock Repo (based on DAL)
            Mock<ProjectRepository> mockRepo = new Mock<ProjectRepository>();

            // Create Mock Projects
            Project mockProject1 = new Project { Id = 1, Title = "The First Project", Description = "Description One" };
            Project mockProject2 = new Project { Id = 2, Title = "The Second Project", Description = "Description Two" };
            Project mockProject3 = new Project { Id = 3, Title = "The Third Project", Description = "Description Three" };

            allProjects = new List<Project> { mockProject1, mockProject2, mockProject3 };

            // Calling "Get" method from DAL
            // We are creating fake responses for our fake DB
            // We are telling the DAL how to behave when the BLL accesses it
            // Called a testing double
            mockRepo.Setup(repo => repo.Get(It.Is<int>(i => i == 1))).Returns(mockProject1);
            mockRepo.Setup(repo => repo.Get(It.Is<int>(i => i == 2))).Returns(mockProject2);
            mockRepo.Setup(repo => repo.Get(It.Is<int>(i => i == 3))).Returns(mockProject3);

            _projectBLL = new ProjectBusinessLogic(mockRepo.Object);
        }

        [TestMethod]
        public void GetProjectByIdTestPasses()
        {
            Assert.AreEqual(_projectBLL.GetProjectById(1).Title, "The First Project");
        }

        [TestMethod]
        public void GetProjectByIdTestThrowsErrorOnNonExistingProjectId()
        {
            Assert.ThrowsException<Exception>(()=> _projectBLL.GetProjectById(10));
        }
    }
}
