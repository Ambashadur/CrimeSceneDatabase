namespace CSD.Domain.Dto;

public class UserDto
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PaternalName { get; set; }

    public string Login { get; set; } = string.Empty;
}
