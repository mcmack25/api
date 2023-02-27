using ip_test_api.Data.Enums;

namespace ip_test_api.Data.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }

        public string UserName { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public UserStatusType UserStatus { get; set; }

        public string? Department { get; set; }
    }
}
