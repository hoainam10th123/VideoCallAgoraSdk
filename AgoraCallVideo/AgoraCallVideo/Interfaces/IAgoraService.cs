using AgoraCallVideo.Dtos;

namespace AgoraCallVideo.Interfaces
{
    public interface IAgoraService
    {
        Task<string> CreateRtcToken(AppSetting setting);
    }
}
