using System.Threading.Tasks;
using CSD.Domain.Dto;

namespace CSD.Story.Users;

public interface IUserLoginStory
{
    Task<string> ExecuteAsync(LoginDto loginDto);
}
