using AgoraCallVideo.Extensions;
using AgoraCallVideo.Interfaces;
using AgoraCallVideo.SingleTokens;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgoraCallVideo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FirebaseAdminController : ControllerBase
    {
        private readonly PresenceTracker _presenceTracker;
        private readonly IUnitOfWork _unitOfWork;
        public FirebaseAdminController(PresenceTracker singleTokens, IUnitOfWork unitOfWork) 
        {
            _presenceTracker = singleTokens;
            _unitOfWork = unitOfWork;
        }        

        //[HttpPost("send-notification-specific/{username}")]
        //public async Task<IActionResult> SendNotificationSpecific(string username)
        //{
        //    var userFcm = await _presenceTracker.GetUserforUsername(username);
        //    if (userFcm != null)
        //    {
        //        var message = new Message()
        //        {
        //            Data = new Dictionary<string, string>()
        //            {
        //                { "channel", "test" },
        //                { "time", "2:45" },
        //            },
        //            Token = userFcm.TokenFcm,
        //            Notification = new Notification { Title = "Title test", Body = "body test" }
        //        };

        //        // Send a message to the device corresponding to the provided
        //        // registration token.
        //        string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
        //        // Response is a message ID string.
        //        Console.WriteLine("Successfully sent message: " + response);
        //    }

        //    return NoContent();
        //}

        [HttpPost("tim-tong-dai-vien/{channel}")]
        public async Task<IActionResult> FindUserTongDai(string channel)
        {
            var tongDaiVien = await _presenceTracker.TimTongDaiVien();
            // notification toi tong dai vien
            await SendNotification(User.Identity.Name, tongDaiVien.Username, channel);
            return NoContent();
        }

        private async Task SendNotification(string usernameFrom, string usernameTo, string data)
        {
            var userFcm = await _presenceTracker.GetUserforUsername(usernameTo);
            if (userFcm != null)
            {
                var userFrom = await _unitOfWork.UserRepository.GetUserByUsernameAsync(usernameFrom);
                var message = new Message()
                {
                    Data = new Dictionary<string, string>()
                    {
                        { "channel", data },
                        { "time", "2:45" },
                    },
                    Token = userFcm.TokenFcm,
                    Notification = new Notification { Title = "Cuộc gọi đến", Body = $"Cuộc gọi từ {userFrom.DisplayName}" }
                };

                // Send a message to the device corresponding to the provided
                // registration token.
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                // Response is a message ID string.
                Console.WriteLine("Successfully sent message: " + response);
            }
        }

        [HttpPost("add-token/{token}")]
        public async Task<IActionResult> AddToken(string token)
        {
            var userIdentity = (ClaimsIdentity)User.Identity;
            await _presenceTracker.UserConnected(User.Identity.Name, token, userIdentity.GetRoles());
            return NoContent();
        }

        [HttpGet("user-disconnected")]
        public async Task<IActionResult> UserDisconnected()
        {
            if (User.Identity.IsAuthenticated)
                await _presenceTracker.UserDisconnected(User.Identity.Name);
            return NoContent();
        }

        [HttpGet("set-finish-calling")]
        public async Task<IActionResult> SetFinishCalling()
        {
            // khi user ket thuc cuoc goi hoac cancel cuoc goi, thi set lai = false
            await _presenceTracker.SetCallingForUsername(User.Identity.Name, false);
            return NoContent();
        }

    }
}
