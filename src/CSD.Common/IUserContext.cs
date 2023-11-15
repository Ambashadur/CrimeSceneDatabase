using CSD.Contracts.Users;

namespace CSD.Common;

public interface IUserContext
{
    UserDto CurrentUser { get; }

    void SetCurrentUser(UserDto userDto);
}
