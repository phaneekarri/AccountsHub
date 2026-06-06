using UserAuthApi.Dto;

namespace UserAuthApi.Services;

public interface IProfileService
{
    Task<ProfileDto?> GetProfileAsync(Guid userId);
    Task<bool> UpdateProfileAsync(Guid userId, UpdateProfileRequest request);
}