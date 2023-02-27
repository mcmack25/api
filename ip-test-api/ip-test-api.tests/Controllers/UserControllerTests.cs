using ip_test_api.Controllers;
using ip_test_api.Data.Entities;
using ip_test_api.Data.Enums;
using ip_test_api.Services;
using FluentResults;
using NSubstitute;

namespace ip_test_api.tests.Controllers;

[TestFixture]
public class UserControllerTests
{
    private IUserService _userService = Substitute.For<IUserService>();

    [Test]
    public void Get_ShouldReturnCorrectUsers()
    {
        // Arrange
        var users = new List<UserEntity> {new UserEntity
        {
            Id = 1,
            UserName = "user3",
            FirstName = "Bob",
            LastName = "Smith",
            Email = "bob.smith@example.com",
            UserStatus = UserStatusType.Active,
            Department = "Sales"
        } };
        _userService.GetUsersAsync(default).Returns(users);
        var controller = new UserController(_userService);

        // Act
        var result = controller.Get(default);

        // Assert
        Assert.That((
            result.Result as Microsoft.AspNetCore.Mvc.OkObjectResult)!.Value,
            Is.EqualTo(users)
        );
    }

    [Test]
    public void Get_ShouldReturnCorrectUser()
    {
        // Arrange
        var user = new UserEntity
        {
            Id = 1,
            UserName = "user3",
            FirstName = "Bob",
            LastName = "Smith",
            Email = "bob.smith@example.com",
            UserStatus = UserStatusType.Active,
            Department = "Sales"
        };
        _userService.GetUserAsync(default, default).Returns(user);
        var controller = new UserController(_userService);

        // Act
        var result = controller.Get(default, default);

        // Assert
        Assert.That((
            result.Result as Microsoft.AspNetCore.Mvc.OkObjectResult)!.Value,
            Is.EqualTo(user)
        );
    }

    [Test]
    public void Get_ShouldBeSuccessful()
    {
        // Arrange
        _userService.AddUserAsync(default!, default).Returns(Result.Ok());
        var controller = new UserController(_userService);

        // Act
        var result = controller.Post(default!, default);

        // Assert
        Assert.That(result.IsCompletedSuccessfully, Is.True);
    }

    [Test]
    public void Put_ShouldBeSuccessful()
    {
        // Arrange
        _userService.UpdateUserAsync(default, default!, default).Returns(Result.Ok());
        var controller = new UserController(_userService);

        // Act
        var result = controller.Put(default, default!, default);

        // Assert
        Assert.That(result.IsCompletedSuccessfully, Is.True);
    }

    [Test]
    public void Delete_ShouldBeSuccessful()
    {
        // Arrange
        _userService.DeleteUserAsync(default, default).Returns(Result.Ok());
        var controller = new UserController(_userService);

        // Act
        var result = controller.Delete(default, default);

        // Assert
        Assert.That(result.IsCompletedSuccessfully, Is.True);
    }
}
