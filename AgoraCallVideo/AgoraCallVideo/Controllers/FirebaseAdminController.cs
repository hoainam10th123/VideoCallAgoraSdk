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

        [HttpPost("tim-tong-dai-vien/{data}")]
        public async Task<IActionResult> FindUserTongDai(string data)//data is json {channel, username}
        {
            var tongDaiVien = await _presenceTracker.TimTongDaiVien();
            if (tongDaiVien == null) await SendNotificationBackToSender(User.Identity.Name, "Tổng đài bận", "Hiện tại các tổng đài viên đều bận", "data");// tong dai ban
            // notification toi tong dai vien
            else await SendNotification(User.Identity.Name, tongDaiVien.Username, data);                        
            return NoContent();
        }

        [HttpGet("get-tu-choi/{username}")]
        public async Task<IActionResult> TuChoiCuocGoi(string username)
        {
            await SendNotificationDenyCall(username, "data");
            return NoContent();
        }

        private async Task SendNotificationDenyCall(string usernameTo, string data)
        {
            var userFcm = await _presenceTracker.GetUserforUsername(usernameTo);
            if (userFcm != null)
            {                
                var message = new Message()
                {
                    Data = new Dictionary<string, string>()
                    {
                        { "statusCode", "DENY" },
                        { "responseData", data },
                        { "usernameTo", usernameTo },
                    },
                    Token = userFcm.TokenFcm,
                    Notification = new Notification { Title = "Từ chối", Body = "Tổng đài viên từ chối cuộc gọi" }
                };

                // Send a message to the device corresponding to the provided
                // registration token.
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                // Response is a message ID string.
                Console.WriteLine("Successfully sent message: " + response);
            }
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
                        { "statusCode", "ONE_ONE" },
                        { "responseData", data },
                        { "usernameTo", usernameTo },
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

        private async Task SendNotificationBackToSender(string usernameFrom, string titleNotification, string bodyNotification, string data)
        {
            var userFcm = await _presenceTracker.GetUserforUsername(usernameFrom);
            if (userFcm != null)
            {
                var message = new Message()
                {
                    Data = new Dictionary<string, string>()
                    {
                        { "statusCode", "CALLER" },
                        { "responseData", data },
                        { "usernameTo", usernameFrom },
                    },
                    Token = userFcm.TokenFcm,
                    Notification = new Notification { Title = titleNotification, Body = bodyNotification }
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

        [AllowAnonymous]
        [HttpGet("user-disconnected")]
        public async Task<IActionResult> UserDisconnected()
        {
            if (User.Identity.IsAuthenticated)
                await _presenceTracker.UserDisconnected(User.Identity.Name);
            return NoContent();
        }

        [HttpGet("set-calling/{iscalling}")]
        public async Task<IActionResult> SetFinishCalling(bool iscalling)
        {
            // khi user ket thuc cuoc goi hoac cancel cuoc goi, thi set lai = false
            await _presenceTracker.SetCallingForUsername(User.Identity.Name, iscalling);
            return NoContent();
        }

    }
}
