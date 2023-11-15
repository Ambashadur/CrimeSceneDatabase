using CSD.Contracts;

namespace CSD.Common;

public interface IPasswordHashService
{
    HashedPassword Hash(string password);

    bool Verify(HashedPassword hashedPassword, string password);
}
