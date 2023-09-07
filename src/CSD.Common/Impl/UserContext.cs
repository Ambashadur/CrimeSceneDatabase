using System;
using CSD.Domain.Dto;

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
