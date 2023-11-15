using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CSD.Contracts.Users;

namespace CSD.Common.Impl;

public class UserTokenService : IUserTokenService
{
    private readonly ConcurrentDictionary<string, UserDto> _tokenCache = new();

    public Task DeleteAsync(string userToken) {
        if (!_tokenCache.TryRemove(userToken, out _)) {
            throw new ArgumentException();
        }

        return Task.CompletedTask;
    }

    public Task<UserDto?> GetAsync(string userToken) {
        if (_tokenCache.TryGetValue(userToken, out var userDto)) {
            return Task.FromResult(userDto);
        }

        return Task.FromResult<UserDto?>(null);
    }

    public Task SetAsync(string userToken, UserDto userDto) {
        if (!_tokenCache.TryAdd(userToken, userDto)) {
            throw new ArgumentException();
        }

        return Task.CompletedTask;
    }
}
