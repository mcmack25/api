using ip_test_api.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace ip_test_api.Models
{
    public class UserDto
    {
        [Required]
        [StringLength(50)]
        public string UserName { get; set; } = null!;

        [StringLength(255)]
        public string FirstName { get; set; } = null!;

        [StringLength(255)]
        public string LastName { get; set; } = null!;

        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public UserStatusType UserStatus { get; set; }

        [StringLength(255)]
        public string? Department { get; set; }
    }
}
