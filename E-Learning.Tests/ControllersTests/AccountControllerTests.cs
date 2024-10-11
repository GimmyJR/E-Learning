using Microsoft.Extensions.Configuration;
using E_Learning.Controllers;
using E_Learning.Dto;
using E_Learning.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace E_Learning.Tests.ControllersTests
{
    public class AccountControllerTests
    {
        [Fact]
        public async Task Register_returnOk_WhenUserAddedsuccessfully()
        {
            //arrange
            var mockUserManger = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);
            var mockSignInManger = new Mock<SignInManager<ApplicationUser>>(
                mockUserManger.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                null,null,null,null
                );
            var mockConfiguration = new Mock<IConfiguration>();
            var registerUserDto = new RegisterUserDto
            {
                UserName = "testuser",
                Email = "testuser@example.com",
                Password = "Password123!",
                AvatarUrl = "https://example.com/avatar.jpg",
                Bio = "This is a test user."
            };

            var mockResult = IdentityResult.Success;
            mockUserManger.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(),registerUserDto.Password)).ReturnsAsync(mockResult);
            var controller = new AccountController(mockUserManger.Object,mockConfiguration.Object,mockSignInManger.Object);

            //act
            var result = await controller.Register(registerUserDto);

            //assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenRegistrationFails()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);

            var mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
                mockUserManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                null, null, null, null);

            var mockConfiguration = new Mock<IConfiguration>();

            var registerUserDto = new RegisterUserDto
            {
                UserName = "testuser",
                Email = "testuser@example.com",
                Password = "Password123!",
                AvatarUrl = "https://example.com/avatar.jpg",
                Bio = "This is a test user."
            };

            var mockIdentityErrors = new List<IdentityError>
            {
            new IdentityError { Description = "Error 1" },
            new IdentityError { Description = "Error 2" }
            };

            var mockResult = IdentityResult.Failed(mockIdentityErrors.ToArray());

            // Mock the CreateAsync method to simulate a failed registration
            mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), registerUserDto.Password))
                           .ReturnsAsync(mockResult);

            var controller = new AccountController(mockUserManager.Object, mockConfiguration.Object, mockSignInManager.Object);

            // Act
            var result = await controller.Register(registerUserDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errors = (badRequestResult.Value as IEnumerable<IdentityError>).ToList();

            Assert.Equal(2, errors.Count);
            Assert.Equal("Error 1", errors[0].Description);
            Assert.Equal("Error 2", errors[1].Description);
        }


        [Fact]
        public async Task Register_ReturnsBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);

            var mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
                mockUserManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                null, null, null, null);

            var mockConfiguration = new Mock<IConfiguration>();


            var controller = new AccountController(mockUserManager.Object, mockConfiguration.Object, mockSignInManager.Object);
            controller.ModelState.AddModelError("UserName", "Required");

            var registerUserDto = new RegisterUserDto
            {
                Email = "testuser@example.com",
                Password = "Password123!",
                AvatarUrl = "https://example.com/avatar.jpg",
                Bio = "This is a test user."
            };

            // Act
            var result = await controller.Register(registerUserDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task Logout_ReturnsOk_WhenUserIsSignedOut()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
               new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);

            var mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
                mockUserManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                null, null, null, null);
            mockSignInManager.Setup(sm => sm.SignOutAsync()).Returns(Task.CompletedTask);

            var mockConfiguration = new Mock<IConfiguration>();


            var controller = new AccountController(mockUserManager.Object, mockConfiguration.Object, mockSignInManager.Object);


            // Act
            var result = await controller.logout();

            // Assert
            mockSignInManager.Verify(sm => sm.SignOutAsync(), Times.Once);
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal("{ Message = User logged out successfully }", okResult.Value.ToString());
        }

    }
}
