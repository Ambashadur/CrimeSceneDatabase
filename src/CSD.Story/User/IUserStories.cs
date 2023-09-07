using System.Threading.Tasks;
using CSD.Domain.Dto;

namespace CSD.Story.User;

public interface IUserStories
{
    Task<string> LoginAsync(LoginDto loginDto);
}
