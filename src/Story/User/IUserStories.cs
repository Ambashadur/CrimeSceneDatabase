using Domain.Dto;

namespace Story.User;

public interface IUserStories
{
    Task<string> LoginAsync(LoginDto loginDto);
}
