using CSD.Common;
using CSD.Common.DataAccess;
using CSD.Domain.Dto;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CSD.Story.Users.Impl;

public class RegisterUserStory : IRegisterUserStory
{
    private readonly CsdContext _context;
    private readonly ILogger<RegisterUserStory> _logger;
    private readonly IPasswordHashService _passwordHashService;

    public RegisterUserStory(CsdContext context, ILogger<RegisterUserStory> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<UserDto> ExecuteAsync(RegisterUserDto registerUserDto)
    {
        if (_context.Users.Any(user => user.Login == registerUserDto.Login))
        {
            _logger.LogError($"User with login {registerUserDto.Login} try register but user with same login already exist!");
            throw new ArgumentException($"User with login {registerUserDto.Login} already exist!");
        }

        var hashedPassword = _passwordHashService.Hash(registerUserDto.Password);

        var user = await _context.Users.AddAsync(new Domain.Entities.User()
        {
            CreateDate = DateTimeOffset.Now,
            UpdateDate = DateTimeOffset.Now,
            FirstName = registerUserDto.FirstName,
        });
    }
}
