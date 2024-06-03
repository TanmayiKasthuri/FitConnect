using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace RunGroopWebApp
{
    //Extension-strange looking function
    public static class ClassPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
