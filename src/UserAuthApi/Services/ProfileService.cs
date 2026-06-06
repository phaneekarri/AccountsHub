using UserAuthApi.Dto;
using UserAuthEntities;

namespace UserAuthApi.Services;

public class ProfileService : IProfileService
{
    private readonly AuthDBContext _context;

    public ProfileService(AuthDBContext context)
    {
        _context = context;
    }

    public async Task<ProfileDto?> GetProfileAsync(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return null;

        return new ProfileDto(user.Id, user.UserName, user.Email, user.Phone);
    }

    public async Task<bool> UpdateProfileAsync(Guid userId, UpdateProfileRequest request)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;

        if (!string.IsNullOrEmpty(request.UserName)) user.UserName = request.UserName;
        if (!string.IsNullOrEmpty(request.Email)) user.Email = request.Email;
        if (!string.IsNullOrEmpty(request.Phone)) user.Phone = request.Phone;

        await _context.SaveChangesAsync();
        return true;
    }
}