using E_Learning.Controllers;
using E_Learning.Models;
using E_Learning.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Tests.Controllers.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void GetCourses_ReturnOkResult_WithListOfCourses()
        {
            //arrange
            var mockRepo = new Mock<IHomeRepository>();
            var coursesList = new List<Course>
            {
                 new Course
                 {
                     Id = 1,
                     Title = "C# Basics",
                     Description = "Learn the basics of C#.",
                     Price = 49.99m,
                     Category = "Programming",
                     Level = CourseLevel.Beginner,
                     InstructorId = "123",
                     Instructor = new ApplicationUser { Id = "123", UserName = "Instructor1" }, // Assuming ApplicationUser has Id and UserName
                     ImageUrl = "image1.jpg",
                     Rating = 4.5,
                     CreatedDate = DateTime.Now.AddMonths(-1),
                     UpdatedDate = DateTime.Now,
                     Duration = TimeSpan.FromHours(10),
                     TotalEnrolled = 100,
                     Language = "English",
                     Requirements = "Basic computer knowledge"
                 },
                 new Course
                 {
                     Id = 2,
                     Title = "ASP.NET Core",
                     Description = "Build web applications using ASP.NET Core.",
                     Price = 79.99m,
                     Category = "Web Development",
                     Level = CourseLevel.Intermediate,
                     InstructorId = "124",
                     Instructor = new ApplicationUser { Id = "124", UserName = "Instructor2" },
                     ImageUrl = "image2.jpg",
                     Rating = 4.8,
                     CreatedDate = DateTime.Now.AddMonths(-2),
                     UpdatedDate = DateTime.Now,
                     Duration = TimeSpan.FromHours(20),
                     TotalEnrolled = 150,
                     Language = "English",
                     Requirements = "Basic C# knowledge"
                 }
            };
            mockRepo.Setup(repo => repo.GetCourses()).Returns(coursesList);
            var controller = new HomeController(mockRepo.Object);

            //act
            var result = controller.GetCourses();

            //assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Course>>(okResult.Value);

            Assert.Equal(2, returnValue.Count);
            Assert.Equal("C# Basics", returnValue[0].Title);
            Assert.Equal(49.99m, returnValue[0].Price);
            Assert.Equal(CourseLevel.Beginner, returnValue[0].Level);
            Assert.Equal("Instructor1", returnValue[0].Instructor.UserName);
            Assert.Equal(4.5, returnValue[0].Rating);
            Assert.Equal(100, returnValue[0].TotalEnrolled);

            Assert.Equal("ASP.NET Core", returnValue[1].Title);
            Assert.Equal("124", returnValue[1].Instructor.Id);

        }
        [Fact]
        public void GetCourseDetails_ReturnsNotFound_WhenCourseIsNull()
        {
            //arrange

            var mockRepo = new Mock<IHomeRepository>();
            int courseId = 1;
            mockRepo.Setup(repo => repo.GetCourseDetails(It.IsAny<int>())).Returns((Course)null);
            var controller = new HomeController(mockRepo.Object);

            //act

             var result = controller.GetCourseDetails(courseId);

            //assert

            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public void GetCourseDetails_ReturnsOkObjectResult_WhenCourseIsFound()
        {
            //arrange

            var mockRepo = new Mock<IHomeRepository>();
            int courseId = 1;

            var course = new Course
            {
                Id = courseId,
                Title = "C# Basics",
                Description = "Learn the basics of C#.",
                Price = 49.99m,
                Category = "Programming",
                Level = CourseLevel.Beginner,
                InstructorId = "123",
                ImageUrl = "image1.jpg",
                Rating = 4.5,
                CreatedDate = DateTime.Now.AddMonths(-1),
                UpdatedDate = DateTime.Now,
                Duration = TimeSpan.FromHours(10),
                TotalEnrolled = 100,
                Language = "English",
                Requirements = "Basic computer knowledge"
            };

            mockRepo.Setup(repo => repo.GetCourseDetails(courseId)).Returns(course);

            var controller = new HomeController(mockRepo.Object);
            //act
            var result = controller.GetCourseDetails(courseId);
            //assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var ReturnValue = Assert.IsType<Course>(okResult.Value);
            Assert.Equal(courseId, ReturnValue.Id);
            Assert.Equal(4.5, ReturnValue.Rating);
        }

        [Fact]
        public void SearchCourses_ReturnOkObjectResult_WithListOfCourses()
        {
            //arrange
            var mockRepo = new Mock<IHomeRepository>();
            var searchQuery = "C#";
            var coursesList = new List<Course>
            {
                 new Course
                 {
                     Id = 1,
                     Title = "C# Basics",
                     Description = "Learn the basics of C#.",
                     Price = 49.99m,
                     Category = "Programming",
                     Level = CourseLevel.Beginner,
                     InstructorId = "123",
                     Instructor = new ApplicationUser { Id = "123", UserName = "Instructor1" }, // Assuming ApplicationUser has Id and UserName
                     ImageUrl = "image1.jpg",
                     Rating = 4.5,
                     CreatedDate = DateTime.Now.AddMonths(-1),
                     UpdatedDate = DateTime.Now,
                     Duration = TimeSpan.FromHours(10),
                     TotalEnrolled = 100,
                     Language = "English",
                     Requirements = "Basic computer knowledge"
                 },
                 new Course
                 {
                     Id = 2,
                     Title = "Advanced",
                     Description = "Build web applications using ASP.NET Core.",
                     Price = 79.99m,
                     Category = "C# Development",
                     Level = CourseLevel.Intermediate,
                     InstructorId = "124",
                     Instructor = new ApplicationUser { Id = "124", UserName = "Instructor2" },
                     ImageUrl = "image2.jpg",
                     Rating = 4.8,
                     CreatedDate = DateTime.Now.AddMonths(-2),
                     UpdatedDate = DateTime.Now,
                     Duration = TimeSpan.FromHours(20),
                     TotalEnrolled = 150,
                     Language = "English",
                     Requirements = "Basic C# knowledge"
                 }
            };

            mockRepo.Setup(repo => repo.SearchCourses(searchQuery)).Returns(coursesList);
            var controller = new HomeController(mockRepo.Object);

            //act

            var result = controller.SearchCourses(searchQuery);

            //assign
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Course>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
            Assert.Equal("C# Basics", returnValue[0].Title);
            Assert.Equal("C# Development", returnValue[1].Category);

        }


        [Fact]
        public void SearchCourses_ReturnOkObjectResult_WithEmptyList_WhenNoCourseSMatch()
        {
            //arrange

            var mockRepo = new Mock<IHomeRepository>();
            var searchQuery = "C#";
            mockRepo.Setup(repo => repo.SearchCourses(searchQuery)).Returns(new List<Course>());
            var controller = new HomeController(mockRepo.Object);

            //act

            var result = controller.SearchCourses(searchQuery);

            //assert

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Course>>(okResult?.Value);
            Assert.Empty(returnValue);
        }
        [Fact]
        public void FilterCourses_ReturnOkObjectResult_WithListOfCourses()
        {
            //arrange
            var mockRepo = new Mock<IHomeRepository>();
            string category = "Programming";
            string level = "Beginner";
            var coursesList = new List<Course>
            {
                 new Course
                 {
                     Id = 1,
                     Title = "C# Basics",
                     Description = "Learn the basics of C#.",
                     Price = 49.99m,
                     Category = "Programming",
                     Level = CourseLevel.Beginner,
                     InstructorId = "123",
                     Instructor = new ApplicationUser { Id = "123", UserName = "Instructor1" }, // Assuming ApplicationUser has Id and UserName
                     ImageUrl = "image1.jpg",
                     Rating = 4.5,
                     CreatedDate = DateTime.Now.AddMonths(-1),
                     UpdatedDate = DateTime.Now,
                     Duration = TimeSpan.FromHours(10),
                     TotalEnrolled = 100,
                     Language = "English",
                     Requirements = "Basic computer knowledge"
                 },
                 new Course
                 {
                     Id = 2,
                     Title = "ASP.NET Core",
                     Description = "Build web applications using ASP.NET Core.",
                     Price = 79.99m,
                     Category = "Programming",
                     Level = CourseLevel.Beginner,
                     InstructorId = "124",
                     Instructor = new ApplicationUser { Id = "124", UserName = "Instructor2" },
                     ImageUrl = "image2.jpg",
                     Rating = 4.8,
                     CreatedDate = DateTime.Now.AddMonths(-2),
                     UpdatedDate = DateTime.Now,
                     Duration = TimeSpan.FromHours(20),
                     TotalEnrolled = 150,
                     Language = "English",
                     Requirements = "Basic C# knowledge"
                 }
            };

            mockRepo.Setup(repo => repo.FilterCourses(category,level)).Returns(coursesList);
            var controller = new HomeController(mockRepo.Object);

            //act

            var result = controller.FilterCourses(category, level);

            //assign
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Course>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
            Assert.Equal("C# Basics", returnValue[0].Title);
            Assert.Equal("ASP.NET Core", returnValue[1].Title);

        }


        [Fact]
        public void FilterCourses_ReturnOkObjectResult_WithEmptyList_WhenNoCourseSMatch()
        {
            //arrange

            var mockRepo = new Mock<IHomeRepository>();
            string category = "Programming";
            string level = "Beginner";
            mockRepo.Setup(repo => repo.FilterCourses(category, level)).Returns(new List<Course>());
            var controller = new HomeController(mockRepo.Object);

            //act

            var result = controller.FilterCourses(category, level);

            //assert

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Course>>(okResult?.Value);
            Assert.Empty(returnValue);
        }
    }
}
