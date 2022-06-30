using AgoraCallVideo.Dtos;

namespace AgoraCallVideo.SingleTokens
{
    public class PresenceTracker
    {
        //mỗi user chỉ được phép sử dụng 1 trình duyệt để kết nối vào server, OnlineUsers = key, value
        private static readonly Dictionary<string, UserFCM> OnlineUsers = new Dictionary<string, UserFCM>();

        public Task<bool> UserConnected(string username, string token, List<string> roles)
        {
            bool isOnline = false;
            lock (OnlineUsers)
            {                
                if (OnlineUsers.ContainsKey(username))
                {
                    // vì mỗi user chỉ được sử dụng 1 trình duyệt,
                    // nếu đăng nhập vào trình duyệt thứ 2 thì cái thứ nhất ko gửi được notification
                    // thay thế token cũ bằng token mới, lấy được ở phía client, Firebase Cloud Messaging sử dụng
                    OnlineUsers[username].TokenFcm = token;
                }
                else
                {
                    OnlineUsers.Add(username, new UserFCM { Username = username, TokenFcm = token, Roles = roles });
                    isOnline = true;                    
                }
            }

            return Task.FromResult(isOnline);
        }

        public Task<bool> UserDisconnected(string username)
        {
            bool isOffline = false;
            lock (OnlineUsers)
            {
                if (OnlineUsers.ContainsKey(username))
                {
                    OnlineUsers.Remove(username);
                    isOffline = true;
                }
            }
            return Task.FromResult(isOffline);
        }

        /// <summary>
        /// Tra ve null neu khong ton tai username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Tra ve null neu khong ton tai username</returns>
        public Task<UserFCM> GetUserforUsername(string username)
        {
            UserFCM user = null;
            lock (OnlineUsers)
            {
                if (OnlineUsers.ContainsKey(username))
                {
                    user = OnlineUsers[username];
                }
            }
            return Task.FromResult(user);
        }

        /// <summary>
        /// Mark user is calling
        /// </summary>
        /// <param name="username">current username</param>
        /// <param name="isCalling">True = user dang co cuoc goi, false = user khong co cuoc goi</param>
        /// <returns></returns>
        public Task SetCallingForUsername(string username, bool isCalling)
        {
            lock (OnlineUsers)
            {
                if (OnlineUsers.ContainsKey(username))
                {
                    OnlineUsers[username].IsCalling = isCalling;
                }
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// tim tong dai vien dang khong co cuoc goi
        /// </summary>
        /// <returns></returns>
        public Task<UserFCM> TimTongDaiVien()
        {
            UserFCM user = null;
            lock (OnlineUsers)
            {
                var temp = OnlineUsers.FirstOrDefault(x => x.Value.Roles.Contains("TongDaiVien") && x.Value.IsCalling == false);                
                if(temp.Value != null)
                {
                    user = temp.Value;
                    temp.Value.IsCalling = true;
                }                    
            }
            return Task.FromResult(user);
        }
    }
}
