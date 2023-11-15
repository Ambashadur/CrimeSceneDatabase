using CSD.Domain.Enums;

namespace CSD.DContracts.Users
{
    public class RegisterUserDto
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? PaternalName { get; set; }

        public UserRole Role { get; set; }

        public string Login { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
