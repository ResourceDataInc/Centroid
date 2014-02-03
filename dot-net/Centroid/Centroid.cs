using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroidConfig
{
    public class Centroid
    {
        string fileName;
        dynamic rawConfig;

        public Centroid(string fileName)
        {
            this.fileName = fileName;
            string json = System.IO.File.ReadAllText(this.fileName);
            rawConfig = JObject.Parse(json);
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
