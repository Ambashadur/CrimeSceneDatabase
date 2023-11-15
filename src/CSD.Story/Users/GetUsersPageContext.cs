using CSD.Contracts;
using CSD.Domain.Enums;

namespace CSD.Story.Users;

public class GetUsersPageContext : PageContext
{
    public UserRole Role { get; set; }
}
