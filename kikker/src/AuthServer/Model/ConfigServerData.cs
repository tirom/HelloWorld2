using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Model
{
    public class ConfigServerData
    {
        public string Url { get; set; }
        public Audience Audience { get; set; }

    }

    public class Audience
    {
        public string Secret { get; set; }
        public string Iss { get; set; }
        public string Aud { get; set; }
    }
}
