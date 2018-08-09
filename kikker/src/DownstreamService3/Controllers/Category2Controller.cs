using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DownstreamService3.Controllers
{
    [Route("api/[controller]")]
    public class Category2Controller : Controller
    {
        // GET api/values
        [Authorize(Policy = "UserRole")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "category6", "category5" };
        }
    }
}
