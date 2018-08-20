using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTest.Services
{
    public interface IValue
    {
		Task<List<string>> GetValuesAsync();
	}
}
