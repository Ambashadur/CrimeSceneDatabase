using Common;
using Domain.Dto;

namespace Story.User.Impl;

public class UserStories : IUserStories
{
    private readonly IAuthService _authService;

    public UserStories(IAuthService authService) {
        _authService = authService;
    }

    public async Task<string> LoginAsync(LoginDto loginDto) {
        var testUser = new UserDto() {
            FirstName = "Testivan",
            LastName = "Testivanov",
            PaternalName = "Testivanovich",
            Login = loginDto.Login
        };

        var token = await _authService.SignInAsync(testUser);

        return token;
    }
}
