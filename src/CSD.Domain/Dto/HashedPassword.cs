namespace CSD.Domain.Dto;

public class HashedPassword
{
    public string Hash { get; set; } = string.Empty;

    public string Salt { get; set; } = string.Empty;
}
