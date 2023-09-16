using CSD.Domain.Dto.Users;

namespace CSD.Common;

public interface IUserContext
{
    UserDto CurrentUser { get; }

    void SetCurrentUser(UserDto userDto);
}
