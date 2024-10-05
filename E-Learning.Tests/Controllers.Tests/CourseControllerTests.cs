using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using E_Learning.Controllers; 
using E_Learning.Repository; 
using System.Threading.Tasks;
using System.Collections.Generic;
using E_Learning.Models;

namespace E_Learning.Tests.Controllers.Tests
{
    public class CourseControllerTests
    {
        [Fact]
        public async Task CreateCourse_ReturnsCreatedAtAction_WhenModelIsValid()
        {
            // Arrange
            var mockRepo = new Mock<ICourseRepository>();
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(),
                null, null, null, null, null, null, null, null);

            var user = new ApplicationUser { UserName = "testuser" };
            var newCourse = new Course
            {
                Title = "New Course",
                Description = "Course Description",
                Price = 49.99M,
                Category = "Programming",
                Level = CourseLevel.Beginner,
                Rating = 0,
                Duration = new TimeSpan(5, 0, 0),
                TotalEnrolled = 0,
                Language = "English",
                Requirements = "Basic knowledge",
                Reviews = new List<Review>()
            };

            mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(user);

            mockRepo.Setup(repo => repo.CreateCourse(It.IsAny<Course>(), It.IsAny<ApplicationUser>()))
                .Returns(Task.CompletedTask);

            var controller = new CourseController(mockUserManager.Object, mockRepo.Object);

            // Act
            var result = await controller.CreateCourse(newCourse);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(controller.CreateCourse), createdAtActionResult.ActionName);

            var createdCourse = Assert.IsType<Course>(createdAtActionResult.Value);

            Assert.Equal(newCourse.Title, createdCourse.Title);
            Assert.Equal(newCourse.Description, createdCourse.Description);
            Assert.Equal(newCourse.Price, createdCourse.Price);
            Assert.Equal(newCourse.Category, createdCourse.Category);
            Assert.Equal(newCourse.Level, createdCourse.Level);
            Assert.Equal(newCourse.Language, createdCourse.Language);
            Assert.Equal(newCourse.Requirements, createdCourse.Requirements);

            Assert.NotNull(createdCourse.Reviews);
            Assert.Empty(createdCourse.Reviews);  
        }

        [Fact]
        public async Task CreateCourse_ReturnsBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            var mockRepo = new Mock<ICourseRepository>();
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(),
                null, null, null, null, null, null, null, null);

            var controller = new CourseController(mockUserManager.Object, mockRepo.Object);

            
            controller.ModelState.AddModelError("Title", "Required");

            var newCourse = new Course { Description = "Invalid Course" };

            // Act
            var result = await controller.CreateCourse(newCourse);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var modelState = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.True(modelState.ContainsKey("Title"));
        }
        [Fact]
        public async Task UpdateCourse_ReturnsOk_WhenCourseIsUpdatedSuccessfully()
        {
            // Arrange
            var mockRepo = new Mock<ICourseRepository>();
            var courseId = 1;
            var existingCourse = new Course
            {
                Id = courseId,
                Title = "Old Title",
                Description = "Old Description",
                Price = 19.99M,
                Category = "Programming",
                Level = CourseLevel.Beginner
            };

            var updatedCourse = new Course
            {
                Title = "New Title",
                Description = "New Description",
                Price = 29.99M,
                Category = "Advanced Programming",
                Level = CourseLevel.Advanced
            };

            mockRepo.Setup(repo => repo.UpdateCourse(courseId, updatedCourse))
                .ReturnsAsync(existingCourse);

            var controller = new CourseController(null!, mockRepo.Object);

            // Act
            var result = await controller.UpdateCourse(courseId, updatedCourse);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCourse = Assert.IsType<Task<Course>>(okResult.Value);
            Assert.Equal(updatedCourse.Title, returnedCourse.Result.Title);
            Assert.Equal(updatedCourse.Description, returnedCourse.Result.Description);
            Assert.Equal(updatedCourse.Price, returnedCourse.Result.Price);
            Assert.Equal(updatedCourse.Category, returnedCourse.Result.Category);
        }

        [Fact]
        public async Task UpdateCourse_ReturnsNotFound_WhenCourseDoesNotExist()
        {
            // Arrange
            var mockRepo = new Mock<ICourseRepository>();
            var courseId = 1;
            var updatedCourse = new Course
            {
                Title = "New Title",
                Description = "New Description",
                Price = 29.99M,
                Category = "Advanced Programming",
                Level = CourseLevel.Advanced
            };

            
            mockRepo.Setup(repo => repo.UpdateCourse(courseId, updatedCourse))
                .ReturnsAsync((Course)null);

            var controller = new CourseController(null!, mockRepo.Object);

            // Act
            var result = await controller.UpdateCourse(courseId, updatedCourse);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateCourse_ReturnsBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            var mockRepo = new Mock<ICourseRepository>();
            var courseId = 1;
            var updatedCourse = new Course
            {
                Title = "New Title",
                Description = "New Description",
                Price = 29.99M,
                Category = "Advanced Programming",
                Level = CourseLevel.Advanced
            };

            var controller = new CourseController(null!, mockRepo.Object);

            
            controller.ModelState.AddModelError("Title", "Required");

            // Act
            var result = await controller.UpdateCourse(courseId, updatedCourse);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var modelState = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.True(modelState.ContainsKey("Title"));
        }


        [Fact]
        public async Task DeleteCourse_ReturnsNoContent_WhenCourseIsDeletedSuccessfully()
        {
            // Arrange
            var mockRepo = new Mock<ICourseRepository>();
            var courseId = 1;

            mockRepo.Setup(repo => repo.DeleteCourse(courseId))
                .Returns(Task.CompletedTask);  

            var controller = new CourseController(null!, mockRepo.Object);

            // Act
            var result = await controller.DeleteCourse(courseId);

            // Assert
            mockRepo.Verify(repo => repo.DeleteCourse(courseId), Times.Once);

            Assert.IsType<NoContentResult>(result);
        }

    }
}
