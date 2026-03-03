using MalaebBooking.Application.Contracts.Auth;
using MalaebBooking.Domain.Entities;
using MalaebBooking.Infrastructure.Authentication;
using Microsoft.AspNetCore.Identity;

namespace MalaebBooking.Application.Services;

public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider jwtProvider = jwtProvider;

    public async Task<AuthResponse?> GetTokenAsync(
        string email,
        string password,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return null;

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);

        if (!isPasswordValid)
            return null;

        var (token, expiration) = jwtProvider.GenerateToken(user);
        return new AuthResponse
        {
            Id = user.Id,
            Name = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Token = token,
            ExpiresIn = (int)(expiration - DateTime.UtcNow).TotalSeconds
        };

    }
}