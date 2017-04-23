using EnterpriseAPI.Controllers;
using EnterpriseAPI.Models;
using EnterpriseAPI.Models.UserModel;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace EnterpriseAPI.Tests
{
    public class AccountControllerTests
    {
        [Fact]
        public void InfoMessage()
        {
            var context = Substitute.For<ApplicationContext>();
            var user = Substitute.For<IUser>();
            AccountController controller = new AccountController(context, user);

            JsonResult result = controller.Info() as JsonResult;

            Assert.Equal("Please authenticate via LinkedIn", result.Value);
        }

        [Fact]
        public void IndexViewDataMessage()
        {
            // Arrange
            var context = Substitute.For<ApplicationContext>();
            var user = Substitute.For<IUser>();
            AccountController controller = new AccountController(context, user);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.Equal("Hello world!", result?.ViewData["Message"]);
        }

        [Fact]
        public void IndexViewResultNotNull()
        {
            // Arrange
            var context = Substitute.For<ApplicationContext>();
            var user = Substitute.For<IUser>();
            AccountController controller = new AccountController(context, user);
            // Act
            ViewResult result = controller.Index() as ViewResult;
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IndexViewNameEqualIndex()
        {
            // Arrange
            var context = Substitute.For<ApplicationContext>();
            var user = Substitute.For<IUser>();
            AccountController controller = new AccountController(context, user);
            // Act
            ViewResult result = controller.Index() as ViewResult;
            // Assert
            Assert.Equal("Index", result?.ViewName);
        }
    }
}
