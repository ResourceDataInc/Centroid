using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centroid
{
    public class Centroid
    {
        dynamic rawConfig;

        public Centroid(string json)
        {
            rawConfig = JObject.Parse(json);
        }

        public static Centroid FromFile(string fileName)
        {
            string json = System.IO.File.ReadAllText(fileName);
            return new Centroid(json);
        }

        public dynamic Environment(string env)
        {            
            dynamic envConfig = rawConfig[env];
            dynamic allConfig = rawConfig["All"];

            foreach (var cfg in allConfig)
            {
                envConfig[cfg.Name] = cfg.Value;
            }

            return new Config(envConfig, env);
        }            
    }
}
