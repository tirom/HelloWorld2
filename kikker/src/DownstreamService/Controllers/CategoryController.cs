using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DownstreamService.Controllers
{
    [Route("api/[controller]")]
    public class Category2Controller : Controller
    {
        // GET api/values
        [Authorize(Policy = "UserRole")]
        [HttpGet]
        public IActionResult Get()
        {
            string[] s = new string[] {"iss", "aud", "username", "exp", "nbf" };
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                return Json(claims/*.Where(c => s.Contains(c.Type.ToLower()))*/.ToDictionary(c => c.Type, x => x.Value));
            }
            return null;
        }

    }
}
