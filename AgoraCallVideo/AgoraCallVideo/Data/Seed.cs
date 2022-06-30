using AgoraCallVideo.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AgoraCallVideo.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;
            //them role
            var roles = new List<AppRole>
            {
                new AppRole{Name = "NhanVien"},
                new AppRole{Name = "TongDaiVien"},
                new AppRole{Name = "KhachHang"},
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            // them user tong dai
            var usersTongDai = new List<AppUser>()
            {
                new AppUser{ UserName = "hoainam10th", DisplayName = "Nguyen Hoai Nam"},
                new AppUser{ UserName = "ubuntu", DisplayName = "Nguyen Tan An"},
                new AppUser{ UserName = "lisa", DisplayName = "Lisa"},
            };

            foreach (var user in usersTongDai)
            {
                await userManager.CreateAsync(user, "123456");
                await userManager.AddToRoleAsync(user, "TongDaiVien");
            }

            // them user khach hang
            var usersKhachHang = new List<AppUser>()
            {
                new AppUser{ UserName = "tony", DisplayName = "Tony Nguyen"},
                new AppUser{ UserName = "bob", DisplayName = "Bob"}
            };

            foreach (var user in usersKhachHang)
            {
                await userManager.CreateAsync(user, "123456");
                await userManager.AddToRoleAsync(user, "KhachHang");
            }
        }
    }
}
