namespace AgoraCallVideo.Dtos
{
    public class UserFCM
    {
        public string Username { get; set; }
        public string TokenFcm { get; set; }
        public List<string> Roles { get; set; }
        public bool IsCalling { get; set; } = false;
    }
}
