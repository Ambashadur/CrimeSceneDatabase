using CSD.Common;
using CSD.Common.DataAccess;
using CSD.Domain.Dto.Users;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CSD.Story.Sso;

public class RegisterUserStory : IStory<UserDto, RegisterUserDto>
{
    private readonly CsdContext _context;
    private readonly ILogger<RegisterUserStory> _logger;
    private readonly IPasswordHashService _passwordHashService;

    public RegisterUserStory(
        CsdContext context,
        ILogger<RegisterUserStory> logger,
        IPasswordHashService passwordHashService) {
        _context = context;
        _logger = logger;
        _passwordHashService = passwordHashService;
    }

    public async Task<UserDto> ExecuteAsync(RegisterUserDto registerUserDto) {
        if (_context.Users.Any(user => user.Login == registerUserDto.Login)) {
            _logger.LogError($"User with login {registerUserDto.Login} try register but user with same login already exist!");
            throw new ArgumentException($"User with login {registerUserDto.Login} already exist!");
        }

        var hashedPassword = _passwordHashService.Hash(registerUserDto.Password);

        var user = await _context.Users.AddAsync(new Domain.Entities.User() {
            CreateDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow,
            FirstName = registerUserDto.FirstName,
            LastName = registerUserDto.LastName,
            PaternalName = registerUserDto.PaternalName,
            Role = registerUserDto.Role,
            Login = registerUserDto.Login,
            Password = hashedPassword.Hash,
            PasswordSalt = hashedPassword.Salt
        });

        await _context.SaveChangesAsync();

        _logger.LogInformation($"User with login {user.Entity.Login} was created");
        return new UserDto() {
            Login = user.Entity.Login,
            FirstName = user.Entity.FirstName,
            LastName = user.Entity.LastName,
            PaternalName = user.Entity.PaternalName
        };
    }
}
