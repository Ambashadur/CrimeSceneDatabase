﻿using CSD.Common;
using CSD.Common.DataAccess;
using CSD.Domain.Dto;
using CSD.Domain.Dto.Users;
using CSD.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CSD.Story.Sso;

public class UserLoginStory : IStory<string, LoginDto>
{
    private readonly IAuthService _authService;
    private readonly IPasswordHashService _passwordHashService;
    private readonly CsdContext _dbContext;
    private readonly ILogger<UserLoginStory> _logger;

    public UserLoginStory(
        IAuthService authService,
        IPasswordHashService passwordHashService,
        CsdContext dbContext,
        ILogger<UserLoginStory> logger) {
        _authService = authService;
        _passwordHashService = passwordHashService;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<string> ExecuteAsync(LoginDto loginDto) {
        var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Login == loginDto.Login);

        if (user is null) {
            _logger.LogError($"User with given login: {loginDto.Login} not found");
            throw new ArgumentException("User not found!");
        }

        if (loginDto.IsAdmin && user.Role != UserRole.Admin) {
            _logger.LogError("User {0} has incorrect role!", user.Login);
            throw new ArgumentException("Incorrect role!");
        }

        var hashedPassword = new HashedPassword() {
            Hash = user.Password,
            Salt = user.PasswordSalt
        };

        if (!_passwordHashService.Verify(hashedPassword, loginDto.Password)) {
            _logger.LogError($"User with login: {loginDto} enter incorrect password");
            throw new ArgumentException("Incorrect password!");
        }

        var scene = await _dbContext.Scenes.FirstOrDefaultAsync(scene => scene.Id == user.SceneId);
        var userDto = new UserDto() {
            Id = user.Id,
            Role = user.Role,
            SceneId = user.SceneId,
            SceneName = scene?.Name,
            Login = loginDto.Login,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PaternalName = user.PaternalName
        };

        var token = await _authService.SignInAsync(userDto);
        _logger.LogInformation($"User with login {loginDto.Login} sign in");

        return token;
    }
}
