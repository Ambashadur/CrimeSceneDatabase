namespace CSD.Domain.Dto;

public class UserDto
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? PaternalName { get; set; }

    public string Login { get; set; } = string.Empty;
}
