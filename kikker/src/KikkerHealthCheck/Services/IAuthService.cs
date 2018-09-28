using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kikker.HealthCheck.Services
{
    public interface IAuthService
    {
		Task<HttpResponseMessage> GetAuthenticationAsync(string username, string password);
	}
}
