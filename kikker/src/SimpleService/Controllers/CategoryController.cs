using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using SimpleService.Models;

namespace DownstreamService.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
		private IOptionsSnapshot<ConfigServerData> IConfigServerData { get; set; }
		
		public CategoryController(IOptionsSnapshot<ConfigServerData> configServerData) {
			if (configServerData != null)
				IConfigServerData = configServerData;
			
		}
		
		// GET api/category
		[HttpGet]
        public IEnumerable<string> Get()
        {
			Log.Information($"{IConfigServerData?.Value?.Spring.Application.Name} {IConfigServerData?.Value?.Eureka.Instance.Port} - GET api/category");
			return new[] { "category1", "category2" };
        }
    }
}
