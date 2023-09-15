using CSD.Domain.Dto;
using System.Threading.Tasks;

namespace CSD.Story.Users;

public interface IRegisterUserStory
{
    Task<UserDto> ExecuteAsync(RegisterUserDto registerUserDto);
}
