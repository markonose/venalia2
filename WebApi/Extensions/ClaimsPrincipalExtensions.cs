using System;
using System.Security.Claims;

namespace WebApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetId(this ClaimsPrincipal user)
        {
            return new Guid(user?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "00000000000000000000000000000001");
        }

        public static bool IsAdministrator(this ClaimsPrincipal user)
        {
            return user.HasRole("Administrator");
        }

        public static bool IsBusiness(this ClaimsPrincipal user)
        {
            return user.HasRole("Business");
        }

        public static bool IsInstaller(this ClaimsPrincipal user)
        {
            return user.HasRole("Installer");
        }

        public static bool IsActive(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Role) == "Active";
        }

        public static bool HasRole(this ClaimsPrincipal user, string role)
        {
            return user.FindFirstValue(ClaimTypes.Role) == role;
        }
    }
}
