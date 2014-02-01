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
        string allConfig_key;

        public Centroid(string fileName = "config.json", string AllConfig_key = "All")
        {
            this.fileName = fileName;
            this.allConfig_key = AllConfig_key;
        }

        public dynamic environment(string env)
        {
            string json = System.IO.File.ReadAllText("../../../../" + this.fileName);
            dynamic rawConfig = JObject.Parse(json);
            dynamic envConfig = rawConfig[env];
            dynamic allConfig = rawConfig[this.allConfig_key];

            foreach (var cfg in allConfig)
            {
                envConfig[cfg.Name] = cfg.Value;
            }

            return new Config(envConfig, env);
        }
            
    }
}
