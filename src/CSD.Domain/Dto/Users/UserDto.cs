using CSD.Domain.Enums;

namespace CSD.Domain.Dto.Users;

public class UserDto
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? PaternalName { get; set; }

    public long? SceneId { get; set; }

    public UserRole Role { get; set; }

    public string Login { get; set; } = string.Empty;
}
