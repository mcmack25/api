using ip_test_api.Data;
using ip_test_api.Data.Entities;
using ip_test_api.Data.Enums;
using ip_test_api.Models;
using ip_test_api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NSubstitute;

namespace ip_test_api.tests.Services;

[TestFixture]
public class UserServiceTests
{
    private UsersContext _context;
    private IUserService _userService;

    [SetUp]
    public void SetUp()
    {
        var option = new DbContextOptionsBuilder<UsersContext>()
                .UseInMemoryDatabase($"UsersTestsDb")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

        _context = Substitute.ForPartsOf<UsersContext>(option);
        _userService = new UserService(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task AddUserAsync_ShouldReturnCreated_WhenUserIsAddedSuccessfully()
    {
        // Arrange
        var user = new UserDto
        {
            UserName = "user3",
            FirstName = "Bob",
            LastName = "Smith",
            Email = "bob.smith@example.com",
            UserStatus = UserStatusType.Active,
            Department = "Sales"
        };
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _userService.AddUserAsync(user, cancellationToken);

        // Assert
        var addedUser = await _context.Users.SingleOrDefaultAsync(u => u.UserName == user.UserName, cancellationToken);
        Assert.That(addedUser, Is.Not.Null);
        Assert.That(addedUser.FirstName, Is.EqualTo(user.FirstName));
        Assert.That(addedUser.LastName, Is.EqualTo(user.LastName));
        Assert.That(addedUser.Email, Is.EqualTo(user.Email));
        Assert.That(addedUser.UserStatus, Is.EqualTo(user.UserStatus));
        Assert.That(addedUser.Department, Is.EqualTo(user.Department));
    }

    [Test]
    public async Task AddUserAsync_ShouldReturnConflict_WhenUserWithSameUserNameAlreadyExists()
    {
        // Arrange
        _context.Users.Add(new UserEntity
        {
            Id = 1,
            UserName = "user1",
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            UserStatus = UserStatusType.Active,
            Department = "IT"
        });

        _context.SaveChanges();

        var user = new UserDto
        {
            UserName = "user1",
            FirstName = "Bob",
            LastName = "Smith",
            Email = "bob.smith@example.com",
            UserStatus = UserStatusType.Active,
            Department = "Sales"
        };
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _userService.AddUserAsync(user, cancellationToken);

        // Assert
        Assert.That(result.Errors[0].Message, Is.EqualTo("User with the same user name already exists."));
    }

    [Test]
    public async Task DeleteUserAsync_UserExists_ReturnsResultDeleted()
    {
        // Arrange
        var user = new UserEntity
        {
            UserName = "user1",
            FirstName = "Bob",
            LastName = "Smith",
            Email = "bob.smith@example.com",
            UserStatus = UserStatusType.Active,
            Department = "Sales"
        };
        _context.Users.Add(user);
        _context.SaveChanges();

        // Act
        var result = await _userService.DeleteUserAsync(user.Id, CancellationToken.None);

        // Assert
        Assert.That(_context.Users.Any(u => u.Id == user.Id), Is.False);
    }

    [Test]
    public async Task DeleteUserAsync_UserDoesNotExist_ReturnsErrorNotFound()
    {
        // Arrange

        // Act
        var result = await _userService.DeleteUserAsync(1, CancellationToken.None);

        // Assert
        Assert.That(result.Errors[0].Message, Is.EqualTo("User is not found."));
    }

    [Test]
    public async Task GetUserAsync_UserExists_ReturnsUser()
    {
        // Arrange
        var user = new UserEntity
        {
            UserName = "user1",
            FirstName = "Bob",
            LastName = "Smith",
            Email = "bob.smith@example.com",
            UserStatus = UserStatusType.Active,
            Department = "Sales"
        };
        _context.Users.Add(user);
        _context.SaveChanges();

        // Act
        var result = await _userService.GetUserAsync(user.Id, CancellationToken.None);

        // Assert
        Assert.That(result.Value.Id, Is.EqualTo(user.Id));
    }

    [Test]
    public async Task GetUserAsync_UserDoesNotExist_ReturnsErrorNotFound()
    {
        // Arrange

        // Act
        var result = await _userService.GetUserAsync(1, CancellationToken.None);

        // Assert
        Assert.That(result.Errors[0].Message, Is.EqualTo("User is not found."));
    }

    [Test]
    public async Task GetUsersAsync_UsersExist_ReturnsUsers()
    {
        // Arrange
        var users = new[]
        {
            new UserEntity {
                UserName = "User1",
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@example.com",
                UserStatus = UserStatusType.Active
            },
            new UserEntity {
                UserName = "User2",
                FirstName = "J1ohn",
                LastName = "D2oe",
                Email = "johndo4e@example.com",
                UserStatus = UserStatusType.Active
            },
            new UserEntity {
                UserName = "User3",
                FirstName = "Jo6hn",
                LastName = "4Doe",
                Email = "johndo2e@example.com",
                UserStatus = UserStatusType.Active
            },
        };
        _context.Users.AddRange(users);
        _context.SaveChanges();

        // Act
        var result = await _userService.GetUsersAsync(CancellationToken.None);

        // Assert
        Assert.That(result.Value.Select(u => u.UserName), Is.EquivalentTo(users.Select(u => u.UserName)));
    }
    [Test]
    public async Task UpdateUserAsync_ExistingUser_ReturnsUpdated()
    {
        // Arrange
        var user = new UserDto
        {
            UserName = "johndoe",
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com",
            UserStatus = UserStatusType.Active,
            Department = "sales"
        };
        await _userService.AddUserAsync(user, default);
        var updatedUser = new UserDto
        {
            UserName = "janedoe",
            FirstName = "Jane",
            LastName = "Doe",
            Email = "janedoe@example.com",
            UserStatus = UserStatusType.Inactive,
            Department = "marketing"
        };

        // Act
        await _userService.UpdateUserAsync(1, updatedUser, default);

        // Assert
        var userFromDb = await _userService.GetUserAsync(1, default);
        Assert.That(userFromDb.Value.UserName, Is.EqualTo(updatedUser.UserName));
        Assert.That(userFromDb.Value.FirstName, Is.EqualTo(updatedUser.FirstName));
        Assert.That(userFromDb.Value.LastName, Is.EqualTo(updatedUser.LastName));
        Assert.That(userFromDb.Value.Email, Is.EqualTo(updatedUser.Email));
        Assert.That(userFromDb.Value.UserStatus, Is.EqualTo(updatedUser.UserStatus));
        Assert.That(userFromDb.Value.Department, Is.EqualTo(updatedUser.Department));
    }

    [Test]
    public async Task UpdateUserAsync_NonexistentUser_ReturnsNotFound()
    {
        // Arrange
        var updatedUser = new UserDto
        {
            UserName = "janedoe",
            FirstName = "Jane",
            LastName = "Doe",
            Email = "janedoe@example.com",
            UserStatus = UserStatusType.Inactive,
            Department = "marketing"
        };

        // Act
        var result = await _userService.UpdateUserAsync(1, updatedUser, default);

        // Assert
        Assert.That(result.Errors[0].Message, Is.EqualTo("User is not found."));
    }
}

