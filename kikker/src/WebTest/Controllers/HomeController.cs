using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebTest.Models;
using WebTest.Services;

namespace WebTest.Controllers
{
    public class HomeController : Controller
    {
		
		private readonly IValue ValueService;

		public HomeController(IValue valueService)
		{
			ValueService = valueService;			
		}

		public async Task<IActionResult> Index()
        {
			if (ValueService != null)
			{
				var values = await ValueService.GetValuesAsync();
				ViewData["Values"] = values.Count > 0 ? values[0] : "No value from api";


				var viewModel = new HomeViewModel()
				{

					Name = values[0]

				};

				return View(viewModel);
			}
			return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
