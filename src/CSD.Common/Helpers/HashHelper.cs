using System;
using System.Security.Cryptography;
using System.Text;

namespace CSD.Common.Helpers;

public static class HashHelper
{
    public static string ComputeHash(string value) {
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(value));

        return Convert.ToHexString(hash);
    }
}
