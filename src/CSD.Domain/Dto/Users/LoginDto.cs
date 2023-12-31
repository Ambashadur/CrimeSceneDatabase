﻿namespace CSD.Domain.Dto.Users;

public class LoginDto
{
    public string Login { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public bool IsAdmin { get; set; } = false;
}
