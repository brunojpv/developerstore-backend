using DeveloperStore.Domain.Enum;

namespace DeveloperStore.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public Address Address { get; set; } = new();
        public Name Name { get; set; } = new();
        public UserStatus Status { get; set; }
        public UserRole Role { get; set; }
    }
}
