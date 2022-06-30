using AgoraCallVideo.Entities;

namespace AgoraCallVideo.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(AppUser appUser);
    }
}
