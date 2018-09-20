using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Rating.Controllers
{
    [Produces("application/json")]
    [Route("api/Rating")]
    public class RatingController : Controller
    {
		// GET api/Rating
		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new string[] { "rating1", "rating2" };
		}

		// GET api/Rating/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "rating";
		}
	}
}