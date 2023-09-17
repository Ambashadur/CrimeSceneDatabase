using CSD.Domain.Enums;

namespace CSD.Story.Users;

public class GetUsersPageContext : GetPageContext
{
    public UserRole Role { get; set; }
}
