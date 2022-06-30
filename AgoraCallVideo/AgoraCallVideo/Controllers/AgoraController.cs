using AgoraCallVideo.Dtos;
using AgoraCallVideo.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgoraCallVideo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AgoraController : ControllerBase
    {
        private readonly IAgoraService _agoraService;
        public AgoraController(IAgoraService agoraService)
        {
            _agoraService = agoraService;
        }

        [HttpPost("get-rtc-token")]
        public async Task<IActionResult> GetRtcToken(AppSetting setting)
        {
            var token = await _agoraService.CreateRtcToken(setting);
            return Ok(token);
        }
    }
}
