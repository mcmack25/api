using FluentResults;
using ip_test_api.Data.Entities;
using ip_test_api.Models;

namespace ip_test_api.Services;

public interface IUserService
{
    Task<Result<UserEntity>> GetUserAsync(int id, CancellationToken cancellation);
    Task<Result<IEnumerable<UserEntity>>> GetUsersAsync(CancellationToken cancellation);
    Task<Result> UpdateUserAsync(int id, UserDto user, CancellationToken cancellation);
    Task<Result> DeleteUserAsync(int id, CancellationToken cancellation);
    Task<Result> AddUserAsync(UserDto user, CancellationToken cancellation);
}
