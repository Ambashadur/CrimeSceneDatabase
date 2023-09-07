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
            FirstName = claims.FirstOrDefault(x => x.Type == ConstClaims.FIRST_NAME)?.Value ?? string.Empty,
            LastName = claims.FirstOrDefault(x => x.Type == ConstClaims.LAST_NAME)?.Value ?? string.Empty,
            PaternalName = claims.FirstOrDefault(x => x.Type == ConstClaims.PATERNAL_NAME)?.Value ?? string.Empty,
            Login = claims.FirstOrDefault(x => x.Type == ConstClaims.LOGIN)?.Value ?? string.Empty,
        };
    }

    public string GenerateJwtToken(UserDto? userDto) {
        var claimsPrincipal = GetClaimsPrincipal(userDto);

        if (claimsPrincipal is null) {
            throw new ArgumentException();
        }

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

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt;
    }

    public ClaimsPrincipal? GetClaimsPrincipal(UserDto? userDto) {
        if (userDto is null) return null;

        var claims = new List<Claim>() {
            new(ConstClaims.FIRST_NAME, userDto.FirstName),
            new(ConstClaims.LAST_NAME, userDto.LastName),
            new(ConstClaims.PATERNAL_NAME, userDto.PaternalName),
            new(ConstClaims.LOGIN, userDto.Login),
        };

        return new ClaimsPrincipal(new ClaimsIdentity(claims, AUTH_TYPE));
    }

    private JwtSecurityToken ValidateToken(string token, string jwtKey) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtKey);

        tokenHandler.ValidateToken(
            token,
            new TokenValidationParameters() {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
            },
            out SecurityToken validateToken);

        return validateToken as JwtSecurityToken;
    }
}
