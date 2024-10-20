using E_Learning.Controllers;
using E_Learning.Models;
using E_Learning.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Tests.ControllersTests
{
    public class EnrollmentControllerTests
    {
        [Fact]
        public async Task EnrollInCourse_ReturnOk_WhenEnrollmentIsSuccessful()
        {
            //arrange
            var mockUserManger = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object,null, null, null, null, null, null, null, null);
            var mockCourse = new Mock<IEnrollmentRepository>();
            var user = new ApplicationUser { Id = "user123", UserName = "TestUser" };

            var enrollment = new Enrollment
            {
                Id = 1,
                UserId = user.Id,
                CourseId = 2,
                EnrollmentDate = DateTime.Now,
            };
            mockUserManger.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            mockCourse.Setup(repo => repo.EnrollInCourse(user, 2))
                      .ReturnsAsync(enrollment);
            var controller = new EnrollmentController(mockUserManger.Object, mockCourse.Object);
           
            //act
            var result =await controller.EnrollInCourse(2);
            
            //assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Enrollment>(okResult.Value);
            Assert.Equal(enrollment.Id, returnValue.Id);
            Assert.Equal(enrollment.UserId, returnValue.UserId);
            Assert.Equal(enrollment.CourseId, returnValue.CourseId);
        }

        [Fact]
        public async Task EnrollInCourse_ReturnsNotFound_WhenEnrollmentFails()
        {
            //arrange
            var mockUserManger = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);
            var mockCourse = new Mock<IEnrollmentRepository>();
            var user = new ApplicationUser { Id = "user123", UserName = "TestUser" };
            mockUserManger.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            mockCourse.Setup(repo => repo.EnrollInCourse(user, 2))
                      .ReturnsAsync((Enrollment)null);
            var controller = new EnrollmentController(mockUserManger.Object, mockCourse.Object);

            //act
            var result = await controller.EnrollInCourse(2);
            
            //assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UnenrollFromCourse_ReturnsNoContent_WhenUnenrollmentIsSuccessful()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(),
                null, null, null, null, null, null, null, null);

            var mockEnrollmentRepo = new Mock<IEnrollmentRepository>();

            var user = new ApplicationUser
            {
                Id = "user123",
                UserName = "testuser"
            };

            var enrollment = new Enrollment
            {
                Id = 1,
                UserId = user.Id,
                CourseId = 2,
                EnrollmentDate = DateTime.Now
            };

            mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                           .ReturnsAsync(user);

            mockEnrollmentRepo.Setup(repo => repo.UnenrollFromCourse(user, 2))
                              .ReturnsAsync(enrollment);

            var controller = new EnrollmentController(mockUserManager.Object, mockEnrollmentRepo.Object);

            // Act
            var result = await controller.UnenrollFromCourse(2);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UnenrollFromCourse_ReturnsNotFound_WhenEnrollmentDoesNotExist()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(),
                null, null, null, null, null, null, null, null);

            var mockEnrollmentRepo = new Mock<IEnrollmentRepository>();

            var user = new ApplicationUser
            {
                Id = "user123",
                UserName = "testuser"
            };

            mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                           .ReturnsAsync(user);

            mockEnrollmentRepo.Setup(repo => repo.UnenrollFromCourse(user, 2))
                              .ReturnsAsync((Enrollment)null);  // Simulate no enrollment found

            var controller = new EnrollmentController(mockUserManager.Object, mockEnrollmentRepo.Object);

            // Act
            var result = await controller.UnenrollFromCourse(2);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

    }
}
