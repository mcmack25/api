using FluentResults;
using ip_test_api.Data.Entities;
using ip_test_api.Data;
using ip_test_api.Models;
using Microsoft.EntityFrameworkCore;

namespace ip_test_api.Services;

public class UserService : IUserService
{
    private readonly UsersContext _context;

    public UserService(UsersContext context)
    {
        _context = context;
    }

    public async Task<Result> AddUserAsync(UserDto user, CancellationToken cancellationToken)
    {
        var newUser = new UserEntity
        {
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            UserStatus = user.UserStatus,
            Department = user.Department
        };

        if (await _context.Users
            .SingleOrDefaultAsync(u => u.UserName == user.UserName, cancellationToken: cancellationToken) is not null)
        {
            return Result.Fail("User with the same user name already exists.");
        }

        _context.Users.Add(newUser);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    public async Task<Result> DeleteUserAsync(int id, CancellationToken cancellationToken)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id, cancellationToken);

        if (user is null)
        {
            return Result.Fail("User is not found.");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }

    public async Task<Result<UserEntity>> GetUserAsync(int id, CancellationToken cancellationToken)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id, cancellationToken);

        if (user is null)
        {
            return Result.Fail("User is not found.");
        }

        return user;
    }

    public async Task<Result<IEnumerable<UserEntity>>> GetUsersAsync(CancellationToken cancellationToken)
    {
        return await _context.Users.ToListAsync(cancellationToken);
    }

    public async Task<Result> UpdateUserAsync(int id, UserDto user, CancellationToken cancellationToken)
    {
        var updatedUser = await _context.Users.SingleOrDefaultAsync(u => u.Id == id, cancellationToken: cancellationToken);
        if (updatedUser is null)
        {
            return Result.Fail("User is not found.");
        }

        updatedUser.UserName = user.UserName;
        updatedUser.FirstName = user.FirstName;
        updatedUser.LastName = user.LastName;
        updatedUser.Email = user.Email;
        updatedUser.UserStatus = user.UserStatus;
        updatedUser.Department = user.Department;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
