using AgoraCallVideo.Dtos;
using AgoraCallVideo.Extensions;
using AgoraCallVideo.Interfaces;
using AgoraIO.Media;

namespace AgoraCallVideo.Services
{
    public class AgoraService : IAgoraService
    {
        private readonly IConfiguration _config;
        public AgoraService(IConfiguration config)
        {
            _config = config;
        }

        public Task<string> CreateRtcToken(AppSetting setting)
        {
            var token = new AccessToken(_config["AgoraAppSettings:AppId"],
                _config["AgoraAppSettings:AppCertificate"], 
                setting.ChannelName, 
                setting.Uid);

            token.addPrivilege(Privileges.kJoinChannel, DateTime.Now.AddDays(1).ToDoUInt32DateTime());
            string result = token.build();
            return Task.FromResult(result);
        }
    }
}
