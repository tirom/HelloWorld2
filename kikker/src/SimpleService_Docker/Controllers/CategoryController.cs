using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
//using Serilog;
using SimpleService.Models;

namespace DownstreamService.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
		private readonly ILogger _logger;
		private IOptionsSnapshot<ConfigServerData> IConfigServerData { get; set; }
				
		public CategoryController(ILogger<CategoryController> logger, IOptionsSnapshot<ConfigServerData> configServerData) {
			_logger = logger;
			if (configServerData != null)
				IConfigServerData = configServerData;			
		}
		
		// GET api/category
		[HttpGet]
        public IEnumerable<string> Get()
        {
			_logger.LogInformation($"{IConfigServerData?.Value?.Spring.Application.Name} {IConfigServerData?.Value?.Eureka.Instance.Port} - GET api/category");
			return new[] { "category1", "category2" };
        }
    }
}
