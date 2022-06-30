using System.Security.Claims;

namespace AgoraCallVideo.Extensions
{
    public static class RolesServiceExtensions
    {
        /// <summary>
        /// Get roles current user
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static List<string> GetRoles(this ClaimsIdentity identity)
        {
            return identity.Claims
                           .Where(c => c.Type == ClaimTypes.Role)
                           .Select(c => c.Value)
                           .ToList();
        }
    }
}
