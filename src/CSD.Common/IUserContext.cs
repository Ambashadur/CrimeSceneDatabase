using CSD.Domain.Dto;

namespace CSD.Common;

public interface IUserContext
{
    UserDto CurrentUser { get; }

    void SetCurrentUser(UserDto userDto);
}
