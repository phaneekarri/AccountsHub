using System.Security.Cryptography;
using Infra.Notification;
using Microsoft.EntityFrameworkCore;
using UserAuthApi.Dto;
using UserAuthEntities;

namespace UserAuthApi.Services;

public class PasswordResetService : IPasswordResetService
{
    private readonly AuthDBContext _context;
    private readonly INotificationSender _notificationSender;
    private readonly IUserService _userService;

    public PasswordResetService(AuthDBContext context, INotificationSender notificationSender, IUserService userService)
    {
        _context = context;
        _notificationSender = notificationSender;
        _userService = userService;
    }

    public async Task<bool> SendResetTokenAsync(ForgotPasswordRequest request)
    {
        var user = await _userService.Get(request.IdentifierType, request.Identifier);
        if (user == null) return false; // Don't reveal if user exists

        var token = Guid.NewGuid().ToString();
        var resetToken = new PasswordResetToken
        {
            UserId = user.Id,
            Token = token,
            CreatedAt = DateTime.UtcNow
        };

        _context.PasswordResetTokens.Add(resetToken);
        await _context.SaveChangesAsync();

        var message = $"Your password reset token: {token}";
        var result = _notificationSender.send(request.Identifier, message, "Password Reset");
        return result.Status == 200; // Assume success status
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var resetToken = await _context.PasswordResetTokens
            .FirstOrDefaultAsync(t => t.Token == request.Token && t.IsActive);

        if (resetToken == null) return false;

        var user = await _userService.Get(resetToken.UserId);
        if (user == null) return false;

        // Update password (assuming PasswordAuthMethod exists)
        var passwordMethod = user.AuthMethods.OfType<PasswordAuthMethod>().FirstOrDefault();
        if (passwordMethod == null) return false;

        var salt = GenerateSalt();
        passwordMethod.PasswordHash = HashPassword(request.NewPassword, salt);
        passwordMethod.Salt = Convert.ToBase64String(salt);
        passwordMethod.LastPasswordChange = DateTime.UtcNow;

        resetToken.MarkInvalid();
        await _context.SaveChangesAsync();
        return true;
    }

    private static byte[] GenerateSalt(int size = 16)
    {
        var salt = new byte[size];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }

    private static string HashPassword(string password, byte[] salt)
    {
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
        {
            byte[] hash = pbkdf2.GetBytes(32);
            return Convert.ToBase64String(hash);
        }
    }
}