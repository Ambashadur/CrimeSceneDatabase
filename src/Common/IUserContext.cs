using Domain.Dto;

namespace Common;

public interface IUserContext
{
    UserDto CurrentUser { get; }

    void SetCurrentUser(UserDto userDto);
}
