using Microsoft.EntityFrameworkCore;
using UserAuthEntities;

namespace UserAuthApi.Services;

public class TokenRevocationService : ITokenRevocationService
{
    private readonly AuthDBContext _context;

    public TokenRevocationService(AuthDBContext context)
    {
        _context = context;
    }

    public async Task<bool> RevokeTokenAsync(string token)
    {
        var accessToken = await _context.UserAccessTokens
            .FirstOrDefaultAsync(t => t.Token == token && t.isActive);

        if (accessToken == null) return false;

        accessToken.MarkInvalid();
        await _context.SaveChangesAsync();
        return true;
    }
}