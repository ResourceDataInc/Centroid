using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace Centroid
{
    public class Config : DynamicObject
    {
        private readonly dynamic _rawConfig;

        public dynamic Json
        {
            get;
            private set;
        }

        public string Environment
        {
            get;
            private set;
        }

        public string Root
        {
            get;
            private set;
        }

        public Config(string json)
        {
            _rawConfig = JObject.Parse(json);
            Root = "root";
        }

        public Config(dynamic config, string env, string root = "root")
        {
            Json = config;
            Environment = env;
            Root = root;
        }

        public static Config FromFile(string fileName)
        {
            string json = System.IO.File.ReadAllText(fileName);
            return new Config(json);
        }

        public static Config FromString(string json)
        {
            return new Config(json);
        }

        public dynamic GetEnvironmentConfig(string environment)
        {
            var envConfig = _rawConfig[environment];
            var allConfig = _rawConfig["All"];

            foreach (var cfg in allConfig)
            {
                envConfig[cfg.Name] = cfg.Value;
            }

            return new Config(envConfig, environment);
        }    

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            try
            {
                result = this.GetValue(binder.Name);
                if (result != null)
                {
                    result = new Config(result, this.Environment, binder.Name);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (binder.Type == typeof(string))
            {
                result = ToString();
                return true;
            }
            else
            {
                try
                {
                    var T = Json.GetType();
                    result = Convert.ChangeType(Json, binder.Type);
                    return true;
                }
                catch (InvalidCastException)
                {
                    return base.TryConvert(binder, out result);
                }    
            }
        }

        public override string ToString()
        {
            return Json.ToString();
        }

        private string NormaliseKey(string key)
        {
            return key.Replace("_", String.Empty).ToLower();
        }

        private dynamic GetValue(string key)
        {
            key = GetActualKey(key);
            return Json[key];
        }

        private string GetActualKey(string key)
        {
            var properties = this.Json.Properties() as IEnumerable<dynamic>;
            var keys = properties.Select(property => property.Name);
            return keys
                .Single(m => NormaliseKey(m) == NormaliseKey(key));
        }
    }
}

