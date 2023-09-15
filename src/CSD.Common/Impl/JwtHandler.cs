using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using CSD.Common.Contsants;
using CSD.Common.Settings;
using CSD.Domain.Dto;
using Microsoft.IdentityModel.Tokens;

namespace CSD.Common.Impl;

public class JwtHandler : IJwtHandler
{
    private const string AUTH_TYPE = "Token";

    private readonly IJwtSettings _jwtSettings;

    public JwtHandler(IJwtSettings jwtSettings) {
        _jwtSettings = jwtSettings;
    }

    public UserDto DecodeJwtToken(string userToken) {
        var claims = ValidateToken(userToken, _jwtSettings.Key).Claims;

        return new UserDto() {
            FirstName = claims.FirstOrDefault(x => x.Type == ConstClaims.FIRST_NAME)?.Value,
            LastName = claims.FirstOrDefault(x => x.Type == ConstClaims.LAST_NAME)?.Value,
            PaternalName = claims.FirstOrDefault(x => x.Type == ConstClaims.PATERNAL_NAME)?.Value,
            Login = claims.FirstOrDefault(x => x.Type == ConstClaims.LOGIN)?.Value ?? string.Empty,
        };
    }

    public string GenerateJwtToken(UserDto? userDto) {
        if (userDto is null) {
            throw new ArgumentException("UserDto must be not null!");
        }

        var claimsPrincipal = GetClaimsPrincipal(userDto);

        var dateTimeNow = DateTime.UtcNow;
        var jwt = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            notBefore: dateTimeNow,
            claims: claimsPrincipal.Claims,
            expires: dateTimeNow.Add(_jwtSettings.JwtLifeTime),
            signingCredentials: new SigningCredentials(
                _jwtSettings.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public ClaimsPrincipal GetClaimsPrincipal(UserDto userDto) {
        var claims = new List<Claim>() {
            new(ConstClaims.FIRST_NAME, userDto.FirstName ?? string.Empty),
            new(ConstClaims.LAST_NAME, userDto.LastName ?? string.Empty),
            new(ConstClaims.PATERNAL_NAME, userDto.PaternalName ?? string.Empty),
            new(ConstClaims.LOGIN, userDto.Login),
        };

        return new ClaimsPrincipal(new ClaimsIdentity(claims, AUTH_TYPE));
    }

    private static JwtSecurityToken ValidateToken(string token, string jwtKey) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtKey);

        tokenHandler.ValidateToken(
            token,
            new TokenValidationParameters() {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
            },
            out SecurityToken validateToken);

        return validateToken as JwtSecurityToken;
    }
}
