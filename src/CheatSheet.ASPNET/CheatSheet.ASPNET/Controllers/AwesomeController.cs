using System.Web.Http;
using System.Web.Security;

namespace CheatSheet.ASPNET.Controllers
{
    // Broken access control
    [Authorize]
    public class AwesomeController : ApiController
    {
        [Authorize]
        public string Get(int id)
        {
            if (Roles.IsUserInRole("User"))
            {
                return "User value";
            }
            else if (Roles.IsUserInRole("Administrator"))
            {
                return "Administrator value";
            }

            return "value";
        }

        [Authorize(Roles = "Users")]
        public string GetAsUser(int id)
        {
            return "value";
        }

        [AllowAnonymous]
        public string GetAnonymous(int id)
        {
            return "value";
        }
    }
}
