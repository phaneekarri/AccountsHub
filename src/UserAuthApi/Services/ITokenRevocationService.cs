namespace UserAuthApi.Services;

public interface ITokenRevocationService
{
    Task<bool> RevokeTokenAsync(string token);
}