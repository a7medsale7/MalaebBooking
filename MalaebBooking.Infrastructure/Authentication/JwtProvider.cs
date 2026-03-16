using MalaebBooking.Domain.Consts;
using MalaebBooking.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace MalaebBooking.Infrastructure.Authentication;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions options = options.Value;

    public (string Token, DateTime Expiration) GenerateToken(ApplicationUser user , IEnumerable<string> roles ,IEnumerable<string> permissions )
    {
        var key = options.Key;
        var issuer = options.Issuer;
        var audience = options.Audience;
        var expiryMinutes = options.ExpiryMinutes;

        var expiration = DateTime.UtcNow.AddMinutes(expiryMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        // إضافة الرولز بشكل منفصل (Standard ASP.NET Core Roles)
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        // إضافة البرمشنز بشكل منفصل (Custom Permission Policy)
        claims.AddRange(permissions.Select(permission => new Claim(Permissions.Type, permission)));

        // اختياري: إضافة الرولز والبرمشنز كـ Array لخدمة أي منطق في الـ Frontend لو محتاج
        claims.Add(new Claim("roles", JsonSerializer.Serialize(roles), JsonClaimValueTypes.JsonArray));
        claims.Add(new Claim("permissions", JsonSerializer.Serialize(permissions), JsonClaimValueTypes.JsonArray));

        var symmetricSecurityKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key));

        var signingCredentials =
            new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: options.Issuer,
            audience: options.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiration,
            signingCredentials: signingCredentials
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return (token, expiration);
    }

    public string? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey( Encoding.UTF8.GetBytes(options.Key));
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidIssuer = options.Issuer,
                ValidateAudience = false,
                ValidAudience = options.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;
            return userId;

        }
        catch
        {
            return null;
        }

    }
}