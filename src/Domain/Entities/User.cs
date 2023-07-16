namespace Domain.Entities;

public class User : EntityBase
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string PaternalName { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }
}
