using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleService.Models
{
	public class ConfigServerData
	{
		public Logstash Logstash { get; set; }
		public Spring Spring { get; set; }
		public Eureka Eureka { get; set; }
	}

	public class Logstash
	{
		public string Uri { get; set; }
	}

	public class Spring
	{
		public Application Application { get; set; }		
	}

	public class Eureka
	{
		public Instance Instance { get; set; }
	}

	public class Instance
	{
		public int Port { get; set; }
	}

	public class Application
	{
		public string Name { get; set; }
	}
}
