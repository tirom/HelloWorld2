using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ratings.Controllers
{
    [Produces("application/json")]
    [Route("api/Ratings")]
    public class RatingsController : Controller
    {
		// GET api/Ratings
		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new string[] { "rating1", "rating2" };
		}

		// GET api/Ratings/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "rating";
		}
	}
}