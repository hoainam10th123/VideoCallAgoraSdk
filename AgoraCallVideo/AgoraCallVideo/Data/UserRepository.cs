using AgoraCallVideo.Entities;
using AgoraCallVideo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgoraCallVideo.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.UserName == username);
        }
    }
}
