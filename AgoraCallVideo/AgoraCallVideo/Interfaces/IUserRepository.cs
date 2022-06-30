using AgoraCallVideo.Entities;

namespace AgoraCallVideo.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserByUsernameAsync(string username);
    }
}
