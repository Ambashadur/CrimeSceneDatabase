using System;
using CSD.Contracts.Users;

namespace CSD.Common.Impl;

public class UserContext : IUserContext
{
    public UserDto CurrentUser { get; private set; }

    public void SetCurrentUser(UserDto userDto) {
        if (CurrentUser is not null) {
            throw new ArgumentException();
        }

        CurrentUser = userDto;
    }
}
