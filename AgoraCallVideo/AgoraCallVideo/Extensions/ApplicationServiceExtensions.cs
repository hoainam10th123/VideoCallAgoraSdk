using AgoraCallVideo.Data;
using AgoraCallVideo.Interfaces;
using AgoraCallVideo.Services;
using AgoraCallVideo.SingleTokens;
using Microsoft.EntityFrameworkCore;

namespace AgoraCallVideo.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<PresenceTracker>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAgoraService, AgoraService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}
