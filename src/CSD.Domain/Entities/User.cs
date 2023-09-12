using System.ComponentModel.DataAnnotations.Schema;
using CSD.Domain.Enums;

namespace CSD.Domain.Entities;

[Table("user")]
public class User : EntityBase
{
    [Column("first_name")]
    public string FirstName { get; set; } = string.Empty;

    [Column("last_name")]
    public string LastName { get; set; } = string.Empty;

    [Column("paternal_name")]
    public string? PaternalName { get; set; }

    [Column("role")]
    public UserRole Role { get; set; }

    [Column("login")]
    public string Login { get; set; } = string.Empty;

    [Column("password")]
    public string Password { get; set; } = string.Empty;

    [Column("password_salt")]
    public string PasswordSalt { get; set; } = string.Empty;
}
